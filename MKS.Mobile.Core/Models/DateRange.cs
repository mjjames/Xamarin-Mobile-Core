using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Models
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public static DateRange FromUpdatedTimeSpan(UpdatedTimeSpan timeSpan)
        {
            DateTime start, end;
            switch (timeSpan)
            {
                case UpdatedTimeSpan.Today:
                    start = DateTime.Today;
                    end = DateTime.UtcNow;
                    break;
                case UpdatedTimeSpan.Yesterday:
                    start = DateTime.Today.AddDays(-1); //should give midnight
                    end = DateTime.Today.AddSeconds(-1); //should give 23:59:59
                    break;
                case UpdatedTimeSpan.WeekToDate:
                    start = DateTime.UtcNow.AddDays(0 - (int)DateTime.Today.DayOfWeek).Date;
                    end = DateTime.UtcNow;
                    break;
                case UpdatedTimeSpan.LastMonth:
                    var lastMonth = DateTime.Today.AddMonths(-1);
                    start = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                    end = start.AddMonths(1).AddSeconds(-1);
                    break;
                case UpdatedTimeSpan.LastWeek:
                    var lastWeek = DateTime.Today.AddDays(0 - (7 + (int)DateTime.Today.DayOfWeek));
                    start = lastWeek.Date;
                    end = start.AddDays(7).AddSeconds(-1);
                    break;
                case UpdatedTimeSpan.MonthToDate:
                    start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    end = DateTime.UtcNow;
                    break;
                case UpdatedTimeSpan.YearToDate:
                    start = new DateTime(DateTime.Today.Year, 1, 1);
                    end = DateTime.UtcNow;
                    break;
                case UpdatedTimeSpan.WithinAMonth:
                    start = DateTime.UtcNow.AddMonths(-1).Date;
                    end = DateTime.UtcNow;
                    break;
                default:
                    start = DateTime.UtcNow;
                    end = DateTime.UtcNow;
                    break;
            }
            return new DateRange
            {
                Start = start,
                End = end
            };
        }

        public static DateRange FromDueTimeSpan(DueTimeSpan timeSpan)
        {
            DateTime start, end;
            switch (timeSpan)
            {
                case DueTimeSpan.Today:
                    start = DateTime.Today;
                    end = DateTime.Today.AddDays(1).AddSeconds(-1); ;
                    break;
                case DueTimeSpan.Tomorrow:
                    start = DateTime.Today.AddDays(1); //should give midnight
                    end = DateTime.Today.AddDays(2).AddSeconds(-1); //should give 23:59:59
                    break;
                case DueTimeSpan.ThisWeek:
                    start = DateTime.Today;
                    end = DateTime.Today.AddDays(8).AddSeconds(-1);
                    break;
                case DueTimeSpan.ThisMonth:
                    start = DateTime.Today;
                    var nextMonth = start.AddMonths(1);
                    end = new DateTime(nextMonth.Year, nextMonth.Month, 1).AddSeconds(-1);
                    break;
                case DueTimeSpan.NextMonth:
                    var nextStartMonth = DateTime.Today.AddMonths(1);
                    start = new DateTime(nextStartMonth.Year, nextStartMonth.Month, 1);
                    end = start.AddMonths(1).AddSeconds(-1);
                    break;
                case DueTimeSpan.NextWeek:
                    start = DateTime.Today.AddDays(7);
                    end = DateTime.Today.AddDays(14);
                    break;
                case DueTimeSpan.WithinAMonth:
                    start = DateTime.Today;
                    end = DateTime.Today.AddMonths(1);
                    break;
                default:
                    start = DateTime.UtcNow;
                    end = DateTime.UtcNow;
                    break;
            }
            return new DateRange
            {
                Start = start,
                End = end
            };
        }
    }
}
