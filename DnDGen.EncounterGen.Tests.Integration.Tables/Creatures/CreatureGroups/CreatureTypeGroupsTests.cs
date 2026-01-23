using DnDGen.EncounterGen.Models;
using NUnit.Framework;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class CreatureTypeGroupsTests : CreatureGroupsTableTests
    {
        [Test]
        public override void EntriesAreComplete()
        {
            AssertCreatureGroupEntriesAreComplete();
        }

        [Test]
        public void AberrationGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Aboleth,
                CreatureDataConstants.Aboleth_Mage,
                CreatureDataConstants.Athach,
                CreatureDataConstants.Beholder,
                CreatureDataConstants.CarrionCrawler,
                CreatureDataConstants.Choker,
                CreatureDataConstants.Chuul,
                CreatureDataConstants.Cloaker,
                CreatureDataConstants.Delver,
                CreatureDataConstants.Destrachan,
                CreatureDataConstants.Drider,
                CreatureDataConstants.EtherealFilcher,
                CreatureDataConstants.Ettercap,
                CreatureDataConstants.GibberingMouther,
                CreatureDataConstants.Grick,
                CreatureDataConstants.Mimic,
                CreatureDataConstants.MindFlayer,
                CreatureDataConstants.Naga,
                CreatureDataConstants.Otyugh,
                CreatureDataConstants.Phasm,
                CreatureDataConstants.RustMonster,
                CreatureDataConstants.Skum,
                CreatureDataConstants.UmberHulk,
                CreatureDataConstants.WillOWisp,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Aberration, creatures);
        }

        [Test]
        public void AnimalGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Ape,
                CreatureDataConstants.Ape_Dire,
                CreatureDataConstants.Baboon,
                CreatureDataConstants.Badger,
                CreatureDataConstants.Bat,
                CreatureDataConstants.Bear,
                CreatureDataConstants.Bison,
                CreatureDataConstants.Boar,
                CreatureDataConstants.Camel,
                CreatureDataConstants.Cat,
                CreatureDataConstants.Cheetah,
                CreatureDataConstants.Crocodile,
                CreatureDataConstants.Dinosaur,
                CreatureDataConstants.Dog,
                CreatureDataConstants.Donkey,
                CreatureDataConstants.Eagle,
                CreatureDataConstants.Elephant,
                CreatureDataConstants.Hawk,
                CreatureDataConstants.Horse,
                CreatureDataConstants.Hyena,
                CreatureDataConstants.Leopard,
                CreatureDataConstants.Lion,
                CreatureDataConstants.Livestock,
                CreatureDataConstants.Lizard,
                CreatureDataConstants.Lizard_Monitor,
                CreatureDataConstants.MantaRay,
                CreatureDataConstants.Monkey,
                CreatureDataConstants.Mule,
                CreatureDataConstants.Octopus,
                CreatureDataConstants.Owl,
                CreatureDataConstants.Pony,
                CreatureDataConstants.Porpoise,
                CreatureDataConstants.Rat,
                CreatureDataConstants.Raven,
                CreatureDataConstants.Rhinoceras,
                CreatureDataConstants.Roc,
                CreatureDataConstants.Snake_Constrictor,
                CreatureDataConstants.Snake_Viper,
                CreatureDataConstants.Shark,
                CreatureDataConstants.Squid,
                CreatureDataConstants.Tiger,
                CreatureDataConstants.Toad,
                CreatureDataConstants.Weasel,
                CreatureDataConstants.Whale,
                CreatureDataConstants.Wolf,
                CreatureDataConstants.Wolverine,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Animal, creatures);
        }

        [Test]
        public void ConstructGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Inevitable,
                //CreatureDataConstants.AnimatedObject,
                CreatureDataConstants.AnimatedObject_Colossal,
                CreatureDataConstants.AnimatedObject_Gargantuan,
                CreatureDataConstants.AnimatedObject_Huge,
                CreatureDataConstants.AnimatedObject_Large,
                CreatureDataConstants.AnimatedObject_Medium,
                CreatureDataConstants.AnimatedObject_Small,
                CreatureDataConstants.AnimatedObject_Tiny,
                CreatureDataConstants.Golem,
                CreatureDataConstants.Homunculus,
                CreatureDataConstants.Retriever,
                CreatureDataConstants.ShieldGuardian,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Construct, creatures);
        }

        [Test]
        public void DragonGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Dragon_Black,
                CreatureDataConstants.Dragon_Blue,
                CreatureDataConstants.Dragon_Green,
                CreatureDataConstants.Dragon_Red,
                CreatureDataConstants.Dragon_White,
                CreatureDataConstants.Dragon_Brass,
                CreatureDataConstants.Dragon_Bronze,
                CreatureDataConstants.Dragon_Copper,
                CreatureDataConstants.Dragon_Gold,
                CreatureDataConstants.Dragon_Silver,
                CreatureDataConstants.DragonTurtle,
                CreatureDataConstants.Pseudodragon,
                CreatureDataConstants.Wyvern,
                CreatureDataConstants.Lammasu_GoldenProtector,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Dragon, creatures);
        }

        [Test]
        public void ElementalGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Belker,
                CreatureDataConstants.Elemental_Air,
                CreatureDataConstants.Elemental_Earth,
                CreatureDataConstants.Elemental_Fire,
                CreatureDataConstants.Elemental_Water,
                CreatureDataConstants.InvisibleStalker,
                CreatureDataConstants.Magmin,
                CreatureDataConstants.Thoqqua
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Elemental, creatures);
        }

        [Test]
        public void FeyGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Dryad,
                CreatureDataConstants.Nymph,
                CreatureDataConstants.Satyr,
                CreatureDataConstants.Sprite,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Fey, creatures);
        }

        [Test]
        public void GiantGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Ettin,
                CreatureDataConstants.Giant_Cloud,
                CreatureDataConstants.Giant_Fire,
                CreatureDataConstants.Giant_Frost,
                CreatureDataConstants.Giant_Hill,
                CreatureDataConstants.Giant_Stone,
                CreatureDataConstants.Giant_Storm,
                CreatureDataConstants.Ogre,
                CreatureDataConstants.Ogre_Merrow,
                CreatureDataConstants.OgreMage,
                CreatureDataConstants.Troll,
                CreatureDataConstants.Troll_Scrag,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Giant, creatures);
        }

        [Test]
        public void HumanoidGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Aasimar_Warrior,
                CreatureDataConstants.Bugbear,
                CreatureDataConstants.Character,
                CreatureDataConstants.Drow,
                CreatureDataConstants.Duergar,
                CreatureDataConstants.Dwarf,
                CreatureDataConstants.Elf,
                CreatureDataConstants.Elf_Aquatic,
                CreatureDataConstants.Gnoll,
                CreatureDataConstants.Gnome,
                CreatureDataConstants.Goblin,
                CreatureDataConstants.Halfling,
                CreatureDataConstants.Hobgoblin,
                CreatureDataConstants.Human,
                CreatureDataConstants.Kobold,
                CreatureDataConstants.KuoToa,
                CreatureDataConstants.Lizardfolk,
                CreatureDataConstants.Locathah,
                CreatureDataConstants.Merfolk,
                CreatureDataConstants.Orc,
                CreatureDataConstants.Orc_Half,
                CreatureDataConstants.Svirfneblin,
                CreatureDataConstants.Tiefling,
                CreatureDataConstants.Troglodyte,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Humanoid, creatures);
        }

        [Test]
        public void MagicalBeastsGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Ankheg,
                CreatureDataConstants.Aranea,
                CreatureDataConstants.Basilisk,
                CreatureDataConstants.Behir,
                CreatureDataConstants.BlinkDog,
                CreatureDataConstants.Bulette,
                CreatureDataConstants.CelestialCreature,
                CreatureDataConstants.Chimera,
                CreatureDataConstants.Cockatrice,
                CreatureDataConstants.Darkmantle,
                CreatureDataConstants.Digester,
                CreatureDataConstants.DisplacerBeast,
                CreatureDataConstants.Dragonne,
                CreatureDataConstants.Eagle_Giant,
                CreatureDataConstants.EtherealMarauder,
                CreatureDataConstants.FiendishCreature,
                CreatureDataConstants.FrostWorm,
                CreatureDataConstants.Girallon,
                CreatureDataConstants.Gorgon,
                CreatureDataConstants.GrayRender,
                CreatureDataConstants.Griffon,
                CreatureDataConstants.Hippogriff,
                CreatureDataConstants.Hydra,
                CreatureDataConstants.Kraken,
                CreatureDataConstants.Krenshar,
                CreatureDataConstants.Lamia,
                CreatureDataConstants.Lammasu,
                CreatureDataConstants.Manticore,
                CreatureDataConstants.Owl_Giant,
                CreatureDataConstants.Owlbear,
                CreatureDataConstants.Pegasus,
                CreatureDataConstants.PhaseSpider,
                CreatureDataConstants.PurpleWorm,
                CreatureDataConstants.RazorBoar,
                CreatureDataConstants.Remorhaz,
                CreatureDataConstants.Roper,
                CreatureDataConstants.SeaCat,
                CreatureDataConstants.ShockerLizard,
                CreatureDataConstants.Androsphinx,
                CreatureDataConstants.Criosphinx,
                CreatureDataConstants.Gynosphinx,
                CreatureDataConstants.Hieracosphinx,
                CreatureDataConstants.SpiderEater,
                CreatureDataConstants.Stirge,
                CreatureDataConstants.Tarrasque,
                CreatureDataConstants.Unicorn,
                CreatureDataConstants.WinterWolf,
                CreatureDataConstants.Worg,
                CreatureDataConstants.Yrthak,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.MagicalBeast, creatures);
        }

        [Test]
        public void MonstrousHumanoidGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Centaur,
                CreatureDataConstants.Derro,
                CreatureDataConstants.Doppelganger,
                CreatureDataConstants.Gargoyle,
                CreatureDataConstants.Gargoyle_Kapoacinth,
                CreatureDataConstants.Grimlock,
                CreatureDataConstants.Hag,
                CreatureDataConstants.Harpy,
                CreatureDataConstants.Werebear,
                CreatureDataConstants.Wereboar,
                CreatureDataConstants.Wererat,
                CreatureDataConstants.Weretiger,
                CreatureDataConstants.Werewolf,
                CreatureDataConstants.Medusa,
                CreatureDataConstants.Minotaur,
                CreatureDataConstants.Sahuagin,
                CreatureDataConstants.Scorpionfolk,
                CreatureDataConstants.YuanTi,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.MonstrousHumanoid, creatures);
        }

        [Test]
        public void OozeGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.BlackPudding,
                CreatureDataConstants.GelatinousCube,
                CreatureDataConstants.Ooze_Gray,
                CreatureDataConstants.Ooze_OchreJelly,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Ooze, creatures);
        }

        [Test]
        public void OutsiderGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Achaierai,
                //CreatureDataConstants.Angel,
                CreatureDataConstants.AstralDeva,
                CreatureDataConstants.Planetar,
                CreatureDataConstants.Solar,
                //CreatureDataConstants.Archon,
                CreatureDataConstants.HoundArchon,
                CreatureDataConstants.LanternArchon,
                CreatureDataConstants.TrumpetArchon,
                //CreatureDataConstants.Arrowhawk,
                CreatureDataConstants.Arrowhawk_Adult,
                CreatureDataConstants.Arrowhawk_Elder,
                CreatureDataConstants.Arrowhawk_Juvenile,
                CreatureDataConstants.Avoral,
                CreatureDataConstants.Azer,
                CreatureDataConstants.Barghest,
                CreatureDataConstants.Basilisk_AbyssalGreater,
                CreatureDataConstants.Bralani,
                CreatureDataConstants.ChaosBeast,
                CreatureDataConstants.Couatl,
                CreatureDataConstants.Babau,
                CreatureDataConstants.Balor,
                CreatureDataConstants.Bebilith,
                CreatureDataConstants.Dretch,
                CreatureDataConstants.Glabrezu,
                CreatureDataConstants.Hezrou,
                CreatureDataConstants.Marilith,
                CreatureDataConstants.Nalfeshnee,
                CreatureDataConstants.Quasit,
                CreatureDataConstants.Succubus,
                CreatureDataConstants.Vrock,
                CreatureDataConstants.Devil,
                CreatureDataConstants.Formian,
                CreatureDataConstants.Genie,
                CreatureDataConstants.Ghaele,
                CreatureDataConstants.Githyanki,
                CreatureDataConstants.Githzerai,
                CreatureDataConstants.HellHound,
                CreatureDataConstants.Hellwasp,
                CreatureDataConstants.Howler,
                CreatureDataConstants.Leonal,
                CreatureDataConstants.Lillend,
                CreatureDataConstants.Mephit,
                CreatureDataConstants.Mephit_CR3,
                CreatureDataConstants.NessianWarhound,
                CreatureDataConstants.NightHag,
                CreatureDataConstants.Nightmare,
                CreatureDataConstants.Rakshasa,
                CreatureDataConstants.Rast,
                CreatureDataConstants.Ravid,
                CreatureDataConstants.Salamander,
                CreatureDataConstants.ShadowMastiff,
                CreatureDataConstants.Slaad,
                CreatureDataConstants.Titan,
                CreatureDataConstants.Tojanida,
                CreatureDataConstants.Triton,
                CreatureDataConstants.Vargouille,
                CreatureDataConstants.Xill,
                CreatureDataConstants.Xorn,
                CreatureDataConstants.YethHound,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Outsider, creatures);
        }

        [Test]
        public void PlantGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.AssassinVine,
                CreatureDataConstants.Fungus,
                CreatureDataConstants.PhantomFungus,
                CreatureDataConstants.ShamblingMound,
                CreatureDataConstants.Tendriculos,
                CreatureDataConstants.Treant,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Plant, creatures);
        }

        [Test]
        public void UndeadGroup()
        {
            var creatures = new[]
            {
                CreatureDataConstants.Allip,
                CreatureDataConstants.Bodak,
                CreatureDataConstants.Devourer,
                CreatureDataConstants.Ghost,
                CreatureDataConstants.Ghoul,
                CreatureDataConstants.Ghoul_Lacedon,
                CreatureDataConstants.Lich,
                CreatureDataConstants.Mohrg,
                CreatureDataConstants.Mummy,
                CreatureDataConstants.Nightshade,
                CreatureDataConstants.Shadow,
                CreatureDataConstants.Skeleton,
                CreatureDataConstants.Spectre,
                CreatureDataConstants.Vampire,
                CreatureDataConstants.Wight,
                CreatureDataConstants.Wraith,
                CreatureDataConstants.Zombie,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Undead, creatures);
        }

        [Test]
        public void VerminGroup()
        {
            var creatures = new[]
            {
                //CreatureDataConstants.Ant_Giant,
                CreatureDataConstants.Ant_Giant_Queen,
                CreatureDataConstants.Ant_Giant_Soldier,
                CreatureDataConstants.Ant_Giant_Worker,
                CreatureDataConstants.Bee_Giant,
                CreatureDataConstants.BombardierBeetle_Giant,
                CreatureDataConstants.Centipede_Monstrous,
                CreatureDataConstants.Centipede_Swarm,
                CreatureDataConstants.FireBeetle_Giant,
                CreatureDataConstants.Locust,
                CreatureDataConstants.PrayingMantis_Giant,
                CreatureDataConstants.Scorpion_Monstrous,
                CreatureDataConstants.Spider_Monstrous,
                CreatureDataConstants.Spider_Swarm,
                CreatureDataConstants.StagBeetle_Giant,
                CreatureDataConstants.Wasp_Giant,
            };

            base.AssertDistinctCollection(CreatureDataConstants.Types.Vermin, creatures);
        }
    }
}
