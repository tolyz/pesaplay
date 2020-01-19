using Newtonsoft.Json;

namespace SecData.Models
{
    public class Filing
    {
        [JsonProperty("cik")]
        public string Cik { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("companyNameLong")]
        public string CompanyNameLong { get; set; }

        [JsonProperty("formType")]
        public string FormType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("filedAt")]
        public string FiledAt { get; set; }

        [JsonProperty("linkToTxt")]
        public string LinkToTxt { get; set; }

        [JsonProperty("linkToHtml")]
        public string LinkToHtml { get; set; }

        [JsonProperty("linkToXbrl")]
        public string LinkToXbrl { get; set; }

        [JsonProperty("linkToFilingDetails")]
        public string LinkToFilingDetails { get; set; }

    }
}
