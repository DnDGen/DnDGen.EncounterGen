using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Selectors.Collections;
using DnDGen.EncounterGen.Selectors.Selections;
using DnDGen.EncounterGen.Tables;
using DnDGen.Infrastructure.Factories;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.Infrastructure.Selectors.Percentiles;
using DnDGen.TreasureGen;
using DnDGen.TreasureGen.Coins;
using DnDGen.TreasureGen.Goods;
using DnDGen.TreasureGen.Items;
using DnDGen.TreasureGen.Items.Magical;
using DnDGen.TreasureGen.Items.Mundane;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Generators
{
    internal class EncounterTreasureGenerator : IEncounterTreasureGenerator
    {
        private readonly ITreasureRatesSelector treasureAdjustmentSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly ICollectionSelector collectionSelector;
        private readonly IItemSelector itemSelector;
        private readonly JustInTimeFactory justInTimeFactory;
        private readonly ICoinGenerator coinGenerator;
        private readonly IGoodsGenerator goodsGenerator;
        private readonly IItemsGenerator itemsGenerator;

        public EncounterTreasureGenerator(ITreasureRatesSelector treasureAdjustmentSelector, IPercentileSelector percentileSelector, ICollectionSelector collectionSelector, IItemSelector itemSelector, JustInTimeFactory justInTimeFactory, ICoinGenerator coinGenerator, IGoodsGenerator goodsGenerator, IItemsGenerator itemsGenerator)
        {
            this.treasureAdjustmentSelector = treasureAdjustmentSelector;
            this.percentileSelector = percentileSelector;
            this.collectionSelector = collectionSelector;
            this.itemSelector = itemSelector;
            this.justInTimeFactory = justInTimeFactory;
            this.coinGenerator = coinGenerator;
            this.goodsGenerator = goodsGenerator;
            this.itemsGenerator = itemsGenerator;
        }

        public IEnumerable<Treasure> GenerateFor(IEnumerable<EncounterCreature> creatures, int level)
        {
            var treasures = new List<Treasure>();

            foreach (var creature in creatures)
            {
                var treasure = GenerateFor(creature, level);

                if (treasure.IsAny)
                    treasures.Add(treasure);
            }

            return treasures;
        }

        private Treasure GenerateFor(EncounterCreature creature, int level)
        {
            var creatureName = GetCreatureNameForTreasure(creature.Creature);
            var treasureRates = treasureAdjustmentSelector.SelectFor(creatureName);

            var treasure = new Treasure();
            treasure.Coin = GetCoin(treasureRates, level);
            treasure.Goods = GetGoods(treasureRates, level);
            treasure.Items = GetItems(treasureRates, level, creatureName, creature.Quantity);

            return treasure;
        }

        private Coin GetCoin(TreasureRatesSelection treasureRates, int level)
        {
            var coin = coinGenerator.GenerateAtLevel(level);

            var rawQuantity = treasureRates.Coin * coin.Quantity;
            coin.Quantity = Convert.ToInt32(rawQuantity);

            return coin;
        }

        private IEnumerable<Good> GetGoods(TreasureRatesSelection treasureRates, int level)
        {
            var goods = new List<Good>();

            while (treasureRates.Goods > 0)
            {
                //INFO: The rate here might be 25%, meaning only 25% of creatures should have treasure.
                //The percentile selector wants a threshold, where above is true. So 25% likelihood would translate to threshold of 75%
                var threshold = 1 - treasureRates.Goods;
                var hasGoods = percentileSelector.SelectFrom(threshold);

                if (hasGoods)
                {
                    var generatedGoods = goodsGenerator.GenerateAtLevel(level);
                    goods.AddRange(generatedGoods);
                }

                treasureRates.Goods--;
            }

            return goods;
        }

        private IEnumerable<Item> GetItems(TreasureRatesSelection treasureRates, int level, string creatureName, int quantity)
        {
            var items = new List<Item>();

            while (treasureRates.Items > 0)
            {
                //INFO: The rate here might be 25%, meaning only 25% of creatures should have treasure.
                //The percentile selector wants a threshold, where above is true. So 25% likelihood would translate to threshold of 75%
                var threshold = 1 - treasureRates.Items;
                var hasItems = percentileSelector.SelectFrom(threshold);

                if (hasItems)
                {
                    var generatedItems = itemsGenerator.GenerateRandomAtLevel(level);
                    items.AddRange(generatedItems);
                }

                treasureRates.Items--;
            }

            var setItems = GetSetItems(creatureName, quantity);
            items.AddRange(setItems);

            return items;
        }

        private IEnumerable<Item> GetSetItems(string creatureName, int quantity)
        {
            var setItems = new List<Item>();
            var setTreasure = collectionSelector.SelectFrom(Config.Name, TableNameConstants.TreasureGroups, creatureName);
            var setItemTemplates = GetTemplates(setTreasure);

            while (quantity-- > 0)
            {
                foreach (var template in setItemTemplates)
                {
                    var item = GetItemFrom(template);
                    setItems.Add(item);
                }
            }

            return setItems;
        }

        private Item GetItemFrom(Item template)
        {
            if (template.IsMagical)
            {
                var magicalGenerator = justInTimeFactory.Build<MagicalItemGenerator>(template.ItemType);
                return magicalGenerator.Generate(template, true);
            }

            var mundaneGenerator = justInTimeFactory.Build<MundaneItemGenerator>(template.ItemType);
            return mundaneGenerator.Generate(template, true);
        }

        private IEnumerable<Item> GetTemplates(IEnumerable<string> setTreasure)
        {
            var templates = new List<Item>();

            foreach (var setItemTemplate in setTreasure)
            {
                var template = itemSelector.SelectFrom(setItemTemplate);
                templates.Add(template);
            }

            return templates;
        }

        private string GetCreatureNameForTreasure(Creature creature)
        {
            var useSubcreatureForTreasure = collectionSelector.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, GroupConstants.UseSubcreatureForTreasure);

            if (useSubcreatureForTreasure.Contains(creature.Name))
            {
                return GetCreatureNameForTreasure(creature.SubCreature);
            }

            return creature.Name;
        }
    }
}
