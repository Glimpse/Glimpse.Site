using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Twitter
{ 
    public interface ISettings
    {
        ITweetQueryProvider TweetQueryProvider { get; }

        void Initialize();
    }
}
