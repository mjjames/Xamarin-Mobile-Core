namespace MKS.Mobile.Core.Models
{
    public enum DueTimeSpan
    {
        Today,
        Tomorrow,
        /// <summary>
        /// Within the next 7 days
        /// </summary>
        ThisWeek,
        /// <summary>
        /// from 7 days time until 14 days time
        /// </summary>
        NextWeek,
        /// <summary>
        /// This calendar month
        /// </summary>
        ThisMonth,
        /// <summary>
        /// Next calendar month
        /// </summary>
        NextMonth,
        /// <summary>
        /// One month from today, regardless of calendar month
        /// </summary>
        WithinAMonth

    }
}
