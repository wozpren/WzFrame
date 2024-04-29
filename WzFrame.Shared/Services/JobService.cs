using Masuit.Tools.Core.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzFrame.Shared.Job;

namespace WzFrame.Shared.Services
{
    [ServiceInject(ServiceLifetime.Singleton)]
    public class JobService
    {
        private readonly ISchedulerFactory schedulerFactory;

        private IScheduler? mainScheduler;

        public JobService(ISchedulerFactory schedulerFactory)
        {
            this.schedulerFactory = schedulerFactory;
        }

        public async Task Test()
        {
            if (mainScheduler == null)
            {
                mainScheduler = await schedulerFactory.GetScheduler();
            }

            IJobDetail job = JobBuilder.Create(typeof(HelloJob))
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .Build();

            await mainScheduler.ScheduleJob(job, trigger);


            await mainScheduler.Start();
        }



    }
}
