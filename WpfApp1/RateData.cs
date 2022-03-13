using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class Rates
    {
        [JsonProperty("Date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("PreviousDate")]
        public DateTimeOffset PreviousDate { get; set; }

        [JsonProperty("PreviousURL")]
        public string PreviousUrl { get; set; }

        [JsonProperty("Timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("Valute")]
        public Dictionary<string, Valute> Valute { get; set; }
    }

    public partial class Valute
    {
        [JsonProperty("ID")]
        public string Id { get; set; }

        [JsonProperty("NumCode")]
        public string NumCode { get; set; }

        [JsonProperty("CharCode")]
        public string CharCode { get; set; }

        [JsonProperty("Nominal")]
        public long Nominal { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public double Value { get; set; }

        [JsonProperty("Previous")]
        public double Previous { get; set; }
    }

    public partial class Rates
    {
        public static Rates FromJson(string json) => JsonConvert.DeserializeObject<Rates>(json, WpfApp1.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Rates self) => JsonConvert.SerializeObject(self, WpfApp1.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class RateData
    {
        private Rates rates;
        public RateData(string url)
        {
            using (WebClient client = new WebClient())
            {
                rates = Rates.FromJson(client.DownloadString(url));
            }
        }

        public Rates Rates { get => rates; }
    }
}
