using DnDGen.CharacterGen.Characters;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Generators;
using DnDGen.RollGen;
using DnDGen.TreasureGen.Coins;
using DnDGen.TreasureGen.Generators;
using DnDGen.TreasureGen.Goods;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;

namespace DnDGen.EncounterGen.Tests.Integration.IoC.Modules
{
    [TestFixture]
    public class GeneratorModuleTests : IoCTests
    {
        [Test]
        public void EncounterGeneratorIsNotASingleton()
        {
            AssertNotSingleton<IEncounterGenerator>();
        }

        [Test]
        public void CreatureGeneratorIsNotASingleton()
        {
            AssertNotSingleton<ICreatureGenerator>();
        }

        [Test]
        public void EncounterCharacterGeneratorIsNotASingleton()
        {
            AssertNotSingleton<IEncounterCharacterGenerator>();
        }

        [Test]
        public void EncounterTreasureGeneratorIsNotASingleton()
        {
            AssertNotSingleton<IEncounterTreasureGenerator>();
        }

        [Test]
        public void EncounterVerifierIsNotASingleton()
        {
            AssertNotSingleton<IEncounterVerifier>();
        }

        [Test]
        public void EXTERNAL_CharacterGeneratorIsInjected()
        {
            AssertNotSingleton<ICharacterGenerator>();
        }

        [Test]
        public void EXTERNAL_TreasureGeneratorIsInjected()
        {
            AssertNotSingleton<ITreasureGenerator>();
        }

        [Test]
        public void EXTERNAL_CoinGeneratorIsInjected()
        {
            AssertNotSingleton<ICoinGenerator>();
        }

        [Test]
        public void EXTERNAL_GoodsGeneratorIsInjected()
        {
            AssertNotSingleton<IGoodsGenerator>();
        }

        [Test]
        public void EXTERNAL_ItemsGeneratorIsInjected()
        {
            AssertNotSingleton<IItemsGenerator>();
        }

        [Test]
        public void EXTERNAL_DiceIsInjected()
        {
            AssertNotSingleton<Dice>();
        }

        [Test]
        public void EXTERNAL_JustInTimeFactoryIsInjected()
        {
            AssertNotSingleton<JustInTimeFactory>();
        }
    }
}
