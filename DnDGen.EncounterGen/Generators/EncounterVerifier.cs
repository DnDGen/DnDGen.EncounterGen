using DnDGen.EncounterGen.Models;
using DnDGen.EncounterGen.Selectors;
using DnDGen.EncounterGen.Selectors.Collections;
using DnDGen.EncounterGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.EncounterGen.Generators
{
    internal class EncounterVerifier : IEncounterVerifier
    {
        private readonly ICollectionSelector collectionSelector;
        private readonly IEncounterCollectionSelector encounterCollectionSelector;
        private readonly IEncounterLevelSelector amountSelector;

        public EncounterVerifier(ICollectionSelector collectionSelector, IEncounterCollectionSelector encounterCollectionSelector, IEncounterLevelSelector amountSelector)
        {
            this.collectionSelector = collectionSelector;
            this.encounterCollectionSelector = encounterCollectionSelector;
            this.amountSelector = amountSelector;
        }

        public bool ValidEncounterExists(EncounterSpecifications encounterSpecifications)
        {
            if (!encounterSpecifications.IsValid())
                return false;

            var exists = ValidEncounterExists(
                encounterSpecifications.Environment,
                encounterSpecifications.Temperature,
                encounterSpecifications.TimeOfDay,
                encounterSpecifications.Level,
                encounterSpecifications.AllowAquatic,
                encounterSpecifications.AllowUnderground,
                encounterSpecifications.CreatureTypeFilters.ToArray());

            return exists;
        }

        public bool EncounterIsValid(Encounter encounter, IEnumerable<string> creatureTypeFilters)
        {
            var creatureNames = encounter.Creatures.Select(c => c.Creature.Name);
            var level = amountSelector.Select(encounter);

            return EncounterSpecifications.LevelIsValid(level) && EncounterIsValid(creatureNames, creatureTypeFilters);
        }

        public bool EncounterIsValid(IEnumerable<string> creatureNames, IEnumerable<string> creatureTypeFilters)
        {
            return creatureNames.Any(c => CreatureIsValid(c, creatureTypeFilters));
        }

        public bool CreatureIsValid(string creatureName, IEnumerable<string> creatureTypeFilters)
        {
            if (!creatureTypeFilters.Any())
                return true;

            var allowedCreatureNames = GetAllowedCreatureNames(creatureTypeFilters);

            return allowedCreatureNames.Contains(creatureName);
        }

        private IEnumerable<string> GetAllowedCreatureNames(IEnumerable<string> creatureTypeFilters)
        {
            var creatureNames = new List<string>();

            foreach (var filter in creatureTypeFilters)
            {
                var filterCreatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.CreatureGroups, filter);
                creatureNames.AddRange(filterCreatures);
            }

            return creatureNames.Distinct();
        }

        public bool ValidEncounterExists(
            string environment = "",
            string temperature = "",
            string timeOfDay = "",
            int level = 0,
            bool allowAquatic = true,
            bool allowUnderground = true,
            params string[] filters)
        {
            var possibleEncounters = encounterCollectionSelector.SelectPossibleEncountersFrom(
                environment,
                temperature,
                timeOfDay,
                level,
                allowAquatic,
                allowUnderground,
                filters);
            return possibleEncounters.Any();
        }

        public IEnumerable<string> GetValidTemperatues(string environment)
        {
            return encounterCollectionSelector.SelectPossibleTemperatures(environment);
        }

        public IEnumerable<string> GetValidTimesOfDay(string environment, string temperature)
        {
            return encounterCollectionSelector.SelectPossibleTimesOfDay(environment, temperature);
        }

        public IEnumerable<int> GetValidLevels(string environment, string temperature, string timeOfDay)
        {
            return encounterCollectionSelector.SelectPossibleLevels(environment, temperature, timeOfDay);
        }

        public IEnumerable<bool> GetValidAllowAquatic(string environment, string temperature, string timeOfDay, int level)
        {
            return encounterCollectionSelector.SelectPossibleAllowAquatic(environment, temperature, timeOfDay, level);
        }

        public IEnumerable<bool> GetValidAllowUnderground(string environment, string temperature, string timeOfDay, int level, bool allowAquatic)
        {
            return encounterCollectionSelector.SelectPossibleAllowUnderground(environment, temperature, timeOfDay, level, allowAquatic);
        }

        public IEnumerable<string> GetValidFilters(string environment, string temperature, string timeOfDay, int level, bool allowAquatic, bool allowUnderground)
        {
            return encounterCollectionSelector.SelectPossibleFilters(environment, temperature, timeOfDay, level, allowAquatic, allowUnderground);
        }
    }
}
