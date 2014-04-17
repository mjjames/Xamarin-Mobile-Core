using System;
using System.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Xunit;
using MKS.Mobile.Core.Models;

namespace MKS.Mobile.Core.Tests
{
    public class DueTimeSpanTests
    {
        [Fact]
        public void NextMonthToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.NextMonth);
                
                Assert.Equal(new DateTime(2013, 03, 01), dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 03, 31, 23,59,59), dateTimeResult.End);

            }
        }

        [Fact]
        public void ThisMonthToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.ThisMonth);
                
                Assert.Equal(fixedDate.Date, dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 28, 23,59,59), dateTimeResult.End);

            }
        }

        [Fact]
        public void NextWeekToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.NextWeek);

                Assert.Equal(new DateTime(2013, 02, 21), dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 28), dateTimeResult.End);

            }
        }

        [Fact]
        public void ThisWeekToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.ThisWeek);

                Assert.Equal(fixedDate.Date, dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 21, 23,59,59), dateTimeResult.End);

            }
        }

        [Fact]
        public void TodayToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.Today);

                Assert.Equal(fixedDate.Date, dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 14, 23, 59, 59), dateTimeResult.End);

            }
        }

        [Fact]
        public void TomorrowToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.Tomorrow);

                Assert.Equal(new DateTime(2013, 02, 15), dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 15, 23, 59, 59), dateTimeResult.End);

            }
        }

        [Fact]
        public void WithinAMonthDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromDueTimeSpan(DueTimeSpan.WithinAMonth);

                Assert.Equal(fixedDate.Date, dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 03, 14,0,0,0), dateTimeResult.End);

            }
        }
    }
}
