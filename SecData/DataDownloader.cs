using Newtonsoft.Json;
using SecData.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecData
{
    public class DataDownloader
    {
        string apiKey = "3d023d6d3d0772581b93c09ad03e61ea6ff3dbfa643c6913a17f3ea1019cb95c";
        string apiUrl = "https://api.sec-api.io?token=3d023d6d3d0772581b93c09ad03e61ea6ff3dbfa643c6913a17f3ea1019cb95c";
        private static readonly HttpClient client = new HttpClient();

        public async Task TestApi()
        {
            try
            {
                var response = await client.GetAsync(apiUrl);
                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
            }
        }

        // all the form types: https://sec-api.io/#list-of-sec-form-types

        public async Task DownloadData()
        {
            var query = new { query_string = new { query = "filedAt:{2019-12-01 TO 2020-12-31} AND formType:\"13F\"" } };
            var sort = new List<object> { new { filedAt = new { order = "desc" } } };
            var values = new { 
                query = query,
                from = 0,
                size = 10,
                sort = sort
            };

            var valuesJson = JsonConvert.SerializeObject(values);

            //var content = new FormUrlEncodedContent(values);
            var stringContent = new StringContent(valuesJson, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, stringContent);

            var responseString = await response.Content.ReadAsStringAsync();

            QueryResponse queryResponse = JsonConvert.DeserializeObject<QueryResponse>(responseString);
        }
    }
}
