using APeiLee.Tool;

namespace APeiLee.Tool.Tests
{
    public class TestEnumValueHelper
    {
        [Fact]
        public void TestEnumDescription()
        {
            var descStr = CarTypeEnum.Car.EnumDescription();

            Assert.Equal("小轿车", descStr);
        }

        public enum CarTypeEnum
        {
            [System.ComponentModel.Description("小轿车")]
            Car = 0,

            [System.ComponentModel.Description("大巴车")]
            Bus = 1,
        }
    }
}