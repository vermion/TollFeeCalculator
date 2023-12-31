﻿using TollFeeCalculator.Models;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private List<TollFee> _tollFees = new List<TollFee>();

        private List<TollFreeDate> _tollFreeDates = new List<TollFreeDate>();

        public TollCalculator()
        {
            // This should be moved to a database or a configuration file. As this is just for a specific year.
            _tollFreeDates.Add(new TollFreeDate { Month = 1, Date = 1 });
            _tollFreeDates.Add(new TollFreeDate { Month = 3, Date = 28 });
            _tollFreeDates.Add(new TollFreeDate { Month = 3, Date = 29 });
            _tollFreeDates.Add(new TollFreeDate { Month = 4, Date = 1 });
            _tollFreeDates.Add(new TollFreeDate { Month = 4, Date = 30 });
            _tollFreeDates.Add(new TollFreeDate { Month = 5, Date = 1 });
            _tollFreeDates.Add(new TollFreeDate { Month = 5, Date = 8 });
            _tollFreeDates.Add(new TollFreeDate { Month = 5, Date = 9 });
            _tollFreeDates.Add(new TollFreeDate { Month = 6, Date = 6 });
            _tollFreeDates.Add(new TollFreeDate { Month = 6, Date = 21 });
            _tollFreeDates.Add(new TollFreeDate { Month = 7, Date = 1 });
            _tollFreeDates.Add(new TollFreeDate { Month = 11, Date = 1 });
            _tollFreeDates.Add(new TollFreeDate { Month = 12, Date = 24 });
            _tollFreeDates.Add(new TollFreeDate { Month = 12, Date = 25 });
            _tollFreeDates.Add(new TollFreeDate { Month = 12, Date = 26 });
            _tollFreeDates.Add(new TollFreeDate { Month = 12, Date = 31 });

            // This should be moved to a database or a configuration file.
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(06, 00), EndTime = new TimeOnly(06, 29), TollFeeAmoúnt = 8m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(06, 30), EndTime = new TimeOnly(06, 59), TollFeeAmoúnt = 13m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(07, 00), EndTime = new TimeOnly(07, 59), TollFeeAmoúnt = 18m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(08, 00), EndTime = new TimeOnly(08, 29), TollFeeAmoúnt = 13m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(08, 30), EndTime = new TimeOnly(14, 59), TollFeeAmoúnt = 8m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(15, 00), EndTime = new TimeOnly(15, 29), TollFeeAmoúnt = 13m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 59), TollFeeAmoúnt = 18m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(17, 00), EndTime = new TimeOnly(17, 59), TollFeeAmoúnt = 13m });
            _tollFees.Add(new TollFee { StartTime = new TimeOnly(18, 00), EndTime = new TimeOnly(18, 29), TollFeeAmoúnt = 8m });
        }

        /**
         * Calculate the total toll currentFee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - current and time of all passes on one day
         * @return - the total toll currentFee for that day
         */
        public decimal GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
        {
            // Maybe add some sanity check on dates and check if vehicle is null.

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
            var vehicleType = vehicle.GetType().Name; 

            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType); // This property could be moved to the Vehicle class for a more OOP approach.
        }

        private decimal GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            {
                return 0m;
            }

            var timeOfEntry = TimeOnly.FromDateTime(date);

            foreach (var tollFee in _tollFees)
            {
                if (timeOfEntry >= tollFee.StartTime && timeOfEntry <= tollFee.EndTime)
                {
                    return tollFee.TollFeeAmoúnt;
                }
            }

            return 0m;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            foreach (var tollFreeDate in _tollFreeDates)
            {
                if (tollFreeDate.Month == month && tollFreeDate.Date == day)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
