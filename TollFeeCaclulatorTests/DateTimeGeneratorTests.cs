using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCaclulatorTests
{
    public class DateTimeGeneratorTests
    {
        [Fact]
        public void CheckThatCorrectWeekdayIsSet()
        {
            // Arrange, Act
            var expected = HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2023, 10, 7, 0, 0, 0), DayOfWeek.Monday);

            // Assert
            Assert.Equal(DayOfWeek.Monday, expected.DayOfWeek);
        }
    }
}
