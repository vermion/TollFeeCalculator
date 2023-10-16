namespace TollFeeCaclulatorTests
{
    public class TollFeeCalculationTests
    {
        [Fact]
        public void CheckThatCarIsChargedMaxTolleFee_OnRegularWeekDay()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 00, 00),DayOfWeek.Monday) });
            // Act

            // Assert
            Assert.Equal(13m, fee);
        }

        [Fact]
        public void CheckThatCarIsNotCharged_OnNewYearEve()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { new DateTime(2020, 01, 01, 06, 30, 00) });

            // Assert
            Assert.Equal(0m, fee);
        }
    }
}