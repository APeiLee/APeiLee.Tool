using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APeiLee.Tool.Tests
{
    public class TestTaskWaitHelper
    {
        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        public void TestCtorError(int maxTaskNum, int taskDetectionInterval = 200)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                TaskWaitHelper waitHelper = new TaskWaitHelper(maxTaskNum, taskDetectionInterval);
            });
        }
    }
}
