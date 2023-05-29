using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Primogit
{
    public class OrarioUfficio
    {
        public int Id { get; set; }
        public DayOfWeek GiornoSettimana { get; set; }
        public TimeSpan Ora_Da { get; set; }
        public TimeSpan Ora_A { get; set; }
        public double TotaleOre { get; set; }

        internal static object Where(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
