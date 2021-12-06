using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDI.Options
{
    public class CommonWebOptions: IBaseOptions
    {
        public const string Position = "CommonWeb";

        public string Token { get; set; }
        public string BaseUrl { get; set; }
    }
}
