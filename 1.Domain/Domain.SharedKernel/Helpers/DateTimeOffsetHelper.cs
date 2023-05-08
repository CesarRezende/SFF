using System.Globalization;
using System.Runtime.InteropServices;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("SFF.SharedKernel.Tests")]
namespace SFF.SharedKernel.Helpers
{
    public static class DateTimeOffsetHelper
    {
        public static DateTimeOffset ConvertToBrazilianDate(this DateTimeOffset source)
        {
            var southamericaTimeZone = GetBrazilianTimeZone();
            return source.ToOffset(southamericaTimeZone.BaseUtcOffset);
        }

        public static string ToBrazilianDate(this DateTimeOffset source)
        {
            return ToBrazilianHour(source, "dd/MM/yyyy");
        }

        public static string ToBrazilianShortDate(this DateTimeOffset source)
        {
            return ToBrazilianHour(source, "dd/MM");
        }

        public static string ToBrazilianTime(this DateTimeOffset source)
        {
            return ToBrazilianHour(source, "HH:mm:ss");
        }

        public static string ToBrazilianShortTime(this DateTimeOffset source)
        {
            return ToBrazilianHour(source, "HH:mm");
        }

        public static string ToBrazilianLongDateTime(this DateTimeOffset source)
        {
            return ToBrazilianHour(source, "dd/MM/yyyy HH:mm:ss");
        }

        public static TimeZoneInfo GetBrazilianTimeZone()
        {
            TimeZoneInfo timezone = TimeZoneInfo.Local;

            try
            {
                string timezoneId = "America/Sao_Paulo";

                var listaTimezone = TimeZoneInfo.GetSystemTimeZones();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    timezoneId = "E. South America Standard Time";
                }

                if (listaTimezone.Any(x => x.Id == timezoneId))
                    timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            }
            catch { }

            return timezone;
        }


        public static DateTimeOffset BrazilianFistDayOffCurrentMonth()
        {
            var brazillianNow = GetBrazillianNow();

            return new DateTimeOffset(
                            new DateTime(
                            year: brazillianNow.Date.Year,
                            month: brazillianNow.Date.Month,
                            day: 1
                            ),
                            offset: DateTimeOffsetHelper.GetBrazilianTimeZone().BaseUtcOffset
                            );
        }

        public static DateTimeOffset BrazilianLastDayOffCurrentMonth()
        {
            var brazillianNow = GetBrazillianNow();
            var daysInMonth = DateTime.DaysInMonth(brazillianNow.Date.Year, brazillianNow.Date.Month);

            return new DateTimeOffset(
                            new DateTime(
                            year: brazillianNow.Date.Year,
                            month: brazillianNow.Date.Month,
                            day: daysInMonth
                            ),
                            offset: DateTimeOffsetHelper.GetBrazilianTimeZone().BaseUtcOffset
                            );
        }

        private static DateTimeOffset GetBrazillianNow()
        {
            return DateTimeOffset.Now.ConvertToBrazilianDate();
        }

        private static string ToBrazilianHour(this DateTimeOffset source, string format)
        {
            var southamericaDate = ConvertToBrazilianDate(source);
            return southamericaDate.ToString(format, new CultureInfo("pt-BR"));
        }
    }
}
