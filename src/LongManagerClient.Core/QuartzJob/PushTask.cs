using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LongManagerClient.Core.QuartzJob
{
    public class PushTask
    {
        public async Task Run()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();
            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<PushJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run on the next round minute
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await sched.ScheduleJob(job, trigger);

            // Start up the scheduler (nothing can actually run until the
            // scheduler has been started)
            await sched.Start();
        }
    }
}
