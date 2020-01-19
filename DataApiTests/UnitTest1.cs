using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SecData;

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
        public async Task TestMethod1()
        {
            await _dataDownloader.TestApi();
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            await _dataDownloader.DownloadData();
        }

    }
}
