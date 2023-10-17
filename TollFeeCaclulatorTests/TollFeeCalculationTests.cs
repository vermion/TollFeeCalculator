namespace TollFeeCaclulatorTests
{
    public class TollFeeCalculationTests
    {
        [Fact]
        public void CheckThatCarIsChargedMaxTolleFee_OnRegularWeekDay()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 00, 00),DayOfWeek.Monday) });
            
            // Assert
            Assert.Equal(18m, fee);
        }

        [Fact]
        public void CheckThatCarIsNotCharged_OnWeekend()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 00, 00), DayOfWeek.Sunday) });

            // Assert
            Assert.Equal(0m, fee);
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

        [Fact]
        public void CheckThatMotorcycleIsNotCharged_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Motorbike(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 00, 00), DayOfWeek.Monday) });

            // Assert
            Assert.Equal(0m, fee);
        }

        [Fact]
        public void CheckThatHighestTollFeeIsUsed_CarShouldPassTwiceWithin60Minutes_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 30, 00), DayOfWeek.Monday), 
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 20, 00), DayOfWeek.Monday) });
            // Assert
            Assert.Equal(18m, fee);
        }

        [Fact]
        public void CheckThatTotalTollFeeIsCalculated_CarPassesTwiceOver60MinuteInterval_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 05, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 25, 00), DayOfWeek.Monday) });
            // Assert
            Assert.Equal(31m, fee);
        }

        [Fact]
        public void CheckThatTotalTollFeeIsCalculated_CarPassesThreeTimesEachPassingMoreThen60Minutes_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 05, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 25, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 09, 50, 00), DayOfWeek.Monday) });
            // Assert
            Assert.Equal(39m, fee);
        }

        [Fact]
        public void CheckThatTotalTollFeeIsCalculated_CarPassesThreeTimes_FirstAndSecondPassingWithin60Minutes_LastPassingMoreThen60MinutesFromPreviousPassing_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 05, 00), DayOfWeek.Monday), // 18 kr <- Charged
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 00, 00), DayOfWeek.Monday), // 13 kr <- Not charged
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 09, 50, 00), DayOfWeek.Monday) }); // 8 kr <- Charged
            // Assert
            Assert.Equal(26m, fee);
        }

        [Fact]
        public void CheckThatTotalTollFeeIsCalculated_CarPassesThreeTimes_FirstAndSecondPassingOver60Minutes_LastPassingLessThen60MinutesFromPreviousPassing_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 05, 00), DayOfWeek.Monday), // 18 kr <- Charged
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 06, 00), DayOfWeek.Monday), // 13 kr <- Charged
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 09, 01, 00), DayOfWeek.Monday) }); // 8 kr <- Not charged
            // Assert
            Assert.Equal(31m, fee);
        }

        [Fact]
        public void CheckThatTotalTollFeeDoesNotExceed60_OnRegularWeekday()
        {
            // Arrange
            var tollCalculator = new TollCalculator();

            // Act
            var fee = tollCalculator.GetTotalTollFee(new Car(), new DateTime[] { HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 07, 05, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 08, 00, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 09, 50, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 15, 30, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 16, 35, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 17, 45, 00), DayOfWeek.Monday),
                                                                                 HelperMethods.GenrateDateWithSpecificWeekday(new DateTime(2020, 01, 03, 17, 45, 00), DayOfWeek.Monday)});
            // Assert
            Assert.Equal(60m, fee);
        }
    }
}