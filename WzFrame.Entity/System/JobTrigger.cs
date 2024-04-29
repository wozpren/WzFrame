using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.System
{
    [SugarTable("JobTrigger", "任务调度")]
    public class JobTrigger : EntityUserTimeBase
    {
        public bool IsEnabled { get; set; } = true;
        public bool StartOnStartup { get; set; }
        public string TriggerName { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string? Cron { get; set; }

        [SugarColumn(ColumnDescription = "任务执行时间")]
        public DateTime? RunTime { get; set; }

        [SugarColumn(ColumnDescription = "任务执行间隔")]
        public TimeSpan? Interval { get; set; }

        [SugarColumn(ColumnDescription = "任务执行次数, 0为无限次")]
        public int RepeatCount { get; set; }

        public string JobData { get; set; } = string.Empty;




    }
}
