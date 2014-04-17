using System.Fakes;
using System;
using Microsoft.QualityTools.Testing.Fakes;
using Xunit;
using MKS.Mobile.Core.Models;

namespace MKS.Mobile.Core.Tests
{
    public class UpdatedTimeSpanTests
    {

        [Fact]
        public void YesterdayTimeCalculationsAreValid()
        {
            Assert.Equal(new TimeSpan(0, 0, 0), DateTime.Today.AddDays(-1).TimeOfDay);
            Assert.Equal(new TimeSpan(23, 59, 59), DateTime.Today.AddSeconds(-1).TimeOfDay);
        }
        [Fact]
        public void WeekTimeCalculationsAreValid()
        {
            var dateTime = new DateTime(2012, 12, 03, 0, 0, 0);
            var endDate = dateTime.AddDays(-6);
            Assert.Equal(new DateTime(2012, 11, 27, 0, 0, 0), endDate);
        }

        [Fact]
        public void TodayTimeIsValid()
        {
            var datetime = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.Today);
            Assert.Equal(new TimeSpan(0, 0, 0), datetime.Start.TimeOfDay);
            Assert.NotEqual(new TimeSpan(23, 59, 59), datetime.End.TimeOfDay);
            Assert.True(datetime.End.TimeOfDay > datetime.Start.TimeOfDay);
        }

        [Fact]
        public void YesterdayTimeIsValid()
        {
            var datetime = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.Yesterday);
            Assert.Equal(new TimeSpan(0, 0, 0), datetime.Start.TimeOfDay);
            Assert.Equal(new TimeSpan(23, 59, 59), datetime.End.TimeOfDay);
        }

        [Fact]
        public void WeekToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.WeekToDate);
                //week start date is Sunday, so for our time period the result is the 10th
                Assert.Equal(new DateTime(2013, 02, 10), dateTimeResult.Start);
                Assert.Equal(fixedDate, dateTimeResult.End);

            }
        }
        [Fact]
        public void MonthToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.MonthToDate);
                //week start date is Sunday, so for our time period the result is the 10th
                Assert.Equal(new DateTime(2013, 02, 01), dateTimeResult.Start);
                Assert.Equal(fixedDate, dateTimeResult.End);

            }
        }

        [Fact]
        public void LastMonthIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.LastMonth);
                
                Assert.Equal(new DateTime(2013, 01, 01), dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 01, 31, 23, 59, 59), dateTimeResult.End);

            }
        }

        [Fact]
        public void LastWeekIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.LastWeek);
                
                Assert.Equal(new DateTime(2013, 02, 3), dateTimeResult.Start);
                Assert.Equal(new DateTime(2013, 02, 9, 23, 59, 59), dateTimeResult.End);

            }
        }

        [Fact]
        public void YearToDateIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.YearToDate);
                
                Assert.Equal(new DateTime(2013, 01, 01), dateTimeResult.Start);
                Assert.Equal(fixedDate, dateTimeResult.End);

            }
        }

        [Fact]
        public void WithinAMonthIsValid()
        {
            var fixedDate = new DateTime(2013, 02, 14, 11, 12, 00);
            using (ShimsContext.Create())
            {
                // Arrange:
                // Detour DateTime.Now to return a fixed date:
                ShimDateTime.NowGet = () => fixedDate;
                ShimDateTime.UtcNowGet = () => fixedDate;
                ShimDateTime.TodayGet = () => fixedDate.Date;
                var dateTimeResult = DateRange.FromUpdatedTimeSpan(UpdatedTimeSpan.WithinAMonth);

                Assert.Equal(new DateTime(2013, 01, 14), dateTimeResult.Start);
                Assert.Equal(fixedDate, dateTimeResult.End);

            }
        }
    }
}
