namespace TollFeeCalculator
{
    public static class HelperMethods
    {
        public static DateTime GenrateDateWithSpecificWeekday(DateTime date, DayOfWeek desiredDay)
        {
            // Calculate difference in days
            int daysToAdd = ((int)desiredDay - (int)date.DayOfWeek + 7) % 7;
            if (daysToAdd == 0)
            {
                // If the day is already the desired day, you might want to do nothing or move to the next week's same day based on your requirement.
                // In this example, we move to the next week's same day.
                daysToAdd = 7;
            }

            // Adjust the date
            return date.AddDays(daysToAdd);
        }
    }
}