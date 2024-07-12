using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.ApiResult
{
    public class EshopResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
        public string Nowpage { get; set; }
        public int Allpage { get; set; }
        public int Count { get; set; }
    }
}
