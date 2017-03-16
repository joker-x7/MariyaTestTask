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
        ICollection<MeteringRecord> GetAll();
        ICollection<MeteringRecord> GetWithoutDate();
    }
}
