using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Tables;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Creatures.EncounterGroups
{
    [TestFixture]
    public class TimeOfDayEncounterGroupsTests : EncounterGroupsTableTests
    {
        private readonly string[] creaturesSensitiveToSunlight =
        [
            CreatureDataConstants.Bodak,
            CreatureDataConstants.Derro,
            CreatureDataConstants.Derro_Noncombatant,
            CreatureDataConstants.Derro_Sorcerer_3rd,
            CreatureDataConstants.Derro_Sorcerer_5thTo8th,
            CreatureDataConstants.Drow_Captain,
            CreatureDataConstants.Drow_Leader,
            CreatureDataConstants.Drow_Lieutenant,
            CreatureDataConstants.Drow_Noncombatant,
            CreatureDataConstants.Drow_Sergeant,
            CreatureDataConstants.Drow_Warrior,
            CreatureDataConstants.Duergar_Captain,
            CreatureDataConstants.Duergar_Leader,
            CreatureDataConstants.Duergar_Lieutenant,
            CreatureDataConstants.Duergar_Noncombatant,
            CreatureDataConstants.Duergar_Sergeant,
            CreatureDataConstants.Duergar_Warrior,
            CreatureDataConstants.NightHag,
            CreatureDataConstants.Nightmare,
            CreatureDataConstants.Nightmare_Cauchemar,
            CreatureDataConstants.Nightcrawler,
            CreatureDataConstants.Nightwalker,
            CreatureDataConstants.Nightwing,
            CreatureDataConstants.Orc,
            CreatureDataConstants.Orc_Captain,
            CreatureDataConstants.Orc_Leader,
            CreatureDataConstants.Orc_Lieutenant,
            CreatureDataConstants.Orc_Noncombatant,
            CreatureDataConstants.Orc_Sergeant,
            CreatureDataConstants.Orc_Warrior,
            CreatureDataConstants.Shadow,
            CreatureDataConstants.Shadow_Greater,
            CreatureDataConstants.ShadowMastiff,
            CreatureDataConstants.Spectre,
            //CreatureDataConstants.Vampire,
            CreatureDataConstants.Vampire_Level1,
            CreatureDataConstants.Vampire_Level2,
            CreatureDataConstants.Vampire_Level3,
            CreatureDataConstants.Vampire_Level4,
            CreatureDataConstants.Vampire_Level5,
            CreatureDataConstants.Vampire_Level6,
            CreatureDataConstants.Vampire_Level7,
            CreatureDataConstants.Vampire_Level8,
            CreatureDataConstants.Vampire_Level9,
            CreatureDataConstants.Vampire_Level10,
            CreatureDataConstants.Vampire_Level11,
            CreatureDataConstants.Vampire_Level12,
            CreatureDataConstants.Vampire_Level13,
            CreatureDataConstants.Vampire_Level14,
            CreatureDataConstants.Vampire_Level15,
            CreatureDataConstants.Vampire_Level16,
            CreatureDataConstants.Vampire_Level17,
            CreatureDataConstants.Vampire_Level18,
            CreatureDataConstants.Vampire_Level19,
            CreatureDataConstants.Vampire_Level20,
            CreatureDataConstants.VampireSpawn,
            CreatureDataConstants.Wraith,
            CreatureDataConstants.Wraith_Dread,
        ];

        [Test]
        public override void EntriesAreComplete()
        {
            AssertEncounterGroupNamesAreComplete();
        }

        [Test]
        public void DayEncounters()
        {
            var encountersSensitiveToSunlight = new[]
            {
                EncounterConstants.Bodak_Gang,
                EncounterConstants.Bodak_Solitary,
                EncounterConstants.Derro_Band,
                EncounterConstants.Derro_Squad,
                EncounterConstants.Derro_Team,
                EncounterConstants.Drow_Band,
                EncounterConstants.Drow_Patrol,
                EncounterConstants.Drow_Squad,
                EncounterConstants.Duergar_Clan,
                EncounterConstants.Duergar_Squad,
                EncounterConstants.Duergar_Team,
                EncounterConstants.NightHag_Covey,
                EncounterConstants.NightHag_Mounted,
                EncounterConstants.NightHag_Solitary,
                EncounterConstants.Nightmare_Cauchemar_Solitary,
                EncounterConstants.Nightmare_Solitary,
                EncounterConstants.Nightcrawler_Pair,
                EncounterConstants.Nightcrawler_Solitary,
                EncounterConstants.Nightwalker_Gang,
                EncounterConstants.Nightwalker_Pair,
                EncounterConstants.Nightwalker_Solitary,
                EncounterConstants.Nightwing_Flock,
                EncounterConstants.Nightwing_Pair,
                EncounterConstants.Nightwing_Solitary,
                EncounterConstants.Orc_Band,
                EncounterConstants.Orc_Gang,
                EncounterConstants.Orc_Squad,
                EncounterConstants.Shadow_Gang,
                EncounterConstants.Shadow_Solitary,
                EncounterConstants.Shadow_Swarm,
                EncounterConstants.Shadow_Greater_Solitary,
                EncounterConstants.ShadowMastiff_Pack,
                EncounterConstants.ShadowMastiff_Pair,
                EncounterConstants.ShadowMastiff_Solitary,
                EncounterConstants.Spectre_Gang,
                EncounterConstants.Spectre_Solitary,
                EncounterConstants.Spectre_Swarm,
                EncounterConstants.VampireSpawn_Pack,
                EncounterConstants.VampireSpawn_Solitary,
                EncounterConstants.Vampire_Level1_Gang,
                EncounterConstants.Vampire_Level1_Pair,
                EncounterConstants.Vampire_Level1_Solitary,
                EncounterConstants.Vampire_Level1_Troupe,
                EncounterConstants.Vampire_Level2_Gang,
                EncounterConstants.Vampire_Level2_Pair,
                EncounterConstants.Vampire_Level2_Solitary,
                EncounterConstants.Vampire_Level2_Troupe,
                EncounterConstants.Vampire_Level3_Gang,
                EncounterConstants.Vampire_Level3_Pair,
                EncounterConstants.Vampire_Level3_Solitary,
                EncounterConstants.Vampire_Level3_Troupe,
                EncounterConstants.Vampire_Level4_Gang,
                EncounterConstants.Vampire_Level4_Pair,
                EncounterConstants.Vampire_Level4_Solitary,
                EncounterConstants.Vampire_Level4_Troupe,
                EncounterConstants.Vampire_Level5_Gang,
                EncounterConstants.Vampire_Level5_Pair,
                EncounterConstants.Vampire_Level5_Solitary,
                EncounterConstants.Vampire_Level5_Troupe,
                EncounterConstants.Vampire_Level6_Gang,
                EncounterConstants.Vampire_Level6_Pair,
                EncounterConstants.Vampire_Level6_Solitary,
                EncounterConstants.Vampire_Level6_Troupe,
                EncounterConstants.Vampire_Level7_Gang,
                EncounterConstants.Vampire_Level7_Pair,
                EncounterConstants.Vampire_Level7_Solitary,
                EncounterConstants.Vampire_Level7_Troupe,
                EncounterConstants.Vampire_Level8_Gang,
                EncounterConstants.Vampire_Level8_Pair,
                EncounterConstants.Vampire_Level8_Solitary,
                EncounterConstants.Vampire_Level8_Troupe,
                EncounterConstants.Vampire_Level9_Gang,
                EncounterConstants.Vampire_Level9_Pair,
                EncounterConstants.Vampire_Level9_Solitary,
                EncounterConstants.Vampire_Level9_Troupe,
                EncounterConstants.Vampire_Level10_Gang,
                EncounterConstants.Vampire_Level10_Pair,
                EncounterConstants.Vampire_Level10_Solitary,
                EncounterConstants.Vampire_Level10_Troupe,
                EncounterConstants.Vampire_Level11_Gang,
                EncounterConstants.Vampire_Level11_Pair,
                EncounterConstants.Vampire_Level11_Solitary,
                EncounterConstants.Vampire_Level11_Troupe,
                EncounterConstants.Vampire_Level12_Gang,
                EncounterConstants.Vampire_Level12_Pair,
                EncounterConstants.Vampire_Level12_Solitary,
                EncounterConstants.Vampire_Level12_Troupe,
                EncounterConstants.Vampire_Level13_Gang,
                EncounterConstants.Vampire_Level13_Pair,
                EncounterConstants.Vampire_Level13_Solitary,
                EncounterConstants.Vampire_Level13_Troupe,
                EncounterConstants.Vampire_Level14_Gang,
                EncounterConstants.Vampire_Level14_Pair,
                EncounterConstants.Vampire_Level14_Solitary,
                EncounterConstants.Vampire_Level14_Troupe,
                EncounterConstants.Vampire_Level15_Gang,
                EncounterConstants.Vampire_Level15_Pair,
                EncounterConstants.Vampire_Level15_Solitary,
                EncounterConstants.Vampire_Level15_Troupe,
                EncounterConstants.Vampire_Level16_Gang,
                EncounterConstants.Vampire_Level16_Pair,
                EncounterConstants.Vampire_Level16_Solitary,
                EncounterConstants.Vampire_Level16_Troupe,
                EncounterConstants.Vampire_Level17_Gang,
                EncounterConstants.Vampire_Level17_Pair,
                EncounterConstants.Vampire_Level17_Solitary,
                EncounterConstants.Vampire_Level17_Troupe,
                EncounterConstants.Vampire_Level18_Gang,
                EncounterConstants.Vampire_Level18_Pair,
                EncounterConstants.Vampire_Level18_Solitary,
                EncounterConstants.Vampire_Level18_Troupe,
                EncounterConstants.Vampire_Level19_Gang,
                EncounterConstants.Vampire_Level19_Pair,
                EncounterConstants.Vampire_Level19_Solitary,
                EncounterConstants.Vampire_Level19_Troupe,
                EncounterConstants.Vampire_Level20_Gang,
                EncounterConstants.Vampire_Level20_Pair,
                EncounterConstants.Vampire_Level20_Solitary,
                EncounterConstants.Vampire_Level20_Troupe,
                EncounterConstants.Wraith_Gang,
                EncounterConstants.Wraith_Pack,
                EncounterConstants.Wraith_Solitary,
                EncounterConstants.Wraith_Dread_Solitary,
                EncounterConstants.Ettin_Colony_WithOrcs,
                EncounterConstants.Giant_Hill_Tribe,
                EncounterConstants.Lich_Level11_Troupe,
                EncounterConstants.Lich_Level12_Troupe,
                EncounterConstants.Lich_Level13_Troupe,
                EncounterConstants.Lich_Level14_Troupe,
                EncounterConstants.Lich_Level15_Troupe,
                EncounterConstants.Lich_Level16_Troupe,
                EncounterConstants.Lich_Level17_Troupe,
                EncounterConstants.Lich_Level18_Troupe,
                EncounterConstants.Lich_Level19_Troupe,
                EncounterConstants.Lich_Level20_Troupe,
            };

            var allEncounters = EncounterConstants.GetAll();
            var dayEncounters = allEncounters.Except(encountersSensitiveToSunlight).ToArray();

            AssertDistinctCollection(EnvironmentConstants.TimesOfDay.Day, dayEncounters);
        }

        private IEnumerable<string> GetCreaturesInEncounter(string encounter)
        {
            var encounterCreatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.EncounterCreatures, encounter);
            var creatures = encounterFormatter.SelectCreaturesAndAmountsFrom(encounterCreatures).Keys;

            return creatures;
        }

        [Test]
        public void BUG_AllCreaturesSensitiveToSunlightDoNotAppearInDay()
        {
            foreach (var encounter in table[EnvironmentConstants.TimesOfDay.Day])
            {
                var encounterCreatures = GetCreaturesInEncounter(encounter);
                Assert.That(encounterCreatures.Intersect(creaturesSensitiveToSunlight), Is.Empty, encounter);
            }
        }

        [Test]
        public void CreaturesNotSensitiveToSunlightAppearInDay()
        {
            var allCreatures = GetAllCreaturesFromEncounters(false);
            var notSensitiveToSunlight = allCreatures.Except(creaturesSensitiveToSunlight);

            var dayCreatures = table[EnvironmentConstants.TimesOfDay.Day].SelectMany(GetCreaturesInEncounter).Distinct();
            AssertDistinctCollection(notSensitiveToSunlight, dayCreatures);
        }

        [Test]
        public void NightEncounters()
        {
            var encounters = EncounterConstants.GetAll().ToArray();
            AssertDistinctCollection(EnvironmentConstants.TimesOfDay.Night, encounters);
        }
    }
}
