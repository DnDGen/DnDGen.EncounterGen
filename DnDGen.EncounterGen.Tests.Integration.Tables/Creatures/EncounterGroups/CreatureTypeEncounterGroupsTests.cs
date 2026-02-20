using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Tables;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Creatures.EncounterGroups
{
    [TestFixture]
    public class CreatureTypeEncounterGroupsTests : EncounterGroupsTableTests
    {
        [Test]
        public override void EntriesAreComplete()
        {
            AssertEncounterGroupNamesAreComplete();
        }

        [TestCase(CreatureDataConstants.Types.Aberration)]
        [TestCase(CreatureDataConstants.Types.Animal)]
        [TestCase(CreatureDataConstants.Types.Construct)]
        [TestCase(CreatureDataConstants.Types.Dragon)]
        [TestCase(CreatureDataConstants.Types.Elemental)]
        [TestCase(CreatureDataConstants.Types.Fey)]
        [TestCase(CreatureDataConstants.Types.Giant)]
        [TestCase(CreatureDataConstants.Types.Humanoid)]
        [TestCase(CreatureDataConstants.Types.MagicalBeast)]
        [TestCase(CreatureDataConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureDataConstants.Types.Ooze)]
        [TestCase(CreatureDataConstants.Types.Outsider)]
        [TestCase(CreatureDataConstants.Types.Plant)]
        [TestCase(CreatureDataConstants.Types.Undead)]
        [TestCase(CreatureDataConstants.Types.Vermin)]
        public void CreatureTypeEncounterGroup(string creatureType)
        {
            var encounters = EncounterConstants.GetAll();
            var encountersOfType = new List<string>();

            foreach (var encounter in encounters)
            {
                var creaturesOfType = GetCreaturesOfTypeInEncounter(creatureType, encounter);
                if (creaturesOfType.Any())
                    encountersOfType.Add(encounter);
            }

            AssertDistinctCollection(creatureType, [.. encountersOfType]);
        }

        private IEnumerable<string> GetCreaturesOfTypeInEncounter(string creatureType, string encounter)
        {
            var creaturesOfType = collectionSelector.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, creatureType);
            var encounterCreatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, encounter);
            var creatures = encounterFormatter.SelectCreaturesAndAmountsFrom(encounterCreatures).Keys;

            return creatures.Intersect(creaturesOfType);
        }
    }
}
