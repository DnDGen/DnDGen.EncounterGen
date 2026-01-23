using DnDGen.EncounterGen.Generators;
using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Selectors.Collections;
using DnDGen.EncounterGen.Selectors.Selections;
using DnDGen.EncounterGen.Tables;
using DnDGen.Infrastructure.Factories;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.Infrastructure.Selectors.Percentiles;
using DnDGen.TreasureGen.Coins;
using DnDGen.TreasureGen.Goods;
using DnDGen.TreasureGen.Items;
using DnDGen.TreasureGen.Items.Magical;
using DnDGen.TreasureGen.Items.Mundane;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Unit.Generators
{
    [TestFixture]
    public class EncounterTreasureGeneratorTests
    {
        private IEncounterTreasureGenerator encounterTreasureGenerator;
        private Mock<ICoinGenerator> mockCoinGenerator;
        private Mock<IGoodsGenerator> mockGoodsGenerator;
        private Mock<IItemsGenerator> mockItemsGenerator;
        private Mock<ITreasureRatesSelector> mockTreasureAdjustmentSelector;
        private Mock<IPercentileSelector> mockPercentileSelector;
        private Mock<ICollectionSelector> mockCollectionSelector;
        private int level;
        private List<EncounterCreature> creatures;
        private EncounterCreature encounterCreature;
        private TreasureRatesSelection treasureRates;
        private List<Good> goods;
        private List<Item> items;
        private Coin coin;
        private List<string> usesSubtypeForTreasure;
        private Mock<IItemSelector> mockItemSelector;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;

        [SetUp]
        public void Setup()
        {
            mockCoinGenerator = new Mock<ICoinGenerator>();
            mockGoodsGenerator = new Mock<IGoodsGenerator>();
            mockItemsGenerator = new Mock<IItemsGenerator>();
            mockTreasureAdjustmentSelector = new Mock<ITreasureRatesSelector>();
            mockPercentileSelector = new Mock<IPercentileSelector>();
            mockCollectionSelector = new Mock<ICollectionSelector>();
            mockItemSelector = new Mock<IItemSelector>();
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();

            encounterTreasureGenerator = new EncounterTreasureGenerator(mockTreasureAdjustmentSelector.Object, mockPercentileSelector.Object, mockCollectionSelector.Object, mockItemSelector.Object, mockJustInTimeFactory.Object, mockCoinGenerator.Object, mockGoodsGenerator.Object, mockItemsGenerator.Object);

            encounterCreature = new EncounterCreature();
            usesSubtypeForTreasure = new List<string>();
            creatures = new List<EncounterCreature>();
            treasureRates = new TreasureRatesSelection();
            goods = new List<Good>() { new Good(), new Good() };
            items = new List<Item>() { new Item(), new Item() };
            coin = new Coin { Currency = "currency", Quantity = 600 };

            level = 9266;
            encounterCreature.Creature.Name = "creature";
            encounterCreature.Creature.SubCreature = new Creature();
            encounterCreature.Creature.SubCreature.Name = "subtype";
            encounterCreature.Quantity = 1;
            creatures.Add(encounterCreature);
            treasureRates.Coin = 1;
            treasureRates.Goods = 1;
            treasureRates.Items = 1;

            mockPercentileSelector.Setup(s => s.SelectFrom(It.IsAny<double>())).Returns((double d) => d < 1);
            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.Name)).Returns(treasureRates);

            mockCoinGenerator.Setup(g => g.GenerateAtLevel(level)).Returns(coin);
            mockGoodsGenerator.Setup(g => g.GenerateAtLevel(level)).Returns(goods);
            mockItemsGenerator.Setup(g => g.GenerateRandomAtLevel(level)).Returns(items);

            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, GroupConstants.UseSubcreatureForTreasure))
                .Returns(usesSubtypeForTreasure);
        }

        [Test]
        public void GetTreasures()
        {
            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetTreasuresForMultipleCreatures()
        {
            var otherCreature = new EncounterCreature();
            otherCreature.Creature.Name = "other creature";
            otherCreature.Quantity = 90210;
            creatures.Add(otherCreature);

            var otherTreasureRates = new TreasureRatesSelection();
            otherTreasureRates.Coin = 1;
            otherTreasureRates.Goods = 1;
            otherTreasureRates.Items = 1;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(otherCreature.Creature.Name)).Returns(otherTreasureRates);

            var otherGoods = new List<Good>() { new Good(), new Good() };
            var otherItems = new List<Item>() { new Item(), new Item() };
            var otherCoin = new Coin { Currency = "other currency", Quantity = 42 };

            mockCoinGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(coin).Returns(otherCoin);
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(otherGoods);
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(otherItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);
            Assert.That(treasures.Count, Is.EqualTo(2));

            var firstTreasure = treasures.First();
            Assert.That(firstTreasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(firstTreasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(firstTreasure.Goods, Is.SupersetOf(goods));
            Assert.That(firstTreasure.Goods.Count, Is.EqualTo(2));
            Assert.That(firstTreasure.Items, Is.SupersetOf(items));
            Assert.That(firstTreasure.Items.Count, Is.EqualTo(2));

            var lastTreasure = treasures.Last();
            Assert.That(lastTreasure.Coin.Currency, Is.EqualTo("other currency"));
            Assert.That(lastTreasure.Coin.Quantity, Is.EqualTo(42));
            Assert.That(lastTreasure.Goods, Is.SupersetOf(otherGoods));
            Assert.That(lastTreasure.Goods.Count, Is.EqualTo(2));
            Assert.That(lastTreasure.Items, Is.SupersetOf(otherItems));
            Assert.That(lastTreasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void DoNotGetEmptyTreasures()
        {
            var otherCreature = new EncounterCreature();
            otherCreature.Creature.Name = "other creature";
            otherCreature.Quantity = 90210;
            creatures.Add(otherCreature);

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(otherCreature.Creature.Name)).Returns(treasureRates);

            var otherGoods = new List<Good>();
            var otherItems = new List<Item>();
            var otherCoin = new Coin { Currency = "other currency", Quantity = 0 };

            mockCoinGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(coin).Returns(otherCoin);
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(otherGoods);
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(otherItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetDifferentQuantitiesOfTreasure()
        {
            treasureRates.Coin = 0;
            treasureRates.Goods = 2;
            treasureRates.Items = 1;

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(0));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetMoreTreasure()
        {
            treasureRates.Coin = 2;
            treasureRates.Goods = 2;
            treasureRates.Items = 2;

            mockCoinGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(coin)
                .Returns(new Coin { Currency = "wrong currency", Quantity = 666 });

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetNoTreasureByChance()
        {
            treasureRates.Coin = 2;
            treasureRates.Goods = 2;
            treasureRates.Items = 2;

            mockCoinGenerator.SetupSequence(g => g.GenerateAtLevel(level))
                .Returns(coin)
                .Returns(new Coin { Currency = "wrong currency", Quantity = 666 });

            goods.Clear();
            items.Clear();
            coin.Quantity = 0;

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Empty);
        }

        [Test]
        public void GetNoTreasureIntentionally()
        {
            treasureRates.Coin = 0;
            treasureRates.Goods = 0;
            treasureRates.Items = 0;

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Empty);
        }

        [Test]
        public void GetPartialTreasureWithGoodsAndItems()
        {
            treasureRates.Coin = .5;
            treasureRates.Goods = .5;
            treasureRates.Items = .5;

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(300));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetDifferentPartialTreasureWithGoodsAndItems()
        {
            treasureRates.Coin = .1;
            treasureRates.Goods = .2;
            treasureRates.Items = .3;

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(60));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPartialTreasureWithGoods()
        {
            treasureRates.Coin = .1;
            treasureRates.Goods = .2;
            treasureRates.Items = .3;

            mockPercentileSelector.Setup(s => s.SelectFrom(.7)).Returns(false);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(60));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.Empty);
        }

        [Test]
        public void GetPartialTreasureWithItems()
        {
            treasureRates.Coin = .1;
            treasureRates.Goods = .2;
            treasureRates.Items = .3;

            mockPercentileSelector.Setup(s => s.SelectFrom(.8)).Returns(false);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(60));
            Assert.That(treasure.Goods, Is.Empty);
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPartialTreasureWithNoGoodsOrItems()
        {
            treasureRates.Coin = .1;
            treasureRates.Goods = .2;
            treasureRates.Items = .3;

            mockPercentileSelector.Setup(s => s.SelectFrom(.8)).Returns(false);
            mockPercentileSelector.Setup(s => s.SelectFrom(.7)).Returns(false);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(60));
            Assert.That(treasure.Goods, Is.Empty);
            Assert.That(treasure.Items, Is.Empty);
        }

        [Test]
        public void GetMoreAndPartialTreasure()
        {
            treasureRates.Coin = 2.1;
            treasureRates.Goods = 2.2;
            treasureRates.Items = 2.3;

            var secondGoods = new[] { new Good(), new Good() };
            var thirdGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods).Returns(thirdGoods);

            var secondItems = new[] { new Item(), new Item() };
            var thirdItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems).Returns(thirdItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1260));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods, Is.SupersetOf(thirdGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(6));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items, Is.SupersetOf(thirdItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(6));
        }

        [Test]
        public void GetMoreAndNoPartialTreasure()
        {
            treasureRates.Coin = 2.1;
            treasureRates.Goods = 2.2;
            treasureRates.Items = 2.3;

            mockPercentileSelector.Setup(s => s.SelectFrom(It.Is<double>(p => p > 0))).Returns(false);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1260));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void UseSubtypeForTreasure()
        {
            usesSubtypeForTreasure.Add(encounterCreature.Creature.Name);

            var subtypeTreasureRates = new TreasureRatesSelection();
            subtypeTreasureRates.Coin = 2;
            subtypeTreasureRates.Goods = 2;
            subtypeTreasureRates.Items = 2;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.SubCreature.Name)).Returns(subtypeTreasureRates);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void UseSubtypeWithoutFurtherSubtypeForTreasure()
        {
            encounterCreature.Creature.SubCreature.SubCreature = new Creature();
            encounterCreature.Creature.SubCreature.SubCreature.Name = "further subtype";

            usesSubtypeForTreasure.Add(encounterCreature.Creature.Name);

            var subtypeTreasureRates = new TreasureRatesSelection();
            subtypeTreasureRates.Coin = 2;
            subtypeTreasureRates.Goods = 2;
            subtypeTreasureRates.Items = 2;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.SubCreature.Name)).Returns(subtypeTreasureRates);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void UseFurtherSubtypeForTreasure()
        {
            encounterCreature.Creature.SubCreature.SubCreature = new Creature();
            encounterCreature.Creature.SubCreature.SubCreature.Name = "further subtype";

            usesSubtypeForTreasure.Add(encounterCreature.Creature.Name);
            usesSubtypeForTreasure.Add(encounterCreature.Creature.SubCreature.Name);

            var subSubtypeTreasureRates = new TreasureRatesSelection();
            subSubtypeTreasureRates.Coin = 2;
            subSubtypeTreasureRates.Goods = 2;
            subSubtypeTreasureRates.Items = 2;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.SubCreature.SubCreature.Name)).Returns(subSubtypeTreasureRates);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetSetTreasure()
        {
            var setTreasureItems = new[] { "item 1", "item 2" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, encounterCreature.Creature.Name)).Returns(setTreasureItems);

            var template1 = new Item();
            var template2 = new Item();

            template1.ItemType = "item type";
            template2.ItemType = "other item type";

            mockItemSelector.Setup(s => s.SelectFrom("item 1")).Returns(template1);
            mockItemSelector.Setup(s => s.SelectFrom("item 2")).Returns(template2);

            var mockMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            var mockOtherMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("item type")).Returns(mockMundaneItemGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("other item type")).Returns(mockOtherMundaneItemGenerator.Object);

            var setItem = new Item();
            var otherSetItem = new Item();
            mockMundaneItemGenerator.Setup(g => g.Generate(template1, true)).Returns(setItem);
            mockOtherMundaneItemGenerator.Setup(g => g.Generate(template2, true)).Returns(otherSetItem);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Contains.Item(setItem));
            Assert.That(treasure.Items, Contains.Item(otherSetItem));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetSetMagicalTreasure()
        {
            var setTreasureItems = new[] { "item 1", "item 2" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, encounterCreature.Creature.Name)).Returns(setTreasureItems);

            var template1 = new Item();
            var template2 = new Item();

            template1.ItemType = "item type";
            template1.Magic.Bonus = 1;
            template2.ItemType = "other item type";
            template2.Magic.Bonus = 1;

            mockItemSelector.Setup(s => s.SelectFrom("item 1")).Returns(template1);
            mockItemSelector.Setup(s => s.SelectFrom("item 2")).Returns(template2);

            var mockMagicalItemGenerator = new Mock<MagicalItemGenerator>();
            var mockOtherMagicalItemGenerator = new Mock<MagicalItemGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<MagicalItemGenerator>("item type")).Returns(mockMagicalItemGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<MagicalItemGenerator>("other item type")).Returns(mockOtherMagicalItemGenerator.Object);

            var setItem = new Item();
            var otherSetItem = new Item();
            mockMagicalItemGenerator.Setup(g => g.Generate(template1, true)).Returns(setItem);
            mockOtherMagicalItemGenerator.Setup(g => g.Generate(template2, true)).Returns(otherSetItem);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Contains.Item(setItem));
            Assert.That(treasure.Items, Contains.Item(otherSetItem));
            Assert.That(treasure.Items.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetSetTreasurePerCreature()
        {
            var setTreasureItems = new[] { "item 1", "item 2" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, encounterCreature.Creature.Name)).Returns(setTreasureItems);

            var template1 = new Item();
            var template2 = new Item();

            template1.ItemType = "item type";
            template2.ItemType = "other item type";

            mockItemSelector.Setup(s => s.SelectFrom("item 1")).Returns(template1);
            mockItemSelector.Setup(s => s.SelectFrom("item 2")).Returns(template2);

            var mockMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            var mockOtherMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("item type")).Returns(mockMundaneItemGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("other item type")).Returns(mockOtherMundaneItemGenerator.Object);

            mockMundaneItemGenerator.Setup(g => g.Generate(template1, true)).Returns((Item t, bool a) => t.Clone());
            mockOtherMundaneItemGenerator.Setup(g => g.Generate(template2, true)).Returns((Item t, bool a) => t.Clone());

            encounterCreature.Quantity = 42;

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(600));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(2));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items.Count(i => i.ItemType == "item type"), Is.EqualTo(42));
            Assert.That(treasure.Items.Count(i => i.ItemType == "other item type"), Is.EqualTo(42));
            Assert.That(treasure.Items.Count, Is.EqualTo(86));
        }

        [Test]
        public void UseSubtypeForSetTreasure()
        {
            usesSubtypeForTreasure.Add(encounterCreature.Creature.Name);

            var subtypeTreasureRates = new TreasureRatesSelection();
            subtypeTreasureRates.Coin = 2;
            subtypeTreasureRates.Goods = 2;
            subtypeTreasureRates.Items = 2;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.SubCreature.Name)).Returns(subtypeTreasureRates);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var setTreasureItems = new[] { "item 1", "item 2" };
            mockCollectionSelector.Setup(s => s.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, encounterCreature.Creature.SubCreature.Name)).Returns(setTreasureItems);

            var template1 = new Item();
            var template2 = new Item();

            template1.ItemType = "item type";
            template2.ItemType = "other item type";

            mockItemSelector.Setup(s => s.SelectFrom("item 1")).Returns(template1);
            mockItemSelector.Setup(s => s.SelectFrom("item 2")).Returns(template2);

            var mockMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            var mockOtherMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("item type")).Returns(mockMundaneItemGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("other item type")).Returns(mockOtherMundaneItemGenerator.Object);

            var setItem = new Item();
            var otherSetItem = new Item();
            mockMundaneItemGenerator.Setup(g => g.Generate(template1, true)).Returns(setItem);
            mockOtherMundaneItemGenerator.Setup(g => g.Generate(template2, true)).Returns(otherSetItem);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items, Contains.Item(setItem));
            Assert.That(treasure.Items, Contains.Item(otherSetItem));
            Assert.That(treasure.Items.Count, Is.EqualTo(6));
        }

        [Test]
        public void UseFurtherSubtypeForSetTreasure()
        {
            encounterCreature.Creature.SubCreature.SubCreature = new Creature();
            encounterCreature.Creature.SubCreature.SubCreature.Name = "further subtype";

            usesSubtypeForTreasure.Add(encounterCreature.Creature.Name);
            usesSubtypeForTreasure.Add(encounterCreature.Creature.SubCreature.Name);

            var subSubtypeTreasureRates = new TreasureRatesSelection();
            subSubtypeTreasureRates.Coin = 2;
            subSubtypeTreasureRates.Goods = 2;
            subSubtypeTreasureRates.Items = 2;

            mockTreasureAdjustmentSelector.Setup(s => s.SelectFor(encounterCreature.Creature.SubCreature.SubCreature.Name)).Returns(subSubtypeTreasureRates);

            var secondGoods = new[] { new Good(), new Good() };
            mockGoodsGenerator.SetupSequence(g => g.GenerateAtLevel(level)).Returns(goods).Returns(secondGoods);

            var secondItems = new[] { new Item(), new Item() };
            mockItemsGenerator.SetupSequence(g => g.GenerateRandomAtLevel(level)).Returns(items).Returns(secondItems);

            var setTreasureItems = new[] { "item 1", "item 2" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, encounterCreature.Creature.SubCreature.SubCreature.Name))
                .Returns(setTreasureItems);

            var template1 = new Item();
            var template2 = new Item();

            template1.ItemType = "item type";
            template2.ItemType = "other item type";

            mockItemSelector.Setup(s => s.SelectFrom("item 1")).Returns(template1);
            mockItemSelector.Setup(s => s.SelectFrom("item 2")).Returns(template2);

            var mockMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            var mockOtherMundaneItemGenerator = new Mock<MundaneItemGenerator>();
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("item type")).Returns(mockMundaneItemGenerator.Object);
            mockJustInTimeFactory.Setup(f => f.Build<MundaneItemGenerator>("other item type")).Returns(mockOtherMundaneItemGenerator.Object);

            var setItem = new Item();
            var otherSetItem = new Item();
            mockMundaneItemGenerator.Setup(g => g.Generate(template1, true)).Returns(setItem);
            mockOtherMundaneItemGenerator.Setup(g => g.Generate(template2, true)).Returns(otherSetItem);

            var treasures = encounterTreasureGenerator.GenerateFor(creatures, level);
            Assert.That(treasures, Is.Not.Null);
            Assert.That(treasures, Is.Not.Empty);

            var treasure = treasures.Single();
            Assert.That(treasure.Coin.Currency, Is.EqualTo("currency"));
            Assert.That(treasure.Coin.Quantity, Is.EqualTo(1200));
            Assert.That(treasure.Goods, Is.SupersetOf(goods));
            Assert.That(treasure.Goods, Is.SupersetOf(secondGoods));
            Assert.That(treasure.Goods.Count, Is.EqualTo(4));
            Assert.That(treasure.Items, Is.SupersetOf(items));
            Assert.That(treasure.Items, Is.SupersetOf(secondItems));
            Assert.That(treasure.Items, Contains.Item(setItem));
            Assert.That(treasure.Items, Contains.Item(otherSetItem));
            Assert.That(treasure.Items.Count, Is.EqualTo(6));
        }
    }
}
