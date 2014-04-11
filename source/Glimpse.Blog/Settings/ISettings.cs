using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Blog
{ 
    public interface ISettings
    {
        IPostQueryProvider PostQueryProvider { get; }

        void Initialize();
    }
}
