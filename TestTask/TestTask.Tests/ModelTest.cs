using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestTask.Wpf.Entities;
using TestTask.Wpf.Models;

namespace TestTask.Tests
{
    [TestClass]
    public class ModelTest
    {
        private IModel model;
        private ICollection<MeteringRecord> records;
        private ICollection<Possibility> possibilitys;

        [TestInitialize]
        public void Initialize()
        {
            records = new List<MeteringRecord>();
            possibilitys = new List<Possibility>();

            for (int i = 1; i <= 10; i++)
            {
                records.Add(new MeteringRecord()
                {
                    Number = i,
                    FullName = "Иванов Иван Иванович-" + i
                });
                possibilitys.Add(new Possibility()
                {
                    City = "Саратов",
                    Date = DateTime.Now.AddDays(i),
                    Limit = 3
                });
                possibilitys.Add(new Possibility()
                {
                    City = "Москва",
                    Date = DateTime.Now.AddDays(i),
                    Limit = 3 + i
                });
            }
            model = new Model(records, possibilitys);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var recordsResult = model.GetAllRecords();
            var possibilitysResult = model.GetAllPossibilitys();

            Assert.IsTrue(records.SequenceEqual(recordsResult));
            Assert.IsTrue(possibilitys.SequenceEqual(possibilitysResult));
        }

        [TestMethod]
        public void SetDatePositivTest()
        {
            var result = model.SetDate(1, DateTime.Now.AddDays(1), "Саратов");

            Assert.AreEqual(ESetDateResult.Ok, result);
        }

        [TestMethod]
        public void SetDateNegativTest()
        {
            var numberIsNullResult = model.SetDate(null, DateTime.Now.AddDays(1), "Саратов");
            var dateIsNullResult = model.SetDate(1, null, "Саратов");
            var notFoundRecordResult = model.SetDate(100, DateTime.Now.AddDays(1), "Саратов");
            var notFoundPossibilitysResult = model.SetDate(1, DateTime.Now.AddDays(100), "Саратов");
            model.SetDate(1, DateTime.Now.AddDays(1), "Саратов");
            model.SetDate(2, DateTime.Now.AddDays(1), "Саратов");
            model.SetDate(3, DateTime.Now.AddDays(1), "Саратов");
            var limitIsOverResult = model.SetDate(4, DateTime.Now.AddDays(1), "Саратов");
            var recordHasDateResult = model.SetDate(1, DateTime.Now.AddDays(1), "Саратов");

            Assert.AreEqual(ESetDateResult.NumberIsNull, numberIsNullResult);
            Assert.AreEqual(ESetDateResult.DateIsNull, dateIsNullResult);
            Assert.AreEqual(ESetDateResult.NotFoundRecord, notFoundRecordResult);
            Assert.AreEqual(ESetDateResult.NotFoundPossibility, notFoundPossibilitysResult);
            Assert.AreEqual(ESetDateResult.LimitIsOver, limitIsOverResult);
            Assert.AreEqual(ESetDateResult.RecordHasDate, recordHasDateResult);
        }

        [TestMethod]
        public void GetWithoutDateTest()
        {
            var result = model.GetWithoutDate();

            Assert.IsTrue(result.SequenceEqual(records.Where(x=>!x.Date.HasValue)));
        }

        [TestMethod]
        public void GetByCityTest()
        {
            var result = model.GetByCity("Саратов");

            Assert.IsTrue(result.SequenceEqual(possibilitys.Where(x => x.City == "Саратов")));
        }

        [TestMethod]
        public void GetByUserPartNameTest()
        {
            var name = "Иванов Иван Иванович-1";
            var result = model.GetByUserPartName(name);

            Assert.IsTrue(result.SequenceEqual(records.Where(x => x.FullName.Contains(name))));
        }
    }
}
