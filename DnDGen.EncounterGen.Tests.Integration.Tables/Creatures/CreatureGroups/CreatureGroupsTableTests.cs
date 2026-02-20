using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Tables;
using NUnit.Framework;
using System.Collections.Generic;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public abstract class CreatureGroupsTableTests : CollectionTests
    {
        protected override string tableName => TableNameConstants.CreatureGroups;

        protected void AssertCreatureGroupEntriesAreComplete()
        {
            var entries = new[]
            {
                string.Empty,
                CreatureDataConstants.Character,
                CreatureDataConstants.DominatedCreature,
                CreatureDataConstants.Types.Aberration,
                CreatureDataConstants.Types.Animal,
                CreatureDataConstants.Types.Construct,
                CreatureDataConstants.Types.Dragon,
                CreatureDataConstants.Types.Elemental,
                CreatureDataConstants.Types.Fey,
                CreatureDataConstants.Types.Giant,
                CreatureDataConstants.Types.Humanoid,
                CreatureDataConstants.Types.MagicalBeast,
                CreatureDataConstants.Types.MonstrousHumanoid,
                CreatureDataConstants.Types.Ooze,
                CreatureDataConstants.Types.Outsider,
                CreatureDataConstants.Types.Plant,
                CreatureDataConstants.Types.Undead,
                CreatureDataConstants.Types.Vermin,
                GroupConstants.RequiresSubcreature,
                GroupConstants.UseSubcreatureForTreasure,
            };

            AssertNamesAreComplete(entries);
        }
        protected IEnumerable<string> GetAllCreatures() => ExplodeCollections(table.Keys);
    }
}
