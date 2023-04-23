using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IFileService
    {
        public Task DownloadFileAsync(string url);
        public (bool, string) InsertAllCsvRecordsIntoDB();
        public Task<(bool, string)> DownloadMultipleFilesAsync(List<string> urls);
        public Task<(bool, string)> UnzippingAllFiles();
        public List<string> getFileList();

    }
}