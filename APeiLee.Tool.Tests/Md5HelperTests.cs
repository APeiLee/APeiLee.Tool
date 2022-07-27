using Xunit;
using APeiLee.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APeiLee.Tool.Tests
{
    public class Md5HelperTests
    {
        [Fact()]
        public void GetMd5Test()
        {
            var id = Md5Helper.GetMd5("Hello World!");
            Assert.Equal("ED076287532E86365E841E92BFC50D8C", id);
        }
    }
}