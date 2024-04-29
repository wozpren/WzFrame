using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Job
{
    public interface IJobBase : IJob
    {
        List<ITrigger>? GetTriggers();

    }
}
