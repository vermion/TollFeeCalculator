namespace TollFeeCalculator
{
    public class TollCalculator
    {
        /**
         * Calculate the total toll currentFee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - current and time of all passes on one day
         * @return - the total toll currentFee for that day
         */
        public decimal GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
        {
            DateTime previous = dates.First();
            decimal totalFee = 0m;
            foreach (DateTime current in dates)
            {
                decimal currentFee = GetTollFee(current, vehicle);
                decimal previousFee = GetTollFee(previous, vehicle);

                TimeSpan difference = current - previous;
                int minutesDifference = (int)difference.TotalMinutes; // Not sure about rounding error. Needs some investigation

                if (minutesDifference <= 60)
                {                        
                    if (currentFee >= previousFee)
                    {
                        // totalFee = totalFee - previousFee;
                        totalFee = totalFee + currentFee;
                    }
                }
                else
                {
                    totalFee += currentFee;
                }

                if (totalFee > 60m)
                {
                    totalFee = 60m;
                    break;
                }
                previous = current;
            }
            
            return totalFee;
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle is null)
            {
                return false;
            }

            var vehicleType = vehicle.GetType().Name;

            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
        }

        private decimal GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) 
            { 
                return 0; 
            }

            int hour = date.Hour;
            int minute = date.Minute;

            switch (hour)
            {
                case 6 when minute >= 0 && minute <= 29:
                    return 8m;
                case 6 when minute >= 30 && minute <= 59:
                    return 13m;
                case 7 when minute >= 0 && minute <= 59:
                    return 18m;
                case 8 when minute >= 0 && minute <= 29:
                    return 13m;
                case >= 8 and <= 14 when minute >= 30 && minute <= 59:
                    return 8m;
                case 15 when minute >= 0 && minute <= 29:
                    return 13m;
                case 15 when minute >= 0:
                case 16 when minute <= 59:
                    return 18m;
                case 17 when minute >= 0 && minute <= 59:
                    return 13m;
                case 18 when minute >= 0 && minute <= 29:
                    return 8m;
                default:
                    return 0m;
            }
        }

        private bool IsTollFreeDate(DateTime date)
        {
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            switch (month)
            {
                case 1 when day == 1:
                case 3 when day == 28 || day == 29:
                case 4 when day == 1 || day == 30:
                case 5 when day == 1 || day == 8 || day == 9:
                case 6 when day == 5 || day == 6 || day == 21:
                case 7:
                case 11 when day == 1:
                case 12 when day == 24 || day == 25 || day == 26 || day == 31:
                    return true;
                default:
                    return false;
            }
        }

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}