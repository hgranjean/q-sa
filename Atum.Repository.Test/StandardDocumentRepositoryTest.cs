using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Atum.Repository.Surveillance;
using Atum.Database.Surveillance.Models;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Atum.Repository.Test
{
    [TestClass]
    public class StandardDocumentRepositoryTest
    {
        private IRepository<StandardDocument> _standardDocumentRepository;
        private DbContext _ctx = new AtumSurveillanceContext();

    [TestInitialize]
    public void TestInitialize()
    {
        _ctx = new AtumSurveillanceContext();
        _standardDocumentRepository = new StandardDocumentRepository(_ctx);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _standardDocumentRepository = null;
        _ctx = null;
    }


    [TestMethod]
    public void TestMethodAddStandardDocument()
    {
        //
        var standardDocument = new StandardDocument { Title = "test_standardDocument" };
        _standardDocumentRepository.Add(standardDocument);

        Assert.IsTrue(standardDocument.Id > 0 );
        }


       [TestMethod]
        public void TestMethodFindStandardDocument()
        {
           int Id = 2;
           var documentById =_standardDocumentRepository.FindById(Id);

           Assert.IsNotNull(documentById);

           Guid guid = new Guid();
           var documentByGuid = _standardDocumentRepository.FindByGuid(guid);

           Assert.IsNotNull(documentByGuid);
        }

       [TestMethod]
       public void TestMethodFindMatchingStandardDocument()
       {
           //Expression criteria = new Expression<this.ab,bool>();

           //_standardDocumentRepository.FindMatching(criteria);           
       }

       [TestMethod]
       public void TestMethodDeleteStandardDocument()
       {
           //Find and Delete
           StandardDocument documentToDelete = _standardDocumentRepository.FindByGuid(new Guid());
           _standardDocumentRepository.Delete(documentToDelete);
           
       }
    }
}
