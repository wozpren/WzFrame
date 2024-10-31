using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity
{
    [SugarTable("License")]
    public class LicenseModel : EntityBase
    {
        public string Iemi { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
        public bool IsVaild { get; set; } = false;

    }
}
