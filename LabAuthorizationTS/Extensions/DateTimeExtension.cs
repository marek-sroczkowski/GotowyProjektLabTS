namespace LabAuthorizationTS.Extensions
{
    public static class DateTimeExtension
    {
        public static bool IsAdult(this DateTime birthDate, int minimumAge)
        {
            return birthDate.AddYears(minimumAge) <= DateTime.Today;
        }
    }
}