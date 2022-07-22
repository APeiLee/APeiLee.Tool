using Xunit;
using APeiLee.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APeiLee.Tool.Tests
{
    public class DateTimeHelperTests
    {
        [Fact()]
        public void ToDateTimeTest()
        {
            Int64 unixTimeStampInt64 = 1658483589;
            var dt64 = unixTimeStampInt64.ToDateTime();
            Int32 unixTimestampInt32 = 1658483589;
            var dt32 = unixTimestampInt32.ToDateTime();
            Assert.True(dt64 == dt32);

            var localDt = unixTimeStampInt64.ToDateTimeInLocal();
            var bjDt = unixTimeStampInt64.ToDateTimeInBeijing();
            Assert.True(localDt == bjDt);
        }

        [Fact()]
        public void ToNullDateTimeTest()
        {
            string timeStr = "2022-07-22 17:53:09";
            var dtFromStr = timeStr.ToDateTime();

            Int64 unixTimeStamp = 1658483589;
            var uTime = unixTimeStamp.ToDateTime().ToLocalTime();
            Assert.True(uTime == dtFromStr);
        }

        [Fact()]
        public void ToTimeStringTest()
        {
            Int64 unixTimeStamp = 1658483589;
            var uTime = unixTimeStamp.ToDateTimeInBeijing();
            var timeStr = uTime.ToTimeString();
            Assert.Equal("2022-07-22 17:53:09", timeStr);

            Int64 u = uTime.ToUnixTimestamp();
            Assert.Equal(unixTimeStamp, u);
        }
    }
}