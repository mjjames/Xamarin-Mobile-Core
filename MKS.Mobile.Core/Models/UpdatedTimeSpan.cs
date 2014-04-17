namespace MKS.Mobile.Core.Models
{
    public enum UpdatedTimeSpan
    {
        Today,
        Yesterday,
        /// <summary>
        /// The last calendar month
        /// </summary>
        LastMonth,
        LastWeek,
        /// <summary>
        /// The current calendar month to date
        /// </summary>
        MonthToDate,
        WeekToDate,
        YearToDate,
        /// <summary>
        /// The last month, regardless of calendar month, simply a ~30 day period
        /// </summary>
        WithinAMonth
    }
}
