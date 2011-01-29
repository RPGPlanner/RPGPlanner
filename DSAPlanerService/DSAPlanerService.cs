using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace DSAPlanerService
{
    public partial class DSAPlanerService : ServiceBase
    {
     //   const int INTERVAL_LENGTH = 86400000;   // 86.400.000 ms = 24 h
        System.Timers.Timer timer;

        public DSAPlanerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debug.WriteLine("OnStart");

            // DAL_DSAPlaner.MailManager.getMailManager().sendChangeMail(DateTime.Now);

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = false;
            timer.Interval = time2Tomorrow().TotalMilliseconds;
            timer.Enabled = true;
            timer.Start();

            Debug.WriteLine("OnStart End; Interval next: " + timer.Interval.ToString());
        }

        private static TimeSpan time2Tomorrow()
        {
            DateTime currentTime = DateTime.Now;
            DateTime tomorrowMorning = currentTime.Subtract(currentTime.TimeOfDay).AddHours(24);

            //    // Test
            //tomorrowMorning = currentTime.AddSeconds(10);
            //    // Test End

            return tomorrowMorning.AddMinutes(5).Subtract(currentTime);
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            System.Timers.Timer tmrSend = (System.Timers.Timer)sender;

            Debug.WriteLine("Tick; Interval next: " + tmrSend.Interval.ToString());

            DAL_DSAPlaner.MailManager.getMailManager().dailyMail(DateTime.Now.AddDays(-0.5));

            if (!tmrSend.AutoReset)
            {
                timer.Interval = time2Tomorrow().TotalMilliseconds;
                tmrSend.Enabled = true;
                tmrSend.AutoReset = true;
                tmrSend.Start();
            }
        }

        protected override void OnStop()
        {
            Debug.WriteLine("OnStop; Interval next: " + timer.Interval.ToString());
        }
    }
}
