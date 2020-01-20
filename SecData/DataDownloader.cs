using Newtonsoft.Json;
using SecData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

        // query api docs: https://sec-api.io/docs#query-examples
        // all the form types: https://sec-api.io/#list-of-sec-form-types

        public async Task<QueryResponse> DownloadData(DateTime startDate, DateTime endDate, string formType, int paginationStart = 0, int count = 10)
        {
            var startDateString = startDate.ToString("yyyy-MM-dd");
            var endDateString = endDate.ToString("yyyy-MM-dd");

            var query = new { query_string = new { query = "filedAt:{"+ startDateString + " TO "+ endDateString + "} AND formType:\""+ formType + "\"" } };
            var sort = new List<object> { new { filedAt = new { order = "desc" } } };
            var values = new { 
                query = query,
                from = paginationStart,
                size = count,
                sort = sort
            };

            var valuesJson = JsonConvert.SerializeObject(values);

            var stringContent = new StringContent(valuesJson, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, stringContent);

            var responseString = await response.Content.ReadAsStringAsync();

            QueryResponse queryResponse = JsonConvert.DeserializeObject<QueryResponse>(responseString);

            return queryResponse;
        }

        public List<InfoTable> Get13FData(string url)
        {
            try
            {
                var client = new WebClient();
                var rawXml = client.DownloadString(url);

                var stringReader = new StringReader(rawXml);
                List<string> xmlBlocks = new List<string>();
                string xmlBlock = "";
                var accumulateXmlBlock = false;

                while (true)
                {
                    var aLine = stringReader.ReadLine();

                    if (aLine != null)
                    {
                        if (aLine == "<XML>")
                        {
                            accumulateXmlBlock = true;
                            continue;
                        }
                        else if (aLine == "</XML>")
                        {
                            accumulateXmlBlock = false;
                            xmlBlocks.Add(xmlBlock);
                            xmlBlock = "";
                        }

                        if (accumulateXmlBlock)
                            xmlBlock += aLine;
                    }
                    else
                    {
                        if(accumulateXmlBlock && !string.IsNullOrEmpty(xmlBlock)) // in case closing tag is missing
                            xmlBlocks.Add(xmlBlock);
                        break;
                    }
                }

                foreach(var xml in xmlBlocks)
                {
                    var data =
                        from el in StreamRootChildDoc(new StringReader(xml), "infoTable")
                        select el;

                    if (data.Count() == 0)
                        continue;

                    var sanitizedXml = SanitizeXml(xml);

                    var serializer = new XmlSerializer(typeof(InfoTableList));

                    using (TextReader reader = new StringReader(sanitizedXml))
                    {
                        var result = (InfoTableList)serializer.Deserialize(reader);
                        return result.InfoTables;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return new List<InfoTable>();
        }

        static bool IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static IEnumerable<XElement> StreamRootChildDoc(StringReader stringReader, string searchElement)
        {
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                reader.MoveToContent();
                // Parse the file and display each of the nodes.  
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == searchElement)
                            {
                                XElement el = XElement.ReadFrom(reader) as XElement;
                                if (el != null)
                                    yield return el;
                            }
                            break;
                    }
                }
            }
        }

        public static string SanitizeXml(string xml)
        {
            var newXml = "";

            try
            {
                var stringReader = new StringReader(xml);
                byte[] byteArray = Encoding.ASCII.GetBytes(xml);
                MemoryStream stream = new MemoryStream(byteArray);
                XmlTextReader xtr = new XmlTextReader(stream);
                xtr.WhitespaceHandling = WhitespaceHandling.None;
                xtr.Read();
                string currentTag = null;

                while (!xtr.EOF)
                {
                    var tag = xtr.Name;
                    var nodeType = xtr.NodeType;
                    if(nodeType == XmlNodeType.Element)
                    {
                        newXml += "<" + tag + ">";
                        currentTag = tag;
                    }
                    else if (nodeType == XmlNodeType.EndElement)
                    {
                        newXml += "</" + tag + ">";
                    }
                    else if (nodeType == XmlNodeType.Text)
                    {
                        var content = xtr.ReadContentAsString()
                            .Replace("<", "&lt;").Replace("&", "&amp;")
                            .Replace(">", "&gt;")
                            .Replace("\"", "&quot;")
                            .Replace("'", "&apos;");
                        newXml += content;
                        newXml += "</" + currentTag + ">";
                    }

                    xtr.Read();
                }

                xtr.Close();
            }
            catch(Exception e)
            {
            }

            return newXml;
        }
    }
}
