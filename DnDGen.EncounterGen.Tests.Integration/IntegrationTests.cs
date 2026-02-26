using DnDGen.EncounterGen.IoC;
using DnDGen.EncounterGen.Models;
using Ninject;
using NUnit.Framework;
using System;
using System.Linq;

namespace DnDGen.EncounterGen.Tests.Integration
{
    [TestFixture]
    public abstract class IntegrationTests
    {
        protected IKernel kernel;

        [OneTimeSetUp]
        public void IntegrationTestsFixtureSetup()
        {
            kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });

            var encounterGenLoader = new EncounterGenModuleLoader();
            encounterGenLoader.LoadModules(kernel);
        }

        protected T GetNewInstanceOf<T>()
        {
            return kernel.Get<T>();
        }

        /// <summary>
        /// As a base limit, we want general encounters to generate in less than a second
        /// Each character can take up to 1 second to generate, so we will uadd that to our limit
        /// Furthermore, high-level encounters may take longer to generate due to increased treasure amounts,
        /// so we will add an additional buffer based on the encounter level
        /// </summary>
        /// <param name="encounter"></param>
        /// <returns></returns>
        protected static double GetTimeLimitInSeconds(Encounter encounter) => encounter.Characters.Count() + Math.Ceiling(encounter.ActualEncounterLevel / 10d);
    }
}
