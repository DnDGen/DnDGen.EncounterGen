using DnDGen.EncounterGen.Generators;
using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Unit.Generators
{
    [TestFixture]
    public class CreatureGeneratorTests
    {
        private ICreatureGenerator creatureGenerator;
        private Mock<Dice> mockDice;
        private Mock<ICollectionSelector> mockCollectionSelector;
        private Mock<IEncounterFormatter> mockEncounterFormatter;
        private Dictionary<string, string> creaturesAndAmounts;
        private List<string> requiresSubcreature;
        private Mock<IChallengeRatingSelector> mockChallengeRatingSelector;
        private Dictionary<string, List<string>> challengeRatings;

        [SetUp]
        public void Setup()
        {
            mockDice = new Mock<Dice>();
            mockCollectionSelector = new Mock<ICollectionSelector>();
            mockEncounterFormatter = new Mock<IEncounterFormatter>();
            mockChallengeRatingSelector = new Mock<IChallengeRatingSelector>();

            creatureGenerator = new CreatureGenerator(
                mockDice.Object,
                mockCollectionSelector.Object,
                mockEncounterFormatter.Object,
                mockChallengeRatingSelector.Object);

            creaturesAndAmounts = [];
            requiresSubcreature = [];
            challengeRatings = [];

            creaturesAndAmounts["creature"] = "creature amount";
            AddChallengeRating("creature", "challenge rating");

            mockDice.Setup(d => d.Roll(It.IsAny<string>())).Returns(ParseRoll);
            mockDice.Setup(d => d.Roll("creature amount")).Returns(ParseRoll("42"));

            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.AverageChallengeRatings, It.IsAny<string>()))
                .Returns((string a, string t, string g) => challengeRatings.ContainsKey(g) ? challengeRatings[g] : Enumerable.Empty<string>());
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, GroupConstants.RequiresSubcreature)).Returns(requiresSubcreature);
            mockCollectionSelector.Setup(s => s.SelectRandomFrom(It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> c) => c.Last());

            mockChallengeRatingSelector.Setup(s => s.SelectAverageForCreature(It.IsAny<string>())).Returns((string s) => challengeRatings[s].Single());

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(It.IsAny<string>())).Returns((string s) => s);
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(It.IsAny<string>())).Returns(string.Empty);
        }

        private void AddChallengeRating(string creature, string cr)
        {
            challengeRatings[creature] = [cr];

            if (!challengeRatings.ContainsKey(cr))
                challengeRatings[cr] = [];

            challengeRatings[cr].Add(creature);
        }

        private PartialRoll ParseRoll(string roll)
        {
            var mockPartialRoll = new Mock<PartialRoll>();

            if (int.TryParse(roll, out var value))
            {
                mockPartialRoll.Setup(r => r.AsSum<int>()).Returns(value);
                return mockPartialRoll.Object;
            }

            throw new ArgumentException("This roll was not set up to be parsed: " + roll);
        }

        private static int GetIndex(int count) => (count % 6) switch
        {
            0 or 2 or 3 => 0,
            1 or 4 or 5 => 1,
            _ => 0,
        };

        [Test]
        public void GenerateForEncounter_GenerateCreatures()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.Description, Is.Empty);
            Assert.That(creature.Creature.SubCreature, Is.Null);
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_IfCreatureHasCountOf0OrLess_ThenRemoveCreature()
        {
            creaturesAndAmounts["other creature"] = "other amount";
            creaturesAndAmounts["another creature"] = "another amount";
            AddChallengeRating("other creature", "other challenge rating");
            AddChallengeRating("another creature", "another challenge rating");

            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockDice.Setup(d => d.Roll("creature amount")).Returns(ParseRoll("0"));
            mockDice.Setup(d => d.Roll("another amount")).Returns(ParseRoll("-42"));
            mockDice.Setup(d => d.Roll("other amount")).Returns(ParseRoll("69"));

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("other creature"));
            Assert.That(creature.Creature.Description, Is.Empty);
            Assert.That(creature.Creature.SubCreature, Is.Null);
            Assert.That(creature.Quantity, Is.EqualTo(69));
            Assert.That(creature.ChallengeRating, Is.EqualTo("other challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_GenerateCreatureWithDescription()
        {
            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creaturesAndAmounts.First().Key)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creaturesAndAmounts.First().Key)).Returns("creature description");

            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature, Is.Null);
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_GenerateCreatureWithSetSubcreature()
        {
            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creaturesAndAmounts.First().Key)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom(creaturesAndAmounts.First().Key)).Returns("sub-creature");

            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.Description, Is.Empty);
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creature.Creature.SubCreature.Description, Is.Empty);
            Assert.That(creature.Creature.SubCreature.SubCreature, Is.Null);
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureShouldUseSetAmountAndSetChallengeRating()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("challenge rating");

            requiresSubcreature.Add("creature");

            var subcreatures = new[] { "wrong creature", "other creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(subcreatures);

            mockCollectionSelector.SetupSequence(s => s.SelectRandomFrom(subcreatures)).Returns(subcreatures.First()).Returns(subcreatures.Last());

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");
            AddChallengeRating(creaturesAndAmounts.Keys.First(), "specific challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("specific challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureShouldPickNewSubtypeForEachInQuantity()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("challenge rating");

            requiresSubcreature.Add("creature");

            var subcreatures = new[] { "other creature", "random creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(subcreatures);

            var count = 0;
            mockCollectionSelector.Setup(s => s.SelectRandomFrom(subcreatures)).Returns((IEnumerable<string> c) => c.ElementAt(GetIndex(count++)));

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");
            AddChallengeRating("random creature", "challenge rating");
            AddChallengeRating(creaturesAndAmounts.Keys.First(), "specific challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);
            Assert.That(creatures.Count, Is.EqualTo(2));

            var first = creatures.First();
            Assert.That(first.Creature.Name, Is.EqualTo("creature"));
            Assert.That(first.Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(first.Quantity, Is.EqualTo(21));
            Assert.That(first.ChallengeRating, Is.EqualTo("specific challenge rating"));

            var last = creatures.Last();
            Assert.That(last.Creature.Name, Is.EqualTo("creature"));
            Assert.That(last.Creature.SubCreature.Name, Is.EqualTo("random creature"));
            Assert.That(last.Quantity, Is.EqualTo(21));
            Assert.That(last.ChallengeRating, Is.EqualTo("specific challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureWithoutSetChallengeRatingThrowsException()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            requiresSubcreature.Add("creature");

            var subcreatures = new[] { "wrong creature", "other creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(subcreatures);

            mockCollectionSelector.SetupSequence(s => s.SelectRandomFrom(subcreatures)).Returns(subcreatures.First()).Returns(subcreatures.Last());

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");

            Assert.That(
                () => creatureGenerator.GenerateFor("my encounter"),
                Throws.InvalidOperationException.With.Message.EqualTo("Cannot generate random sub-creature of creature without a set challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureWithFurtherSubcreatureShouldUseSetAmountAndSetChallengeRating()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom("other creature")).Returns("sub-creature");

            requiresSubcreature.Add("creature");

            var subcreatures = new[] { "wrong creature", "other creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(subcreatures);

            mockCollectionSelector.SetupSequence(s => s.SelectRandomFrom(subcreatures)).Returns(subcreatures.First()).Returns(subcreatures.Last());

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");
            AddChallengeRating(creaturesAndAmounts.Keys.First(), "specific challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("specific challenge rating"));
        }

        //INFO: Example of this might be a dominated mephit
        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureHasFurtherRandomSubcreature()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom("other creature")).Returns("sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom("other creature")).Returns("other challenge rating");

            requiresSubcreature.Add("creature");
            requiresSubcreature.Add("other creature");

            var subSubcreatures = new[] { "wrong sub-creature", "sub-creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "other creature")).Returns(subSubcreatures);
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(["wrong creature", "other creature"]);

            mockCollectionSelector.SetupSequence(s => s.SelectRandomFrom(subSubcreatures)).Returns(subSubcreatures.First()).Returns(subSubcreatures.Last());

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");
            AddChallengeRating("wrong sub-creature", "wrong challenge rating");
            AddChallengeRating("sub-creature", "other challenge rating");
            AddChallengeRating(creaturesAndAmounts.Keys.First(), "specific challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("specific challenge rating"));
        }

        //INFO: Example of this might be a dominated mephit
        [Test]
        public void GenerateForEncounter_CreatureWithRandomSubcreatureAndFurtherRandomSubcreatureSelectsRandomEachTime()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom("other creature")).Returns("other challenge rating");

            requiresSubcreature.Add("creature");
            requiresSubcreature.Add("other creature");

            var subtypes = new[] { "wrong creature", "random creature", "other creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(subtypes);

            var subSubtypes = new[] { "wrong sub-creature", "other sub-creature", "sub-creature" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "other creature")).Returns(subSubtypes);

            var count = 0;
            mockCollectionSelector.Setup(s => s.SelectRandomFrom(It.IsAny<IEnumerable<string>>())).Returns((IEnumerable<string> c) => c.ElementAt(GetIndex(count++)));

            AddChallengeRating("other creature", "challenge rating");
            AddChallengeRating("random creature", "challenge rating");
            AddChallengeRating("wrong creature", "wrong challenge rating");
            AddChallengeRating("wrong sub-creature", "wrong challenge rating");
            AddChallengeRating("sub-creature", "other challenge rating");
            AddChallengeRating("other sub-creature", "other challenge rating");
            AddChallengeRating(creaturesAndAmounts.Keys.First(), "specific challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter").ToArray();
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);
            Assert.That(creatures.Length, Is.EqualTo(3));

            Assert.That(creatures[0].Creature.Name, Is.EqualTo("creature"));
            Assert.That(creatures[0].Creature.SubCreature.Name, Is.EqualTo("random creature"));
            Assert.That(creatures[0].Quantity, Is.EqualTo(21));
            Assert.That(creatures[0].ChallengeRating, Is.EqualTo("specific challenge rating"));

            Assert.That(creatures[1].Creature.Name, Is.EqualTo("creature"));
            Assert.That(creatures[1].Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(creatures[1].Creature.SubCreature.SubCreature.Name, Is.EqualTo("other sub-creature"));
            Assert.That(creatures[1].Quantity, Is.EqualTo(11));
            Assert.That(creatures[1].ChallengeRating, Is.EqualTo("specific challenge rating"));

            Assert.That(creatures[2].Creature.Name, Is.EqualTo("creature"));
            Assert.That(creatures[2].Creature.SubCreature.Name, Is.EqualTo("other creature"));
            Assert.That(creatures[2].Creature.SubCreature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creatures[2].Quantity, Is.EqualTo(10));
            Assert.That(creatures[2].ChallengeRating, Is.EqualTo("specific challenge rating"));
        }

        [Test]
        public void GenerateForEncounter_DoNotGetRandomSubcreatureIfSet()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creaturesAndAmounts.First().Key)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creaturesAndAmounts.First().Key)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom(creaturesAndAmounts.First().Key)).Returns("sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");

            requiresSubcreature.Add("creature");
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature")).Returns(["wrong sub-creature"]);

            AddChallengeRating("wrong sub-creature", "creature challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter").ToArray();
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature, Is.Null);
        }

        //INFO: There is no current example of this in the system, but it is a good thing to test
        [Test]
        public void GenerateForEncounter_DeepRecursiveCreaturesAreOkay()
        {
            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creaturesAndAmounts.First().Key)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creaturesAndAmounts.First().Key)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creaturesAndAmounts.First().Key)).Returns("creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom("sub-creature")).Returns("sub-creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("further sub-creature")).Returns("further sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("further sub-creature")).Returns("further sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom("further sub-creature")).Returns("last sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("last sub-creature")).Returns("last sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("last sub-creature")).Returns("last sub-creature description");

            requiresSubcreature.Add(creaturesAndAmounts.First().Key);
            requiresSubcreature.Add("sub-creature");
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "creature name")).Returns(["sub-creature"]);
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, "sub-creature name")).Returns(["further sub-creature"]);

            AddChallengeRating("sub-creature", "creature challenge rating");
            AddChallengeRating("further sub-creature", "sub-creature challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("further sub-creature"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Description, Is.EqualTo("further sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature.Name, Is.EqualTo("last sub-creature"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature.Description, Is.EqualTo("last sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void GenerateForEncounter_GetMultipleCreatures()
        {
            creaturesAndAmounts["other creature"] = "other creature amount";
            mockDice.Setup(d => d.Roll("other creature amount")).Returns(ParseRoll("600"));

            var creatureData = creaturesAndAmounts.Select(kvp => $"{kvp.Key}/{kvp.Value}");
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, "my encounter"))
                .Returns(creatureData);
            mockEncounterFormatter
                .Setup(f => f.SelectCreaturesAndAmountsFrom(creatureData))
                .Returns(creaturesAndAmounts);

            AddChallengeRating("other creature", "other challenge rating");

            var creatures = creatureGenerator.GenerateFor("my encounter");
            Assert.That(creatures, Is.Not.Null);
            Assert.That(creatures, Is.Not.Empty);

            var creature = creatures.Single(c => c.Creature.Name == "creature");
            var otherCreature = creatures.Single(c => c.Creature.Name == "other creature");
            Assert.That(creature.Quantity, Is.EqualTo(42));
            Assert.That(creature.ChallengeRating, Is.EqualTo("challenge rating"));
            Assert.That(otherCreature.Quantity, Is.EqualTo(600));
            Assert.That(otherCreature.ChallengeRating, Is.EqualTo("other challenge rating"));
        }

        [Test]
        public void CleanCreatures_CleanUpCreatureName()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_DoNotOverwriteExistingDescriptionWhenCleaningCreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";
            creatures[0].Creature.Description = "description";

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("wrong description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("description"));
            Assert.That(creature.Creature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_CleanUpRandomSubcreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";
            creatures[0].Creature.SubCreature = new Creature
            {
                Name = "sub-creature"
            };

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creatures[0].Creature.Name)).Returns("creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_CleanUpSetSubcreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom(creatures[0].Creature.Name)).Returns("sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_DoNotOverwriteExistingSubcreatureWhenCleaning()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";
            creatures[0].Creature.SubCreature = new Creature
            {
                Name = "existing sub-creature"
            };

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom(creatures[0].Creature.Name)).Returns("sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("existing sub-creature")).Returns("existing sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("existing sub-creature")).Returns("existing sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("existing sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("existing sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_CleanUpRandomSubcreatureWithFurtherRandomSubcreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";
            creatures[0].Creature.SubCreature = new Creature
            {
                Name = "sub-creature",
                SubCreature = new Creature()
            };
            creatures[0].Creature.SubCreature.SubCreature.Name = "further sub-creature";

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creatures[0].Creature.Name)).Returns("creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom("sub-creature")).Returns("sub-creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("further sub-creature")).Returns("further sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("further sub-creature")).Returns("further sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("further sub-creature name"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Description, Is.EqualTo("further sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_CleanUpRandomSubcreatureWithFurtherSetSubcreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";
            creatures[0].Creature.SubCreature = new Creature
            {
                Name = "sub-creature"
            };

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectChallengeRatingFrom(creatures[0].Creature.Name)).Returns("creature challenge rating");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom("sub-creature")).Returns("further sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("further sub-creature")).Returns("further sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("further sub-creature")).Returns("further sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("further sub-creature name"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Description, Is.EqualTo("further sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_CleanUpSetSubcreatureWithFurtherSetSubcreature()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";

            mockEncounterFormatter.Setup(s => s.SelectNameFrom(creatures[0].Creature.Name)).Returns("creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom(creatures[0].Creature.Name)).Returns("sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("sub-creature")).Returns("sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("sub-creature")).Returns("sub-creature description");
            mockEncounterFormatter.Setup(s => s.SelectSubCreatureFrom("sub-creature")).Returns("further sub-creature");
            mockEncounterFormatter.Setup(s => s.SelectNameFrom("further sub-creature")).Returns("further sub-creature name");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom("further sub-creature")).Returns("further sub-creature description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("creature name"));
            Assert.That(creature.Creature.Description, Is.EqualTo("creature description"));
            Assert.That(creature.Creature.SubCreature.Name, Is.EqualTo("sub-creature name"));
            Assert.That(creature.Creature.SubCreature.Description, Is.EqualTo("sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Name, Is.EqualTo("further sub-creature name"));
            Assert.That(creature.Creature.SubCreature.SubCreature.Description, Is.EqualTo("further sub-creature description"));
            Assert.That(creature.Creature.SubCreature.SubCreature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_IfCreatureHasSetMetarace_UseThatAsNameWhenCleaningUp()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";

            mockEncounterFormatter.Setup(s => s.SelectMetaraceFrom(creatures[0].Creature.Name)).Returns("metarace");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("metarace"));
            Assert.That(creature.Creature.Description, Is.EqualTo("description"));
            Assert.That(creature.Creature.SubCreature, Is.Null);
        }

        [Test]
        public void CleanCreatures_IfCreatureHasSetBaseRace_UseThatAsNameWhenCleaningUp()
        {
            var creatures = new[]
            {
                new EncounterCreature(),
            };

            creatures[0].Creature.Name = "creature";

            mockEncounterFormatter.Setup(s => s.SelectBaseRaceFrom(creatures[0].Creature.Name)).Returns("base race");
            mockEncounterFormatter.Setup(s => s.SelectDescriptionFrom(creatures[0].Creature.Name)).Returns("description");

            var cleanedCreatures = creatureGenerator.CleanCreatures(creatures);
            Assert.That(cleanedCreatures, Is.Not.Null);
            Assert.That(cleanedCreatures, Is.Not.Empty);
            Assert.That(cleanedCreatures, Is.EqualTo(creatures));

            var creature = cleanedCreatures.Single();
            Assert.That(creature.Creature.Name, Is.EqualTo("base race"));
            Assert.That(creature.Creature.Description, Is.EqualTo("description"));
            Assert.That(creature.Creature.SubCreature, Is.Null);
        }
    }
}
