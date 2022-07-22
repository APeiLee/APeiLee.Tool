using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APeiLee.Tool.Tests
{
    public class TestObjePropertyHelper
    {
        [Fact]
        public void TestStringNullToEmpty()
        {
            var s = new Student();
            s = s.StringNullToEmpty();

            Assert.NotNull(s.Name);
        }

        internal class Student
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
