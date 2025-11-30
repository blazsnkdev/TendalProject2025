namespace TendalProject.Common.Time
{
    public class DateTimeProvider : IDateTimeProvider
    {
            public DateTime GetDateTimeNow() => DateTime.Now;
    }
}
