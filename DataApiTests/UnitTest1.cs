using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SecData;
using System;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace DataApiTests
{
    [TestClass]
    public class UnitTest1
    {
        private DataDownloader _dataDownloader;

        [TestInitialize]
        public void Init()
        {
            _dataDownloader = new DataDownloader();
        }


        [TestMethod]
        public async Task TestApiConnection()
        {
            await _dataDownloader.TestApi();
        }

        [TestMethod]
        public async Task Get13Fs()
        {
            var startTime = DateTime.ParseExact("2019-12-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endTime = DateTime.ParseExact("2019-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var response = await _dataDownloader.DownloadData(startTime, endTime, "13F", 0, 10);
            Assert.IsTrue(response.Filings.Count == 10);
        }

        [TestMethod]
        public void Get13FData()
        {
            var result = _dataDownloader.Get13FData("https://www.sec.gov/Archives/edgar/data/1781948/000178194819000001/0001781948-19-000001.txt");
            Assert.IsTrue(result.Count == 33);
        }

        [TestMethod]
        public void SanitizeXml()
        {
            var testXml = @"<informationTable xmlns='http://www.sec.gov/edgar/document/thirteenf/informationtable'>
                        <infoTable>
                            <nameOfIssuer>ABERDEEN STD PALLADIUM ETF T</nameOfIssuer>
                            <titleOfClass>PHYSCL PALLADM</titleOfClass>
                            <cusip>003262102</cusip>
                            <value>16361</value>
                            <shrsOrPrnAmt>
                              <sshPrnamt>102775</sshPrnamt>
                              <sshPrnamtType>SH</sshPrnamtType>
                            </shrsOrPrnAmt>
                            <investmentDiscretion>SOLE</investmentDiscretion>
                            <votingAuthority>
                              <Sole>0</Sole>
                              <Shared>0</Shared>
                              <None>102775</None>
                            </votingAuthority>
                          </infoTable>
                        </informationTable>";

            var xml = DataDownloader.SanitizeXml(testXml);
        }



        [XmlRoot("informationTable")]
        public class InfoTableList
        {
            [XmlElement("infoTable")]
            public List<InfoTable> Nodes { get; set; }
        }

        [XmlRoot("infoTable")]
        public class InfoTable
        {
            [XmlElement("value")]
            public string Value { get; set; }
        }

        [TestMethod]
        public void TestXmlReader()
        {
            var sanitizedXml = "<informationTable><infoTable><value>test1</value><value2>test1</value2></infoTable><infoTable><value>test2</value><value2>test2</value2></infoTable></informationTable>";

            try
            {
                var serializer = new XmlSerializer(typeof(InfoTableList));

                using (TextReader reader = new StringReader(sanitizedXml))
                {
                    var result = (InfoTableList)serializer.Deserialize(reader);
                }
            }
            catch(Exception e)
            {
            }
        }
    }
}
