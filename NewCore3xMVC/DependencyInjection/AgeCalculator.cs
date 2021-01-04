using System;

namespace NewCore3xMVC.DependencyInjection
{
    public class AgeCalculator : IAgeCalculator
    {
        public string GetMyAge(DateTime dob)
        {
            var val = DateTime.Now.Subtract(dob);
            DateTime age = DateTime.MinValue + val;
            return string.Format("{0} Years {1} months {2} days", age.Year - 1, age.Month - 1, age.Day - 1);
        }
    }
}
