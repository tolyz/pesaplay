using System.Collections.Generic;
using System.Xml.Serialization;

namespace SecData.Models
{
    [XmlRoot("informationTable")]
    public class InfoTableList
    {
        [XmlElement("infoTable")]
        public List<InfoTable> InfoTables { get; set; }
    }


    [XmlRoot("infoTable")]
    public class InfoTable
    {
        [XmlElement("nameOfIssuer")]
        public string NameOfIssuer { get; set; }
        
        [XmlElement("titleOfClass")]
        public string TitleOfClass { get; set; }

        [XmlElement("cusip")]
        public string Cusip { get; set; }

        [XmlElement("value")]
        public string Value { get; set; }

        [XmlElement("shrsOrPrnAmt")]
        public Shrs ShrsOfPrnAmt { get; set; }

        [XmlElement("investmentDiscretion")]
        public string InvestmentDiscretion { get; set; }


        [XmlRoot("shrsOrPrnAmt")]
        public class Shrs
        {
            [XmlElement("sshPrnamt")]
            public string SshPrnamt { get; set; }

            [XmlElement("sshPrnamtType")]
            public string SshPrnamtType { get; set; }
        }
        
    }
}
