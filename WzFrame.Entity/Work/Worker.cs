using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Work
{
    [SugarTable("Worker", TableDescription = "工人表")]
    public class Worker : EntityUserTimeBase
    {
        [AutoGenerateColumn(Text = "姓名")]
        public string Name { get; set; } = string.Empty;

        [AutoGenerateColumn(Text = "头像",  Visible = false)]
        public string Avatar { get; set; } = string.Empty;

        [AutoGenerateColumn(Text = "身份证号")]
        public string Identity { get; set; } = string.Empty;
        [AutoGenerateColumn(Text = "操作机型")]
        public string Machine { get; set; } = string.Empty;

        [AutoGenerateColumn(Text = "指导员")]
        public string Trainee { get; set; } = string.Empty;

        [AutoGenerateColumn(Text = "培训时间")]
        public DateTime TrainTime { get; set; } = DateTime.Now;

        [AutoGenerateColumn(Text = "有效时间")]
        public DateTime ValidTime { get; set; } = DateTime.Now;

        [AutoGenerateColumn(Text = "所属", Ignore = true)]
        public string Belong { get; set; } = string.Empty;
    }
}
