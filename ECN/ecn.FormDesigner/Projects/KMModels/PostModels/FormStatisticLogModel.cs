using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.PostModels
{
    public class FormStatisticLogModel : PostModelBase
    {
        public DateTime StartPage { get; set; }
        public DateTime FinishPage { get; set; }
        public int PageNumber { get; set; }
        
        public string Start;
        public string Finish;
        public string Duration;
        public override void FillData(object entity)
        {
            base.FillData(entity);

            Start = StartPage.ToString();
            if (FinishPage == DateTime.MinValue)
                Finish = "Unknown";
            else
                Finish = FinishPage.ToString();

            if (FinishPage != DateTime.MinValue && StartPage != DateTime.MinValue)
            {
                TimeSpan temp = (FinishPage - StartPage);
                string day = "", hour, min, sec, msec, strFormat = "{0}h {1}m {2}sec {3}msec";
                if (temp.TotalHours > 24)
                {
                    day = temp.Days.ToString();
                    strFormat = "{4}day {0}h {1}m {2}sec {3}msec";
                }
                hour = temp.Hours.ToString();
                min = temp.Minutes.ToString();
                sec = temp.Seconds.ToString();
                msec = temp.Milliseconds.ToString();
                Duration = String.Format(strFormat, hour, min, sec, msec, day);

            }
            else Duration = "Unknown";

        }
    }
}
