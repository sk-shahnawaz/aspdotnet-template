using System;
using Newtonsoft.Json;

namespace ASP.NET.Core.WebAPI.Infrastructure.API.Converters
{
    /// <summary>
    /// Enables this API to return / receive date time in a fixed format to / from its clients.
    /// </summary>
    public class DateConverter : JsonConverter
    {
        private readonly string _dateTimeFormat = string.Empty;

        public DateConverter()
        {
            // Open API specification format for 'Date' type.
            // See: https://stackoverflow.com/a/49379235
            _dateTimeFormat = "yyyy-MM-dd";

            // Forcing the API to consider only this format when it is invoked from other clients.
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime))
                return true;
            return false;
        }

        public override bool CanRead => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(DateTime))
                return DateTime.ParseExact(Convert.ToString(reader.Value), _dateTimeFormat, null, System.Globalization.DateTimeStyles.None);
            return Convert.ToString(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
                writer.WriteValue(dateTime.ToString(_dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}