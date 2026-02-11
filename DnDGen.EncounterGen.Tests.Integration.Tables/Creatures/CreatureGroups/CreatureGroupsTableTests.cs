using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Tables;
using NUnit.Framework;
using System.Collections.Generic;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public abstract class CreatureGroupsTableTests : CollectionTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.CreatureGroups;
            }
        }

        protected void AssertCreatureGroupEntriesAreComplete()
        {
            var entries = new[]
            {
                string.Empty,
                CreatureDataConstants.Character,
                CreatureDataConstants.DominatedCreature,
                CreatureDataConstants.Scorpionfolk,
                CreatureDataConstants.Shadow,
                CreatureDataConstants.Shark,
                CreatureDataConstants.Skeleton,
                CreatureDataConstants.Slaad,
                CreatureDataConstants.Snake_Constrictor,
                CreatureDataConstants.Snake_Viper,
                CreatureDataConstants.Spider_Monstrous,
                CreatureDataConstants.Sprite,
                CreatureDataConstants.Squid,
                CreatureDataConstants.Svirfneblin,
                CreatureDataConstants.Tiefling,
                CreatureDataConstants.Tiger,
                CreatureDataConstants.Tojanida,
                CreatureDataConstants.Troll,
                CreatureDataConstants.Troll_Scrag,
                CreatureDataConstants.Troglodyte,
                CreatureDataConstants.Unicorn,
                CreatureDataConstants.UmberHulk,
                CreatureDataConstants.Vampire,
                CreatureDataConstants.Warrior_Bandit,
                CreatureDataConstants.Warrior_Lieutenant,
                CreatureDataConstants.Warrior_Guard,
                CreatureDataConstants.Warrior_Leader,
                CreatureDataConstants.Weasel,
                CreatureDataConstants.Werewolf,
                CreatureDataConstants.Whale,
                CreatureDataConstants.Wizard_FamousResearcher,
                CreatureDataConstants.Wolf,
                CreatureDataConstants.Wolverine,
                CreatureDataConstants.Wraith,
                CreatureDataConstants.Xorn,
                CreatureDataConstants.YuanTi,
                CreatureDataConstants.Zombie,
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
