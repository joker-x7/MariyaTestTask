using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Wpf.Entities
{
    public enum ESetDateResult
    {
        Ok,
        DateIsNull,
        NumberIsNull,
        NotFoundRecord,
        NotFoundPossibility,
        LimitIsOver,
        RecordHasDate,
    }
}
