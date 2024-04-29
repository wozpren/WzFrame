using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.System
{
    [SugarTable("JobTrigger", "�������")]
    public class JobTrigger : EntityUserTimeBase
    {
        public bool IsEnabled { get; set; } = true;
        public bool StartOnStartup { get; set; }
        public string TriggerName { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string? Cron { get; set; }

        [SugarColumn(ColumnDescription = "����ִ��ʱ��")]
        public DateTime? RunTime { get; set; }

        [SugarColumn(ColumnDescription = "����ִ�м��")]
        public TimeSpan? Interval { get; set; }

        [SugarColumn(ColumnDescription = "����ִ�д���, 0Ϊ���޴�")]
        public int RepeatCount { get; set; }

        public string JobData { get; set; } = string.Empty;




    }
}
