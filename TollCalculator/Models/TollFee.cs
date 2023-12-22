namespace TollFeeCalculator.Models
{
    internal class TollFee
    {
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public decimal TollFeeAmoúnt { get; set; }
    }
}