namespace AccessControl.API.Helpers
{
    public class HelperClass
    {
        public static List<string> MapWeekDays(IEnumerable<DayOfWeek> days)
        {
            var daysList = new List<string>();
            var enumDays = days.ToList();
            enumDays.ForEach(i =>
            {
                if (i.Equals(DayOfWeek.Sunday))
                    daysList.Add("Sunday");
                if (i.Equals(DayOfWeek.Monday))
                    daysList.Add("Monday");
                if (i.Equals(DayOfWeek.Tuesday))
                    daysList.Add("Tuesday");
                if (i.Equals(DayOfWeek.Wednesday))
                    daysList.Add("Wednesday");
                if (i.Equals(DayOfWeek.Thursday))
                    daysList.Add("Thursday");
                if (i.Equals(DayOfWeek.Friday))
                    daysList.Add("Friday");
                if (i.Equals(DayOfWeek.Saturday))
                    daysList.Add("Saturday");
            });
            return daysList;
        }

        public static List<DayOfWeek> MapScheduledDays(List<string> realDays)
        {
            var enumDays = new List<DayOfWeek>();
            realDays.ForEach(i =>
            {
                if (i == "Sunday")
                    enumDays.Add(DayOfWeek.Sunday);
                if (i == "Monday")
                    enumDays.Add(DayOfWeek.Monday);
                if (i == "Tuesday")
                    enumDays.Add(DayOfWeek.Tuesday);
                if (i == "Wednesday")
                    enumDays.Add(DayOfWeek.Wednesday);
                if (i == "Thursday")
                    enumDays.Add(DayOfWeek.Thursday);
                if (i == "Friday")
                    enumDays.Add(DayOfWeek.Friday);
                if (i == "Saturday")
                    enumDays.Add(DayOfWeek.Saturday);
            });

            return enumDays;
        }

        public class ColorGenerator
        {
            private static Random random = new Random();

            public static string GenerateRandomColor()
            {
                int red = random.Next(256);
                int green = random.Next(256);
                int blue = random.Next(256);

                return $"#{red:X2}{green:X2}{blue:X2}";
            }
        }
    }
}
