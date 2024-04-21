using AccessControl.API.Enums;

namespace AccessControl.API.Helper
{
    public class HelperClass
    {
        public static List<string> MapWeekDays(IEnumerable<Days> days)
        {
            var daysList = new List<string>();
            var enumDays = days.ToList();
            enumDays.ForEach(i =>
            {
                if (i.Equals(Days.MONDAY))
                    daysList.Add("Monday");
                if (i.Equals(Days.TUESDAY))
                    daysList.Add("Tuesday");
                if (i.Equals(Days.WEDNESDAY))
                    daysList.Add("Wednesday");
                if (i.Equals(Days.THURSDAY))
                    daysList.Add("Thursday");
                if (i.Equals(Days.FRIDAY))
                    daysList.Add("Friday");
            });
            return daysList;
        }
        public static List<Days> MapScheduledDays(List<string> realDays)
        {
            var enumDays = new List<Days>();
            realDays.ForEach(i =>
            {
                if (i == "Monday")
                    enumDays.Add(Days.MONDAY);
                if (i == "Tuesday")
                    enumDays.Add(Days.TUESDAY);
                if (i == "Wednesday")
                    enumDays.Add(Days.WEDNESDAY);
                if (i == "Thursday")
                    enumDays.Add(Days.THURSDAY);
                if (i == "Friday")
                    enumDays.Add(Days.FRIDAY);
            });

            return enumDays;
        }
    }
}
