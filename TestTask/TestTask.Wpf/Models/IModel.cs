using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Wpf.Entities;

namespace TestTask.Wpf.Models
{
    public interface IModel
    {
        ICollection<MeteringRecord> GetAllRecords();
        ICollection<MeteringRecord> GetWithoutDate();
        ESetDateResult SetDate(int? number, DateTime? date, string city);
        ICollection<Possibility> GetAllPossibilitys();
        ICollection<Possibility> GetByCity(string city);
    }
}
