using Newtonsoft.Json;
using System.Collections.Generic;

namespace SecData.Models
{
    public class QueryResponse
    {
        [JsonProperty("total")]
        public Total Tot { get; set; }

        [JsonProperty("filings")]
        public List<Filing> Filings { get; set; }

        public class Total
        {
            [JsonProperty("value")]
            public int Value { get; set; }

            [JsonProperty("relation")]
            public string Relation { get; set; }
        }
    }
}
