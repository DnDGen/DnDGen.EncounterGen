using DnDGen.EncounterGen.Generators;
using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Selectors.Collections;
using DnDGen.EncounterGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Unit.Generators
{
    [TestFixture]
    public class EncounterVerifierTests
    {
        private const int Level = 10;
        private const string Environment = "environment";
        private const string Temperature = "temperature";
        private const string TimeOfDay = "time of day";
        private const string Filter = "filter";

        private IEncounterVerifier encounterVerifier;
        private Mock<ICollectionSelector> mockCollectionSelector;
        private Mock<IEncounterCollectionSelector> mockEncounterCollectionSelector;
        private Mock<IEncounterLevelSelector> mockAmountSelector;
        private List<string> filterGroup;
        private EncounterSpecifications specifications;

        [SetUp]
        public void Setup()
        {
            mockCollectionSelector = new Mock<ICollectionSelector>();
            mockEncounterCollectionSelector = new Mock<IEncounterCollectionSelector>();
            mockAmountSelector = new Mock<IEncounterLevelSelector>();
            encounterVerifier = new EncounterVerifier(mockCollectionSelector.Object, mockEncounterCollectionSelector.Object, mockAmountSelector.Object);

            specifications = new EncounterSpecifications
            {
                Environment = Environment,
                Level = Level,
                Temperature = Temperature,
                TimeOfDay = TimeOfDay
            };

            filterGroup = ["creature", "other creature"];

            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, Filter)).Returns(filterGroup);
        }

        private void SetupEncounter(string encounter, int setupLevel)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleEncountersFrom(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    setupLevel,
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string[]>()))
                .Returns([encounter]);
        }

        private void SetupEncounterWithFilters(string encounter, int setupLevel)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleEncountersFrom(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    setupLevel,
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    specifications.CreatureTypeFilters.ToArray()))
                .Returns([encounter]);
        }

        [Test]
        public void ValidEncounterExistsIfAnyCreaturesInEncounter()
        {
            SetupEncounter("my encounter", Level);
            var isValid = encounterVerifier.ValidEncounterExists(specifications);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void ValidEncounterExistsIfAnyCreaturesInEncounterWithFilter()
        {
            specifications.CreatureTypeFilters = [Filter];
            SetupEncounterWithFilters("my encounter", Level);

            var isValid = encounterVerifier.ValidEncounterExists(specifications);
            Assert.That(isValid, Is.True);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(31)]
        public void ValidEncounterDoesNotExistAtLevelIfLevelInvalid(int level)
        {
            specifications.CreatureTypeFilters = [Filter];
            SetupEncounter("my encounter", level);

            var isValid = encounterVerifier.ValidEncounterExists(specifications);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void ValidEncounterDoesNotExistAtLevelIfNoCreaturesAvailable()
        {
            specifications.CreatureTypeFilters = [Filter];
            SetupEncounter("my encounter", Level - 1);

            var isValid = encounterVerifier.ValidEncounterExists(specifications);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void EncounterIsValidIfNoFiltersAreProvided()
        {
            var creatures = new[] { "creature" };

            var isValid = encounterVerifier.EncounterIsValid(creatures, specifications.CreatureTypeFilters);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterIsNotValidIfCreatureDoesNotMatchFilter()
        {
            var creatures = new[] { "bogus creature" };
            var isValid = encounterVerifier.EncounterIsValid(creatures, [Filter]);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void EncounterIsNotValidIfCreatureMatchesAnyFilter()
        {
            var creatures = new[] { "bogus creature" };
            var isValid = encounterVerifier.EncounterIsValid(creatures, [Filter, "other filter"]);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void EncounterIsValidIfCreatureMatchesAnyFilter()
        {
            filterGroup.Remove("creature");

            var additionalFilterGroup = new List<string>
            {
                "creature",
                "other creature"
            };

            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "other filter")).Returns(additionalFilterGroup);

            var creatures = new[] { "creature" };

            var isValid = encounterVerifier.EncounterIsValid(creatures, [Filter, "other filter"]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterIsValidIfAnyCreatureMatchesFilter()
        {
            filterGroup.Remove("creature");
            filterGroup.Add("other creature");

            var creatures = new[] { "creature", "other creature" };
            var isValid = encounterVerifier.EncounterIsValid(creatures, [Filter]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterIsValidIfCreatureWithDescriptionMatches()
        {
            var creatures = new[] { "creature (description)" };

            filterGroup.Remove("creature");
            filterGroup.Add("creature (description)");

            var isValid = encounterVerifier.EncounterIsValid(creatures, [Filter]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterFromCreaturesIsValidIfNoFiltersAreProvided()
        {
            var encounter = new Encounter
            {
                Creatures =
                [
                    new EncounterCreature { Creature = new Creature { Name = "creature" }, Quantity = 9266 },
                    new EncounterCreature { Creature = new Creature { Name = "other creature" }, Quantity = 90210 },
                ]
            };

            mockAmountSelector.Setup(s => s.Select(encounter)).Returns(15);

            var isValid = encounterVerifier.EncounterIsValid(encounter, specifications.CreatureTypeFilters);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterFromCreaturesIsNotValidIfCreatureDoesNotMatchFilter()
        {
            var encounter = new Encounter
            {
                Creatures =
                [
                    new EncounterCreature { Creature = new Creature { Name = "bogus creature" }, Quantity = 9266 },
                ]
            };

            mockAmountSelector.Setup(s => s.Select(encounter)).Returns(15);

            var isValid = encounterVerifier.EncounterIsValid(encounter, [Filter]);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void EncounterFromCreaturesIsValidIfCreatureMatchesAnyFilter()
        {
            var encounter = new Encounter
            {
                Creatures =
                [
                    new EncounterCreature { Creature = new Creature { Name = "creature" }, Quantity = 9266 },
                ]
            };

            mockAmountSelector.Setup(s => s.Select(encounter)).Returns(15);

            filterGroup.Remove("creature");

            var additionalFilterGroup = new List<string>
            {
                "creature",
                "other creature"
            };

            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "other filter")).Returns(additionalFilterGroup);

            var isValid = encounterVerifier.EncounterIsValid(encounter, [Filter, "other filter"]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void EncounterFromCreaturesIsValidIfAnyCreatureMatchesFilter()
        {
            filterGroup.Remove("creature");
            filterGroup.Add("other creature");

            var encounter = new Encounter
            {
                Creatures =
                [
                    new EncounterCreature { Creature = new Creature { Name = "creature" }, Quantity = 9266 },
                    new EncounterCreature { Creature = new Creature { Name = "other creature" }, Quantity = 90210 },
                ]
            };

            mockAmountSelector.Setup(s => s.Select(encounter)).Returns(15);

            var isValid = encounterVerifier.EncounterIsValid(encounter, [Filter, "other filter"]);
            Assert.That(isValid, Is.True);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(31)]
        public void EncounterFromCreaturesIsNotValidIfLevelInvalid(int level)
        {
            var encounter = new Encounter
            {
                Creatures =
                [
                    new EncounterCreature { Creature = new Creature { Name = "creature" }, Quantity = 9266 },
                    new EncounterCreature { Creature = new Creature { Name = "other creature" }, Quantity = 90210 },
                ]
            };

            mockAmountSelector.Setup(s => s.Select(encounter)).Returns(level);

            var isValid = encounterVerifier.EncounterIsValid(encounter, specifications.CreatureTypeFilters);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void EncounterFromCreaturesIsNotValidIfNoCreatures()
        {
            var encounter = new Encounter();
            var isValid = encounterVerifier.EncounterIsValid(encounter, specifications.CreatureTypeFilters);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void CreatureIsValidIfNoFiltersAreProvided()
        {
            var isValid = encounterVerifier.CreatureIsValid("bogus creature", specifications.CreatureTypeFilters);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void CreatureIsValidIfWithinFilterGroup()
        {
            var isValid = encounterVerifier.CreatureIsValid("creature", [Filter]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void CreatureIsNotValidIfOutsideFilterGroup()
        {
            var isValid = encounterVerifier.CreatureIsValid("wrong creature", [Filter]);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void CreatureIsValidIfAnyFilterMatches()
        {
            filterGroup.Remove("creature");

            var additionalFilterGroup = new List<string>
            {
                "creature",
                "other creature"
            };

            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "additional filter")).Returns(additionalFilterGroup);

            var isValid = encounterVerifier.CreatureIsValid("creature", [Filter, "additional filter"]);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void CreatureIsNotValidIfNoFilterMatches()
        {
            filterGroup.Remove("creature");

            var additionalFilterGroup = new List<string>
            {
                "creature",
                "other creature"
            };

            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "additional filter")).Returns(additionalFilterGroup);

            var isValid = encounterVerifier.CreatureIsValid("wrong creature", [Filter, "additional filter"]);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void CreatureIsValidIfCreatureWithDescriptionMatches()
        {
            filterGroup.Remove("creature");
            filterGroup.Add("creature (description)");

            var isValid = encounterVerifier.CreatureIsValid("creature (description)", [Filter]);
            Assert.That(isValid, Is.True);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ValidEncounterExists_ReturnsTrue_WhenThereIsAnEncounter(bool aquatic, bool underground)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleEncountersFrom("environment", "temperature", "time of day", 9266, aquatic, underground, "filter 1", "filter 2"))
                .Returns(["my encounter"]);

            var exists = encounterVerifier.ValidEncounterExists("environment", "temperature", "time of day", 9266, aquatic, underground, "filter 1", "filter 2");
            Assert.That(exists, Is.True);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ValidEncounterExists_ReturnsFalse_WhenThereIsNoEncounter(bool aquatic, bool underground)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleEncountersFrom("environment", "temperature", "time of day", 9266, aquatic, underground, "filter 1", "filter 2"))
                .Returns([]);

            var exists = encounterVerifier.ValidEncounterExists("environment", "temperature", "time of day", 9266, aquatic, underground, "filter 1", "filter 2");
            Assert.That(exists, Is.False);
        }

        [Test]
        public void GetValidTemperatues_ReturnsPossibleTemperatures()
        {
            var possibleTemps = new[] { "temp 1", "temp 2" };
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleTemperatures("environment"))
                .Returns(possibleTemps);

            var temperatures = encounterVerifier.GetValidTemperatues("environment");
            Assert.That(temperatures, Is.EqualTo(possibleTemps));
        }

        [Test]
        public void GetValidTimesOfDay_ReturnsPossibleTimesOfDay()
        {
            var possibleTimesOfDay = new[] { "time 1", "time 2" };
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleTimesOfDay("environment", "temperature"))
                .Returns(possibleTimesOfDay);

            var timesOfDay = encounterVerifier.GetValidTimesOfDay("environment", "temperature");
            Assert.That(timesOfDay, Is.EqualTo(possibleTimesOfDay));
        }

        [Test]
        public void GetValidLevels_ReturnsPossibleLevels()
        {
            var possibleLevels = new[] { 9266, 90210 };
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleLevels("environment", "temperature", "time of day"))
                .Returns(possibleLevels);

            var levels = encounterVerifier.GetValidLevels("environment", "temperature", "time of day");
            Assert.That(levels, Is.EqualTo(possibleLevels));
        }

        [TestCase]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(true, false)]
        public void GetValidAllowAquatic_ReturnsPossibleAllowAquatic(params bool[] possibleAquatic)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleAllowAquatic("environment", "temperature", "time of day", 9266))
                .Returns(possibleAquatic);

            var aquatic = encounterVerifier.GetValidAllowAquatic("environment", "temperature", "time of day", 9266);
            Assert.That(aquatic, Is.EqualTo(possibleAquatic));
        }

        [TestCase(true)]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(true, true, false)]
        [TestCase(false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        [TestCase(false, true, false)]
        public void GetValidAllowUnderground_ReturnsPossibleAllowUnderground(bool aquatic, params bool[] possibleUnderground)
        {
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleAllowUnderground("environment", "temperature", "time of day", 9266, aquatic))
                .Returns(possibleUnderground);

            var underground = encounterVerifier.GetValidAllowUnderground("environment", "temperature", "time of day", 9266, aquatic);
            Assert.That(underground, Is.EqualTo(possibleUnderground));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void GetValidFilters_ReturnsPossibleFilters(bool aquatic, bool underground)
        {
            var possibleFilters = new[] { "filter 1", "filter 2" };
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleFilters("environment", "temperature", "time of day", 9266, aquatic, underground))
                .Returns(possibleFilters);

            var filters = encounterVerifier.GetValidFilters("environment", "temperature", "time of day", 9266, aquatic, underground);
            Assert.That(filters, Is.EqualTo(possibleFilters));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void GetValidFilters_ReturnsPossibleFilters_None(bool aquatic, bool underground)
        {
            var possibleFilters = Enumerable.Empty<string>();
            mockEncounterCollectionSelector
                .Setup(s => s.SelectPossibleFilters("environment", "temperature", "time of day", 9266, aquatic, underground))
                .Returns(possibleFilters);

            var filters = encounterVerifier.GetValidFilters("environment", "temperature", "time of day", 9266, aquatic, underground);
            Assert.That(filters, Is.Empty);
        }
    }
}
