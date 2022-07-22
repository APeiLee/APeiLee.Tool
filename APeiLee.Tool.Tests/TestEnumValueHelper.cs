using APeiLee.Tool;

namespace APeiLee.Tool.Tests
{
    public class TestEnumValueHelper
    {
        [Fact]
        public void TestEnumDescription()
        {
            var descStr = CarTypeEnum.Car.EnumDescription();

            Assert.Equal("С�γ�", descStr);
        }

        public enum CarTypeEnum
        {
            [System.ComponentModel.Description("С�γ�")]
            Car = 0,

            [System.ComponentModel.Description("��ͳ�")]
            Bus = 1,
        }
    }
}