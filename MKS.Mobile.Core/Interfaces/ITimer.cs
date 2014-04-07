using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKS.Mobile.Core.Interfaces
{
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Resets the timer to start the period cycle again
        /// </summary>
        void Reset();
        /// <summary>
        /// Action to perform on period elapsed
        /// </summary>
        Action<DateTime> Elapsed { get; set; }
        /// <summary>
        /// Interval Length
        /// </summary>
        TimeSpan Interval { get; set; }
        /// <summary>
        /// Whether the timer should be enabled and running
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
