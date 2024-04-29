using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Shared.Extensions;

namespace WzFrame.Shared.Job
{
    public class HelloJob : IJobBase
    {
        public Task Execute(IJobExecutionContext context)
        {
            var t = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .Build();

            var st = t.ToJson();
            Console.WriteLine(st);
            return Task.CompletedTask;
        }

        public List<ITrigger>? GetTriggers()
        {

            return null;
        }
    }
}
