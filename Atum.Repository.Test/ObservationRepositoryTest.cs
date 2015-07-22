using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.QualityManagement.Auditing;
using Atum.Repository.Surveillance;
using Atum.Database.Surveillance.Models;
using System.Data.Entity;

namespace Atum.Repository.Test
{
    [TestClass]
    public class ObservationRepositoryTest
    {
        private IRepository<Observation> _observationRepository;
        private DbContext _ctx = new AtumSurveillanceContext();

        [TestInitialize]
        public void TestInitialize()
        {
            _ctx = new AtumSurveillanceContext();
            _observationRepository = new ObservationRepository(_ctx);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _observationRepository = null;
            _ctx = null;
        }

        //StandarDocument Root Aggregate
        [TestMethod]
        public void TestMethodAddObservation()
        {
       
            //Create Observation
            string remark = "";
            string standardKey = "";
            string location = "";

            Observation observation = new Observation(new Domain.Common.Person("Hervé", "L", "Granjean"), remark, standardKey);
            

        
        }

    }
}
