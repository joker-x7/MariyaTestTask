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
        public ICollection<MeteringRecord> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<MeteringRecord> GetWithoutDate()
        {
            throw new NotImplementedException();
        }
    }
}
