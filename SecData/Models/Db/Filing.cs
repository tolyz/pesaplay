using System;

namespace SecData.Models.Db
{
    public class Filing
    {
        public int Id { get; set; }

        public string Cik { get; set; }

        public DateTime FileDate { get; set; }

        public string FormType { get; set; }
    }
}
