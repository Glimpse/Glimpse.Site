using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Build
{ 
    public interface ISettings
    {
        IStatusQueryProvider StatusQueryProvider { get; }

        void Initialize();
    }
}
