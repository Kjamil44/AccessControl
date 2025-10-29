namespace AccessControl.API.Enums
{
    public enum ScheduleType
    {
        /// <summary>
        /// This type is the default schedule. It allows the user to choose weekdays, precise dates, and time slots, providing more flexibility for recurring or long-term planning.
        /// </summary>
        Standard,
        /// <summary>
        /// This type is a short-term schedule. The user can select specific time slots only, making it suitable for brief engagements. 
        /// </summary>
        Temporary
    }
}
