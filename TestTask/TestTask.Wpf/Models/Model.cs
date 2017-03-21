using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Wpf.Entities;

namespace TestTask.Wpf.Models
{
    public class Model : IModel
    {
        private ICollection<MeteringRecord> records;
        private ICollection<Possibility> possibilitys;

        public Model()
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
        }

        public Model(ICollection<MeteringRecord> records, ICollection<Possibility> possibilitys)
        {
            this.records = records;
            this.possibilitys = possibilitys;
        }

        public ICollection<MeteringRecord> GetAllRecords()
        {
            return records.ToList();
        }

        public ICollection<MeteringRecord> GetWithoutDate()
        {
            return records.Where(x => !x.Date.HasValue).ToList();
        }

        public ESetDateResult SetDate(int? number, DateTime? date, string city)
        {
            if(!number.HasValue)
            {
                return ESetDateResult.NumberIsNull;
            }
            if (!date.HasValue)
            {
                return ESetDateResult.DateIsNull;
            }

            var record = records.FirstOrDefault(x => x.Number == number.Value);

            if(record == null)
            {
                return ESetDateResult.NotFoundRecord;
            }

            if(record.Date.HasValue)
            {
                return ESetDateResult.RecordHasDate;
            }

            var possibility = possibilitys.FirstOrDefault(x => x.City == city && x.Date.Date == date.Value.Date);

            if(possibility == null)
            {
                return ESetDateResult.NotFoundPossibility;
            }
            if(possibility.Limit <= 0)
            {
                return ESetDateResult.LimitIsOver;
            }

            record.Date = date.Value;
            possibility.Limit -= 1;

            return ESetDateResult.Ok;
        }

        public ICollection<Possibility> GetAllPossibilitys()
        {
            return possibilitys.ToList();
        }

        public ICollection<Possibility> GetByCity(string city)
        {
            return possibilitys.Where(x => x.City == city).ToList();
        }

        public ICollection<MeteringRecord> GetByUserPartName(string partName)
        {
            return records.Where(x => x.FullName.Contains(partName)).ToList();
        }
    }
}
