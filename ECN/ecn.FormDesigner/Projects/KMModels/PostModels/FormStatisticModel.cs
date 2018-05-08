using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.PostModels
{
    public class FormStatisticModel: PostModelBase
    {
        [GetFromField("FormStatistic_Seq_ID")]
        public long FormStatistic_ID { get; set; }
        public DateTime StartForm { get; set; }
        public DateTime FinishForm { get; set; }
        public int TotalPages { get; set; }
        public string Email { get; set; }
        public bool IsSubmitted { get; set; }
        public string Start;
        public string Finish;
        
        public string Duration;
        public override void FillData(object entity)
        {
            base.FillData(entity);
            if (Email == null)
                Email = "Unknown";
            
            if (StartForm == DateTime.MinValue)
                Start = "Unknown";
            else
                Start = StartForm.ToString();

            if (FinishForm == DateTime.MinValue)
                Finish = "Unknown";
            else
                Finish = FinishForm.ToString();

            if (FinishForm != DateTime.MinValue && StartForm != DateTime.MinValue)
            {
                TimeSpan DurationTimeSpan = (FinishForm - StartForm);
                string day = "", hour, min, sec, msec, strFormat = "{0}h {1}m {2}sec {3}msec";
                if (DurationTimeSpan.TotalHours > 24)
                {
                    day = DurationTimeSpan.Days.ToString();
                    strFormat = "{4}day {0}h {1}m {2}sec {3}msec";
                }
                hour = DurationTimeSpan.Hours.ToString();
                min = DurationTimeSpan.Minutes.ToString();
                sec = DurationTimeSpan.Seconds.ToString();
                msec = DurationTimeSpan.Milliseconds.ToString();
                Duration = String.Format(strFormat, hour, min, sec, msec, day);
            }
            else {                
                Duration = "Unknown"; 
            }

            if (!IsSubmitted)
            {
                Finish = "Unknown";
                TotalPages = 0;
            }
        }
    }
}
