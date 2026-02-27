using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Tables;
using DnDGen.TreasureGen.Items;
using DnDGen.TreasureGen.Items.Magical;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Integration.Tables.Treasures
{
    [TestFixture]
    public class TreasureGroupsTests : CollectionTests
    {
        internal IItemSelector itemSelector;

        protected override string tableName => TableNameConstants.TreasureGroups;

        [SetUp]
        public void Setup()
        {
            itemSelector = GetNewInstanceOf<IItemSelector>();
        }

        [Test]
        public override void EntriesAreComplete()
        {
            var allCreatures = GetAllCreaturesFromEncounters();
            var useSubcreatureForTreasure = collectionSelector.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, GroupConstants.UseSubcreatureForTreasure);
            allCreatures = allCreatures.Except(useSubcreatureForTreasure);

            AssertNamesAreComplete(allCreatures);
        }

        [TestCase(CreatureDataConstants.Aboleth)]
        [TestCase(CreatureDataConstants.Aboleth_Mage)]
        [TestCase(CreatureDataConstants.Achaierai)]
        [TestCase(CreatureDataConstants.Allip)]
        [TestCase(CreatureDataConstants.Androsphinx)]
        [TestCase(CreatureDataConstants.AnimatedObject_Colossal)]
        [TestCase(CreatureDataConstants.AnimatedObject_Gargantuan)]
        [TestCase(CreatureDataConstants.AnimatedObject_Huge)]
        [TestCase(CreatureDataConstants.AnimatedObject_Large)]
        [TestCase(CreatureDataConstants.AnimatedObject_Medium)]
        [TestCase(CreatureDataConstants.AnimatedObject_Small)]
        [TestCase(CreatureDataConstants.AnimatedObject_Tiny)]
        [TestCase(CreatureDataConstants.Ankheg)]
        [TestCase(CreatureDataConstants.Annis)]
        [TestCase(CreatureDataConstants.SeaHag)]
        [TestCase(CreatureDataConstants.Ant_Giant_Soldier)]
        [TestCase(CreatureDataConstants.Ant_Giant_Worker)]
        [TestCase(CreatureDataConstants.Ant_Giant_Queen)]
        [TestCase(CreatureDataConstants.Ape)]
        [TestCase(CreatureDataConstants.Ape_Dire)]
        [TestCase(CreatureDataConstants.Aranea)]
        [TestCase(CreatureDataConstants.Arrowhawk_Adult)]
        [TestCase(CreatureDataConstants.Arrowhawk_Elder)]
        [TestCase(CreatureDataConstants.Arrowhawk_Juvenile)]
        [TestCase(CreatureDataConstants.AssassinVine)]
        [TestCase(CreatureDataConstants.Avoral)]
        [TestCase(CreatureDataConstants.Babau)]
        [TestCase(CreatureDataConstants.Baboon)]
        [TestCase(CreatureDataConstants.Badger)]
        [TestCase(CreatureDataConstants.Badger_Dire)]
        [TestCase(CreatureDataConstants.Badger_Celestial)]
        [TestCase(CreatureDataConstants.Barghest)]
        [TestCase(CreatureDataConstants.Barghest_Greater)]
        [TestCase(CreatureDataConstants.Basilisk)]
        [TestCase(CreatureDataConstants.Basilisk_AbyssalGreater)]
        [TestCase(CreatureDataConstants.Bat)]
        [TestCase(CreatureDataConstants.Bat_Dire)]
        [TestCase(CreatureDataConstants.Bat_Swarm)]
        [TestCase(CreatureDataConstants.Bear_Black)]
        [TestCase(CreatureDataConstants.Bear_Brown)]
        [TestCase(CreatureDataConstants.Bear_Dire)]
        [TestCase(CreatureDataConstants.Bear_Polar)]
        [TestCase(CreatureDataConstants.Bebilith)]
        [TestCase(CreatureDataConstants.Bee_Giant)]
        [TestCase(CreatureDataConstants.Behir)]
        [TestCase(CreatureDataConstants.Beholder)]
        [TestCase(CreatureDataConstants.Belker)]
        [TestCase(CreatureDataConstants.Bison)]
        [TestCase(CreatureDataConstants.BlackPudding)]
        [TestCase(CreatureDataConstants.BlackPudding_Elder)]
        [TestCase(CreatureDataConstants.BlinkDog)]
        [TestCase(CreatureDataConstants.Boar)]
        [TestCase(CreatureDataConstants.Boar_Dire)]
        [TestCase(CreatureDataConstants.Bodak)]
        [TestCase(CreatureDataConstants.BombardierBeetle_Giant)]
        [TestCase(CreatureDataConstants.Bulette)]
        [TestCase(CreatureDataConstants.Camel)]
        [TestCase(CreatureDataConstants.CarrionCrawler)]
        [TestCase(CreatureDataConstants.Cat)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Colossal)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Gargantuan)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Huge)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Large)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Medium)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Small)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Tiny)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Fiendish_Colossal)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Fiendish_Gargantuan)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Fiendish_Huge)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Fiendish_Large)]
        [TestCase(CreatureDataConstants.Centipede_Monstrous_Fiendish_Medium)]
        [TestCase(CreatureDataConstants.Centipede_Swarm)]
        [TestCase(CreatureDataConstants.ChaosBeast)]
        [TestCase(CreatureDataConstants.Cheetah)]
        [TestCase(CreatureDataConstants.Chimera)]
        [TestCase(CreatureDataConstants.Choker)]
        [TestCase(CreatureDataConstants.Chuul)]
        [TestCase(CreatureDataConstants.Cloaker)]
        [TestCase(CreatureDataConstants.Cockatrice)]
        [TestCase(CreatureDataConstants.Couatl)]
        [TestCase(CreatureDataConstants.Criosphinx)]
        [TestCase(CreatureDataConstants.Crocodile)]
        [TestCase(CreatureDataConstants.Crocodile_Giant)]
        [TestCase(CreatureDataConstants.Cryohydra_10Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_11Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_12Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_5Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_6Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_7Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_8Heads)]
        [TestCase(CreatureDataConstants.Cryohydra_9Heads)]
        [TestCase(CreatureDataConstants.Darkmantle)]
        [TestCase(CreatureDataConstants.Deinonychus)]
        [TestCase(CreatureDataConstants.Delver)]
        [TestCase(CreatureDataConstants.Destrachan)]
        [TestCase(CreatureDataConstants.Devourer)]
        [TestCase(CreatureDataConstants.Digester)]
        [TestCase(CreatureDataConstants.DisplacerBeast)]
        [TestCase(CreatureDataConstants.DisplacerBeast_PackLord)]
        [TestCase(CreatureDataConstants.Djinni)]
        [TestCase(CreatureDataConstants.Djinni_Noble)]
        [TestCase(CreatureDataConstants.Dog)]
        [TestCase(CreatureDataConstants.Dog_Celestial)]
        [TestCase(CreatureDataConstants.Dog_Riding)]
        [TestCase(CreatureDataConstants.Donkey)]
        [TestCase(CreatureDataConstants.Doppelganger)]
        [TestCase(CreatureDataConstants.Dragon_Black_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Black_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Black_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Black_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Black_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Black_Old)]
        [TestCase(CreatureDataConstants.Dragon_Black_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Black_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Black_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Black_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Black_Young)]
        [TestCase(CreatureDataConstants.Dragon_Black_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Blue_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Blue_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Old)]
        [TestCase(CreatureDataConstants.Dragon_Blue_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Blue_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Blue_Young)]
        [TestCase(CreatureDataConstants.Dragon_Blue_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Green_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Green_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Green_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Green_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Green_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Green_Old)]
        [TestCase(CreatureDataConstants.Dragon_Green_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Green_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Green_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Green_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Green_Young)]
        [TestCase(CreatureDataConstants.Dragon_Green_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Red_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Red_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Red_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Red_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Red_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Red_Old)]
        [TestCase(CreatureDataConstants.Dragon_Red_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Red_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Red_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Red_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Red_Young)]
        [TestCase(CreatureDataConstants.Dragon_Red_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_White_Adult)]
        [TestCase(CreatureDataConstants.Dragon_White_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_White_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_White_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_White_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_White_Old)]
        [TestCase(CreatureDataConstants.Dragon_White_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_White_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_White_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_White_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_White_Young)]
        [TestCase(CreatureDataConstants.Dragon_White_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Old)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_Young)]
        [TestCase(CreatureDataConstants.Dragon_Bronze_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Copper_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Copper_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Old)]
        [TestCase(CreatureDataConstants.Dragon_Copper_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Copper_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Copper_Young)]
        [TestCase(CreatureDataConstants.Dragon_Copper_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Silver_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Silver_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Old)]
        [TestCase(CreatureDataConstants.Dragon_Silver_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Silver_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Silver_Young)]
        [TestCase(CreatureDataConstants.Dragon_Silver_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Gold_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Gold_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Old)]
        [TestCase(CreatureDataConstants.Dragon_Gold_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Gold_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Gold_Young)]
        [TestCase(CreatureDataConstants.Dragon_Gold_YoungAdult)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Adult)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Ancient)]
        [TestCase(CreatureDataConstants.Dragon_Brass_GreatWyrm)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Juvenile)]
        [TestCase(CreatureDataConstants.Dragon_Brass_MatureAdult)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Old)]
        [TestCase(CreatureDataConstants.Dragon_Brass_VeryOld)]
        [TestCase(CreatureDataConstants.Dragon_Brass_VeryYoung)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Wyrm)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Wyrmling)]
        [TestCase(CreatureDataConstants.Dragon_Brass_Young)]
        [TestCase(CreatureDataConstants.Dragon_Brass_YoungAdult)]
        [TestCase(CreatureDataConstants.DragonTurtle)]
        [TestCase(CreatureDataConstants.Dragonne)]
        [TestCase(CreatureDataConstants.Dretch)]
        [TestCase(CreatureDataConstants.Eagle)]
        [TestCase(CreatureDataConstants.Eagle_Giant)]
        [TestCase(CreatureDataConstants.Efreeti)]
        [TestCase(CreatureDataConstants.Elasmosaurus)]
        [TestCase(CreatureDataConstants.Elephant)]
        [TestCase(CreatureDataConstants.Elemental_Air_Elder)]
        [TestCase(CreatureDataConstants.Elemental_Air_Greater)]
        [TestCase(CreatureDataConstants.Elemental_Air_Huge)]
        [TestCase(CreatureDataConstants.Elemental_Air_Large)]
        [TestCase(CreatureDataConstants.Elemental_Air_Medium)]
        [TestCase(CreatureDataConstants.Elemental_Air_Small)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Elder)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Greater)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Huge)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Large)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Medium)]
        [TestCase(CreatureDataConstants.Elemental_Earth_Small)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Elder)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Greater)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Huge)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Large)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Medium)]
        [TestCase(CreatureDataConstants.Elemental_Fire_Small)]
        [TestCase(CreatureDataConstants.Elemental_Water_Elder)]
        [TestCase(CreatureDataConstants.Elemental_Water_Greater)]
        [TestCase(CreatureDataConstants.Elemental_Water_Huge)]
        [TestCase(CreatureDataConstants.Elemental_Water_Large)]
        [TestCase(CreatureDataConstants.Elemental_Water_Medium)]
        [TestCase(CreatureDataConstants.Elemental_Water_Small)]
        [TestCase(CreatureDataConstants.EtherealFilcher)]
        [TestCase(CreatureDataConstants.EtherealMarauder)]
        [TestCase(CreatureDataConstants.Ettercap)]
        [TestCase(CreatureDataConstants.FireBeetle_Giant)]
        [TestCase(CreatureDataConstants.FireBeetle_Giant_Celestial)]
        [TestCase(CreatureDataConstants.FormianQueen)]
        [TestCase(CreatureDataConstants.FormianTaskmaster)]
        [TestCase(CreatureDataConstants.FormianWarrior)]
        [TestCase(CreatureDataConstants.FormianWorker)]
        [TestCase(CreatureDataConstants.FrostWorm)]
        [TestCase(CreatureDataConstants.Gargoyle)]
        [TestCase(CreatureDataConstants.Gargoyle_Kapoacinth)]
        [TestCase(CreatureDataConstants.GelatinousCube)]//Stone and metal only
        [TestCase(CreatureDataConstants.Ghoul)]
        [TestCase(CreatureDataConstants.Ghoul_Lacedon)]
        [TestCase(CreatureDataConstants.Ghoul_Ghast)]
        [TestCase(CreatureDataConstants.GibberingMouther)]
        [TestCase(CreatureDataConstants.Girallon)]
        [TestCase(CreatureDataConstants.Glabrezu)]
        [TestCase(CreatureDataConstants.Golem_Clay)]
        [TestCase(CreatureDataConstants.Golem_Flesh)]
        [TestCase(CreatureDataConstants.Golem_Iron)]
        [TestCase(CreatureDataConstants.Golem_Stone)]
        [TestCase(CreatureDataConstants.Golem_Stone_Greater)]
        [TestCase(CreatureDataConstants.Gorgon)]
        [TestCase(CreatureDataConstants.GrayRender)]
        [TestCase(CreatureDataConstants.GreenHag)]
        [TestCase(CreatureDataConstants.Grick)]
        [TestCase(CreatureDataConstants.Griffon)]
        [TestCase(CreatureDataConstants.Gynosphinx)]
        [TestCase(CreatureDataConstants.BarbedDevil_Hamatula)]
        [TestCase(CreatureDataConstants.Hawk)]
        [TestCase(CreatureDataConstants.Hellcat_Bezekira)]
        [TestCase(CreatureDataConstants.HellHound)]
        [TestCase(CreatureDataConstants.Hellwasp_Swarm)]
        [TestCase(CreatureDataConstants.Hezrou)]
        [TestCase(CreatureDataConstants.Hieracosphinx)]
        [TestCase(CreatureDataConstants.Hippogriff)]
        [TestCase(CreatureDataConstants.Homunculus)]
        [TestCase(CreatureDataConstants.Horse_Heavy)]
        [TestCase(CreatureDataConstants.Horse_Heavy_War)]
        [TestCase(CreatureDataConstants.Horse_Light)]
        [TestCase(CreatureDataConstants.Horse_Light_War)]
        [TestCase(CreatureDataConstants.Howler)]
        [TestCase(CreatureDataConstants.Hydra_10Heads)]
        [TestCase(CreatureDataConstants.Hydra_11Heads)]
        [TestCase(CreatureDataConstants.Hydra_12Heads)]
        [TestCase(CreatureDataConstants.Hydra_5Heads)]
        [TestCase(CreatureDataConstants.Hydra_6Heads)]
        [TestCase(CreatureDataConstants.Hydra_7Heads)]
        [TestCase(CreatureDataConstants.Hydra_8Heads)]
        [TestCase(CreatureDataConstants.Hydra_9Heads)]
        [TestCase(CreatureDataConstants.Hyena)]
        [TestCase(CreatureDataConstants.Imp)]
        [TestCase(CreatureDataConstants.InvisibleStalker)]
        [TestCase(CreatureDataConstants.Kraken)]
        [TestCase(CreatureDataConstants.Krenshar)]
        [TestCase(CreatureDataConstants.Lammasu)]
        [TestCase(CreatureDataConstants.Lammasu_GoldenProtector)]
        [TestCase(CreatureDataConstants.LanternArchon)]
        [TestCase(CreatureDataConstants.Lemure)]
        [TestCase(CreatureDataConstants.Leonal)]
        [TestCase(CreatureDataConstants.Leopard)]
        [TestCase(CreatureDataConstants.Lion)]
        [TestCase(CreatureDataConstants.Lion_Dire)]
        [TestCase(CreatureDataConstants.Lizard)]
        [TestCase(CreatureDataConstants.Lizard_Monitor)]
        [TestCase(CreatureDataConstants.Locust_Swarm)]
        [TestCase(CreatureDataConstants.Magmin)] //Nonflammable
        [TestCase(CreatureDataConstants.MantaRay)]
        [TestCase(CreatureDataConstants.Manticore)]
        [TestCase(CreatureDataConstants.Marut)]
        [TestCase(CreatureDataConstants.Megaraptor)]
        [TestCase(CreatureDataConstants.Mephit_CR3)]
        [TestCase(CreatureDataConstants.Mephit_Air)]
        [TestCase(CreatureDataConstants.Mephit_Dust)]
        [TestCase(CreatureDataConstants.Mephit_Earth)]
        [TestCase(CreatureDataConstants.Mephit_Fire)]
        [TestCase(CreatureDataConstants.Mephit_Ice)]
        [TestCase(CreatureDataConstants.Mephit_Magma)]
        [TestCase(CreatureDataConstants.Mephit_Ooze)]
        [TestCase(CreatureDataConstants.Mephit_Salt)]
        [TestCase(CreatureDataConstants.Mephit_Steam)]
        [TestCase(CreatureDataConstants.Mephit_Water)]
        [TestCase(CreatureDataConstants.Mimic)]
        [TestCase(CreatureDataConstants.MindFlayer)]
        [TestCase(CreatureDataConstants.Mohrg)]
        [TestCase(CreatureDataConstants.Monkey)]
        [TestCase(CreatureDataConstants.Monkey_Celestial)]
        [TestCase(CreatureDataConstants.Mule)]
        [TestCase(CreatureDataConstants.Mummy)]
        [TestCase(CreatureDataConstants.Naga_Dark)]
        [TestCase(CreatureDataConstants.Naga_Guardian)]
        [TestCase(CreatureDataConstants.Naga_Spirit)]
        [TestCase(CreatureDataConstants.Naga_Water)]
        [TestCase(CreatureDataConstants.Nalfeshnee)]
        [TestCase(CreatureDataConstants.NightHag)]
        [TestCase(CreatureDataConstants.Nightcrawler)]
        [TestCase(CreatureDataConstants.Nightmare)]
        [TestCase(CreatureDataConstants.Nightmare_Cauchemar)]
        [TestCase(CreatureDataConstants.Nightwalker)]
        [TestCase(CreatureDataConstants.Nightwing)]
        [TestCase(CreatureDataConstants.Octopus)]
        [TestCase(CreatureDataConstants.Octopus_Giant)]
        [TestCase(CreatureDataConstants.Ooze_Gray)]
        [TestCase(CreatureDataConstants.Ooze_OchreJelly)]
        [TestCase(CreatureDataConstants.Otyugh)]
        [TestCase(CreatureDataConstants.BoneDevil_Osyluth)]
        [TestCase(CreatureDataConstants.Owl)]
        [TestCase(CreatureDataConstants.Owl_Giant)]
        [TestCase(CreatureDataConstants.Owl_Celestial)]
        [TestCase(CreatureDataConstants.Owlbear)]
        [TestCase(CreatureDataConstants.Pegasus)]
        [TestCase(CreatureDataConstants.PhantomFungus)]
        [TestCase(CreatureDataConstants.PhaseSpider)]
        [TestCase(CreatureDataConstants.Phasm)]
        [TestCase(CreatureDataConstants.PitFiend)]
        [TestCase(CreatureDataConstants.Pony)]
        [TestCase(CreatureDataConstants.Pony_War)]
        [TestCase(CreatureDataConstants.Porpoise)]
        [TestCase(CreatureDataConstants.Porpoise_Celestial)]
        [TestCase(CreatureDataConstants.PrayingMantis_Giant)]
        [TestCase(CreatureDataConstants.Pseudodragon)]
        [TestCase(CreatureDataConstants.PurpleWorm)]//Only stone goods
        [TestCase(CreatureDataConstants.Pyrohydra_10Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_11Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_12Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_5Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_6Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_7Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_8Heads)]
        [TestCase(CreatureDataConstants.Pyrohydra_9Heads)]
        [TestCase(CreatureDataConstants.Quasit)]
        [TestCase(CreatureDataConstants.Rakshasa)]
        [TestCase(CreatureDataConstants.Rast)]
        [TestCase(CreatureDataConstants.Rat)]
        [TestCase(CreatureDataConstants.Rat_Dire)]
        [TestCase(CreatureDataConstants.Rat_Dire_Fiendish)]
        [TestCase(CreatureDataConstants.Rat_Swarm)]
        [TestCase(CreatureDataConstants.Raven)]
        [TestCase(CreatureDataConstants.Raven_Fiendish)]
        [TestCase(CreatureDataConstants.Ravid)]
        [TestCase(CreatureDataConstants.RazorBoar)]
        [TestCase(CreatureDataConstants.Remorhaz)]
        [TestCase(CreatureDataConstants.Retriever)]
        [TestCase(CreatureDataConstants.Rhinoceras)]
        [TestCase(CreatureDataConstants.Roc)]
        [TestCase(CreatureDataConstants.Roper)]//only stone goods
        [TestCase(CreatureDataConstants.RustMonster)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Colossal)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Gargantuan)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Huge)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Large)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Medium)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Small)]
        [TestCase(CreatureDataConstants.Scorpion_Monstrous_Tiny)]
        [TestCase(CreatureDataConstants.SeaCat)]
        [TestCase(CreatureDataConstants.Shadow)]
        [TestCase(CreatureDataConstants.Shadow_Greater)]
        [TestCase(CreatureDataConstants.ShadowMastiff)]
        [TestCase(CreatureDataConstants.ShamblingMound)]
        [TestCase(CreatureDataConstants.Shark_Dire)]
        [TestCase(CreatureDataConstants.Shark_Huge)]
        [TestCase(CreatureDataConstants.Shark_Large)]
        [TestCase(CreatureDataConstants.Shark_Medium)]
        [TestCase(CreatureDataConstants.ShieldGuardian)]
        [TestCase(CreatureDataConstants.ShockerLizard)]
        [TestCase(CreatureDataConstants.Shrieker)]
        [TestCase(CreatureDataConstants.Skeleton_Chimera)]
        [TestCase(CreatureDataConstants.Skeleton_Dragon_Red_YoungAdult)]
        [TestCase(CreatureDataConstants.Skeleton_Ettin)]
        [TestCase(CreatureDataConstants.Skeleton_Giant_Cloud)]
        [TestCase(CreatureDataConstants.Skeleton_Human)]
        [TestCase(CreatureDataConstants.Skeleton_Megaraptor)]
        [TestCase(CreatureDataConstants.Skeleton_Owlbear)]
        [TestCase(CreatureDataConstants.Skeleton_Troll)]
        [TestCase(CreatureDataConstants.Skeleton_Wolf)]
        [TestCase(CreatureDataConstants.Skum)]
        [TestCase(CreatureDataConstants.Slaad_Blue)]
        [TestCase(CreatureDataConstants.Slaad_Death)]
        [TestCase(CreatureDataConstants.Slaad_Gray)]
        [TestCase(CreatureDataConstants.Slaad_Green)]
        [TestCase(CreatureDataConstants.Slaad_Red)]
        [TestCase(CreatureDataConstants.Snake_Constrictor)]
        [TestCase(CreatureDataConstants.Snake_Constrictor_Giant)]
        [TestCase(CreatureDataConstants.Snake_Viper_Huge)]
        [TestCase(CreatureDataConstants.Snake_Viper_Large)]
        [TestCase(CreatureDataConstants.Snake_Viper_Medium)]
        [TestCase(CreatureDataConstants.Snake_Viper_Small)]
        [TestCase(CreatureDataConstants.Snake_Viper_Tiny)]
        [TestCase(CreatureDataConstants.Spectre)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Colossal)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Gargantuan)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Huge)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Large)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Medium)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Small)]
        [TestCase(CreatureDataConstants.Spider_Monstrous_Tiny)]
        [TestCase(CreatureDataConstants.Spider_Swarm)]
        [TestCase(CreatureDataConstants.SpiderEater)]
        [TestCase(CreatureDataConstants.Squid)]
        [TestCase(CreatureDataConstants.Squid_Giant)]
        [TestCase(CreatureDataConstants.StagBeetle_Giant)]
        [TestCase(CreatureDataConstants.Stirge)]
        [TestCase(CreatureDataConstants.Succubus)]
        [TestCase(CreatureDataConstants.Tarrasque)]
        [TestCase(CreatureDataConstants.Tendriculos)]
        [TestCase(CreatureDataConstants.Thoqqua)]
        [TestCase(CreatureDataConstants.Tiger)]
        [TestCase(CreatureDataConstants.Tiger_Dire)]
        [TestCase(CreatureDataConstants.Toad)]
        [TestCase(CreatureDataConstants.Tojanida_Adult)]
        [TestCase(CreatureDataConstants.Tojanida_Elder)]
        [TestCase(CreatureDataConstants.Tojanida_Juvenile)]
        [TestCase(CreatureDataConstants.Treant)]
        [TestCase(CreatureDataConstants.Triceratops)]
        [TestCase(CreatureDataConstants.Troll)]
        [TestCase(CreatureDataConstants.Troll_Scrag)]
        [TestCase(CreatureDataConstants.Tyrannosaurus)]
        [TestCase(CreatureDataConstants.UmberHulk)]
        [TestCase(CreatureDataConstants.UmberHulk_TrulyHorrid)]
        [TestCase(CreatureDataConstants.Unicorn)]
        [TestCase(CreatureDataConstants.Unicorn_CelestialCharger)]
        [TestCase(CreatureDataConstants.VampireSpawn)]
        [TestCase(CreatureDataConstants.Vargouille)]
        [TestCase(CreatureDataConstants.VioletFungus)]
        [TestCase(CreatureDataConstants.Vrock)]
        [TestCase(CreatureDataConstants.Wasp_Giant)]
        [TestCase(CreatureDataConstants.Weasel)]
        [TestCase(CreatureDataConstants.Weasel_Dire)]
        [TestCase(CreatureDataConstants.Whale_Baleen)]
        [TestCase(CreatureDataConstants.Whale_Cachalot)]
        [TestCase(CreatureDataConstants.Whale_Orca)]
        [TestCase(CreatureDataConstants.Wight)]
        [TestCase(CreatureDataConstants.WillOWisp)]
        [TestCase(CreatureDataConstants.WinterWolf)]
        [TestCase(CreatureDataConstants.Wolf)]
        [TestCase(CreatureDataConstants.Wolf_Dire)]
        [TestCase(CreatureDataConstants.Wolverine)]
        [TestCase(CreatureDataConstants.Wolverine_Dire)]
        [TestCase(CreatureDataConstants.Worg)]
        [TestCase(CreatureDataConstants.Wraith)]
        [TestCase(CreatureDataConstants.Wraith_Dread)]
        [TestCase(CreatureDataConstants.Wyvern)]
        [TestCase(CreatureDataConstants.Xill)]
        [TestCase(CreatureDataConstants.Xorn_Average)]
        [TestCase(CreatureDataConstants.Xorn_Elder)]
        [TestCase(CreatureDataConstants.Xorn_Minor)]
        [TestCase(CreatureDataConstants.YethHound)]
        [TestCase(CreatureDataConstants.Yrthak)]
        [TestCase(CreatureDataConstants.Zombie_Bugbear)]
        [TestCase(CreatureDataConstants.Zombie_GrayRender)]
        [TestCase(CreatureDataConstants.Zombie_Human)]
        [TestCase(CreatureDataConstants.Zombie_Kobold)]
        [TestCase(CreatureDataConstants.Zombie_Minotaur)]
        [TestCase(CreatureDataConstants.Zombie_Ogre)]
        [TestCase(CreatureDataConstants.Zombie_Troglodyte)]
        [TestCase(CreatureDataConstants.Zombie_Wyvern)]
        public void TreasureGroupIsEmpty(string entry)
        {
            base.AssertDistinctCollection(entry);
        }

        [TestCase(CreatureDataConstants.AstralDeva, WeaponConstants.HeavyMace, ItemTypeConstants.Weapon, 3, SpecialAbilityConstants.Disruption, TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.BeardedDevil_Barbazu, WeaponConstants.Glaive, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.ChainDevil_Kyton, WeaponConstants.SpikedChain, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.FormianMyrmarch, WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Ghaele, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 4, SpecialAbilityConstants.Holy, TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.Giant_Cloud, WeaponConstants.Morningstar, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Gargantuan)]
        [TestCase(CreatureDataConstants.Giant_Fire, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Giant_Stone, WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Giant_Stone_Elder, WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Grimlock, WeaponConstants.Battleaxe, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)] //only gems, no art objects
        [TestCase(CreatureDataConstants.Harpy, WeaponConstants.Club, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.HornedDevil_Cornugon, WeaponConstants.SpikedChain, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.HoundArchon, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.IceDevil_Gelugon, WeaponConstants.Longspear, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Kolyarut, WeaponConstants.Longsword, ItemTypeConstants.Weapon, 2, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.KuoToa, WeaponConstants.Shortspear, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.Lamia, WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Lillend, WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Minotaur, WeaponConstants.Greataxe, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.NessianWarhound, ArmorConstants.ChainShirt, ItemTypeConstants.Armor, 2, "", "Barding", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Nymph, WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)]
        [TestCase(CreatureDataConstants.Planetar, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 3, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.Salamander_Average, WeaponConstants.Shortspear, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Medium)] //Non-flammable
        [TestCase(CreatureDataConstants.Salamander_Flamebrother, WeaponConstants.Shortspear, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Small)]//Non-flammable
        [TestCase(CreatureDataConstants.Salamander_Noble, WeaponConstants.Longspear, ItemTypeConstants.Weapon, 3, "", TraitConstants.Sizes.Large)]//Non-flammable
        [TestCase(CreatureDataConstants.Scorpionfolk, WeaponConstants.Lance, ItemTypeConstants.Weapon, 0, "", TraitConstants.Sizes.Large)]
        [TestCase(CreatureDataConstants.TrumpetArchon, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 4, "", TraitConstants.Sizes.Medium)]
        public void TreasureGroupOfOnePresetItem(string entry, string itemName, string itemType, int itemBonus, string abilityName, params string[] traits)
        {
            var setItem = FormatSetItem(itemName, itemType, itemBonus, abilityName, 0, false, traits);
            base.AssertDistinctCollection(entry, [setItem]);
        }

        private string FormatSetItem(string itemName, string itemType, int itemBonus = 0, string abilityName = "", int abilityBonus = 0, bool isMagic = false, params string[] traits)
        {
            var item = new Item
            {
                Name = itemName,
                ItemType = itemType
            };
            item.Magic.Bonus = itemBonus;

            if (!string.IsNullOrEmpty(abilityName))
            {
                var ability = new SpecialAbility
                {
                    Name = abilityName,
                    BonusEquivalent = abilityBonus
                };

                item.Magic.SpecialAbilities = [ability];
            }

            item.IsMagical = isMagic;

            foreach (var trait in traits)
                item.Traits.Add(trait);

            return itemSelector.SelectFrom(item);
        }

        [Test]
        public void AthachItems()
        {
            var morningstar = FormatSetItem(WeaponConstants.Morningstar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Huge);

            base.AssertCollection(CreatureDataConstants.Athach, [morningstar, morningstar, morningstar]);
        }

        [Test]
        public void AzerItems()
        {
            var warhammer = FormatSetItem(WeaponConstants.Warhammer, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var spear = FormatSetItem(WeaponConstants.Shortspear, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Azer, [warhammer, spear]);
        }

        [Test]
        public void BalorItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Vorpal, 0, false, TraitConstants.Sizes.Large);
            var whip = FormatSetItem(WeaponConstants.Whip, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Flaming, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Balor, [longsword, whip]);
        }

        [Test]
        public void BralaniItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Holy, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Holy, 0, false, "+4 Strength bonus", TraitConstants.Sizes.Medium);
            base.AssertDistinctCollection(CreatureDataConstants.Bralani, [scimitar, longbow]);
        }

        [Test]
        public void BugbearItems()
        {
            var morningstar = FormatSetItem(WeaponConstants.Morningstar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Bugbear, [morningstar, javelin]);
        }

        [Test]
        public void CentaurItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large, "+4 Strength bonus");

            base.AssertDistinctCollection(CreatureDataConstants.Centaur, [longsword, longbow]);
        }

        [Test]
        public void DerroItems()
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);
            var crossbow = FormatSetItem(WeaponConstants.LightRepeatingCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);
            base.AssertDistinctCollection(CreatureDataConstants.Derro, [shortSword, crossbow]);
        }

        [Test]
        public void DriderItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertCollection(CreatureDataConstants.Drider, [dagger, dagger, shortbow]);
        }

        [Test]
        public void DryadItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium, TraitConstants.Masterwork);

            base.AssertDistinctCollection(CreatureDataConstants.Dryad, [dagger, longbow]);
        }

        [Test]
        public void ErinyesItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Flaming, 0, false, "+5 Strength bonus", TraitConstants.Sizes.Medium);
            var rope = FormatSetItem("Rope, 50 ft.", ItemTypeConstants.Tool);

            base.AssertDistinctCollection(CreatureDataConstants.Erinyes, [longsword, longbow, rope]);
        }

        [Test]
        public void EttinItems()
        {
            var morningstar = FormatSetItem(WeaponConstants.Morningstar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Ettin, [morningstar, javelin]);
        }

        [Test]
        public void FrostGiantItems()
        {
            var greataxe = FormatSetItem(WeaponConstants.Greataxe, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var armor = FormatSetItem(ArmorConstants.ChainShirt, ItemTypeConstants.Armor, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Giant_Frost, [greataxe, armor]);
        }

        [Test]
        public void HillGiantItems()
        {
            var greatclub = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var armor = FormatSetItem(ArmorConstants.HideArmor, ItemTypeConstants.Armor, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Giant_Hill, [greatclub, armor]);
        }

        [Test]
        public void GnollItems()
        {
            var battleaxe = FormatSetItem(WeaponConstants.Battleaxe, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Gnoll, [battleaxe, shortbow]);
        }

        [Test]
        public void GrigItems()
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Tiny);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Tiny);

            base.AssertDistinctCollection(CreatureDataConstants.Grig, [shortSword, longbow]);
        }

        [Test]
        public void JanniItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Janni, [scimitar, longbow]);
        }

        [Test]
        public void LizardfolkItems()
        {
            var club = FormatSetItem(WeaponConstants.Club, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Lizardfolk, [club, javelin]);
        }

        [Test]
        public void MarilithItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertCollection(CreatureDataConstants.Marilith, [longsword, longsword, longsword, longsword, longsword, longsword]);
        }

        [Test]
        public void MedusaItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Medusa, [dagger, shortbow]);
        }

        [Test]
        public void NixieItems()
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);
            var crossbow = FormatSetItem(WeaponConstants.LightCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);

            base.AssertDistinctCollection(CreatureDataConstants.Nixie, [shortSword, crossbow]);
        }

        [Test]
        public void OgreItems()
        {
            var greatclub = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Ogre, [greatclub, javelin]);
        }

        [Test]
        public void OgreMerrowItems()
        {
            var longspear = FormatSetItem(WeaponConstants.Longspear, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Ogre_Merrow, [longspear, javelin]);
        }

        [Test]
        public void OgreMageItems()
        {
            var greatsword = FormatSetItem(WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.OgreMage, [greatsword, longbow]);
        }

        [Test]
        public void PixieItems()
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);

            base.AssertDistinctCollection(CreatureDataConstants.Pixie, [shortSword, longbow]);
        }

        [Test]
        public void PixieWithIrresistableDanceItems()
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Small);

            base.AssertDistinctCollection(CreatureDataConstants.Pixie_WithIrresistableDance, [shortSword, longbow]);
        }

        [Test]
        public void SahuaginItems()
        {
            var trident = FormatSetItem(WeaponConstants.Trident, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var crossbow = FormatSetItem(WeaponConstants.HeavyCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Sahuagin, [trident, crossbow]);
        }

        [Test]
        public void SatyrItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Satyr, [dagger, shortbow]);
        }

        [Test]
        public void SatyrWithPipesItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var pipes = FormatSetItem("Pipes", ItemTypeConstants.Tool);

            base.AssertDistinctCollection(CreatureDataConstants.Satyr_WithPipes, [dagger, shortbow, pipes]);
        }

        [Test]
        public void SolarItems()
        {
            var greatsword = FormatSetItem(WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 5, SpecialAbilityConstants.Dancing, 0, false, TraitConstants.Sizes.Large);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 2, "Creates slaying arrows keyed to any creature type or subtype", 5, false, "+5 Strength bonus", TraitConstants.Sizes.Large);

            base.AssertDistinctCollection(CreatureDataConstants.Solar, [greatsword, longbow]);
        }

        [Test]
        public void StormGiantItems()
        {
            var greatsword = FormatSetItem(WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Huge);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Huge, "+14 Strength bonus");

            base.AssertDistinctCollection(CreatureDataConstants.Giant_Storm, [greatsword, longbow]);
        }

        [Test]
        public void TitanItems()
        {
            var warhammer = FormatSetItem(WeaponConstants.Warhammer, ItemTypeConstants.Weapon, 3, string.Empty, 0, false, TraitConstants.Sizes.Gargantuan, TraitConstants.SpecialMaterials.Adamantine);
            var armor = FormatSetItem(ArmorConstants.HalfPlate, ItemTypeConstants.Armor, 4, string.Empty, 0, false, TraitConstants.Sizes.Huge);

            base.AssertDistinctCollection(CreatureDataConstants.Titan, [warhammer, armor]);
        }

        [Test]
        public void TritonItems()
        {
            var trident = FormatSetItem(WeaponConstants.Trident, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var crossbow = FormatSetItem(WeaponConstants.HeavyCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Triton, [trident, crossbow]);
        }

        [Test]
        public void TroglodyteItems()
        {
            var club = FormatSetItem(WeaponConstants.Club, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Troglodyte, [club, javelin]);
        }

        [Test]
        public void WerebearItems()
        {
            var greataxe = FormatSetItem(WeaponConstants.Greataxe, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var throwingAxe = FormatSetItem(WeaponConstants.ThrowingAxe, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Werebear, [greataxe, throwingAxe]);
        }

        [Test]
        public void WereboarItems()
        {
            var battleaxe = FormatSetItem(WeaponConstants.Battleaxe, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var armor = FormatSetItem(ArmorConstants.ScaleMail, ItemTypeConstants.Armor, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var shield = FormatSetItem(ArmorConstants.HeavyWoodenShield, ItemTypeConstants.Armor, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Wereboar, [battleaxe, javelin, armor, shield]);
        }

        [Test]
        public void HillGiantDireWereboarItems()
        {
            var greatclub = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var armor = FormatSetItem(ArmorConstants.HideArmor, ItemTypeConstants.Armor, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            base.AssertDistinctCollection(CreatureDataConstants.Wereboar_HillGiantDire, [greatclub, armor]);
        }

        [Test]
        public void WereratItems()
        {
            var rapier = FormatSetItem(WeaponConstants.Rapier, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var lightCrossbow = FormatSetItem(WeaponConstants.LightCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Wererat, [rapier, lightCrossbow]);
        }

        [Test]
        public void WeretigerItems()
        {
            var glaive = FormatSetItem(WeaponConstants.Glaive, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium, "+1 Strength bonus");

            base.AssertDistinctCollection(CreatureDataConstants.Weretiger, [glaive, longbow]);
        }

        [Test]
        public void WerewolfItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var lightCrossbow = FormatSetItem(WeaponConstants.LightCrossbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.Werewolf, [longsword, lightCrossbow]);
        }

        [Test]
        public void YuanTiPurebloodItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);

            base.AssertDistinctCollection(CreatureDataConstants.YuanTi_Pureblood, [scimitar, longbow]);
        }

        [Test]
        public void YuanTiHalfbloodItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Medium, "+2 Strength bonus");

            base.AssertDistinctCollection(CreatureDataConstants.YuanTi_Halfblood, [scimitar, longbow]);
        }

        [Test]
        public void YuanTiAbominationItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large, "+4 Strength bonus");

            base.AssertDistinctCollection(CreatureDataConstants.YuanTi_Abomination, [scimitar, longbow]);
        }

        [Test]
        public void ZelekhutItems()
        {
            var chain = FormatSetItem(WeaponConstants.SpikedChain, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, TraitConstants.Sizes.Large);

            base.AssertCollection(CreatureDataConstants.Zelekhut, [chain, chain]);
        }

        [Test]
        public void CharactersDoNotHaveSetItems()
        {
            var entries = GetEntries();
            var characters = entries.Where(e => e.Contains(CreatureDataConstants.Character));
            Assert.That(characters, Is.Not.Empty);

            foreach (var character in characters)
            {
                AssertDistinctCollection(character);
            }
        }

        [Test]
        public void NoncombatantsDoNotHaveSetItems()
        {
            var entries = GetEntries();
            var noncombatants = entries.Where(e => e.Contains(CreatureDataConstants.Noncombatant));
            Assert.That(noncombatants, Is.Not.Empty);

            foreach (var noncombatant in noncombatants)
            {
                AssertDistinctCollection(noncombatant);
            }
        }

        [Test]
        public void AllWeaponsAndArmorHaveSize()
        {
            var entries = GetEntries();
            var sizes = TraitConstants.Sizes.GetAll();

            foreach (var entry in entries)
            {
                var items = GetCollection(entry);

                var weapons = items.Where(i => i.Contains(ItemTypeConstants.Weapon));
                foreach (var weapon in weapons)
                    Assert.That(sizes.Any(weapon.Contains), Is.True, $"Item {weapon} for {entry}");

                var armors = items.Where(i => i.Contains(ItemTypeConstants.Armor));
                foreach (var armor in armors)
                    Assert.That(sizes.Any(armor.Contains), Is.True, $"Item {armor} for {entry}");
            }
        }
    }
}
