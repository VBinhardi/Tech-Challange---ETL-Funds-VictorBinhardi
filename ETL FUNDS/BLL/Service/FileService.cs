using BLL.DTO;
using CsvHelper;
using DAL.IService;
using DAL.Model;
using DAL.Service;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class FileService : IFileService
    {
        private readonly IFundDALService _fundDALService;
        public FileService(IFundDALService fundDALService)
        {
            _fundDALService = fundDALService;
        }

        public async Task DownloadFileAsync(string url)
        {
            try
            {
                System.IO.DirectoryInfo newFolder = new System.IO.DirectoryInfo("DownloadedZipFiles");
                if (!newFolder.Exists)
                {
                    newFolder.Create();
                }
                using (WebClient webClient = new WebClient())
                {
                    string fileName;
                    if (url.Contains("HIST"))
                    {
                        fileName = url.Substring(60);
                    }
                    else
                    {
                        fileName = url.Substring(55);
                    }

                    string downloadToDirectory = "DownloadedZipFiles\\" + fileName;
                    await webClient.DownloadFileTaskAsync(new Uri(url), downloadToDirectory);
                }
            }
            catch (Exception e)
            {

            }
        }

        public (bool, string) InsertAllCsvRecordsIntoDB()
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo("DownloadedZipFiles\\ExtractedCsvFiles");
                FileInfo[] Files = d.GetFiles("*.csv");
                foreach (FileInfo file in Files)
                {
                    using var streamReader = File.OpenText(file.FullName);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
                    using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
                    var csvRecordsList = csvReader.GetRecords<CsvRowDTO>().ToList();
                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                    var fileRecords = csvRecordsList
                        .Select(x => new Fund() 
                        {
                            CNPJ_FUNDO = x.CNPJ_FUNDO,
                            DT_COMPTC = x.DT_COMPTC,
                            VL_TOTAL = Convert.ToDouble(x.VL_TOTAL),
                            VL_QUOTA = x.VL_QUOTA,
                            VL_PATRIM_LIQ = Convert.ToDouble(x.VL_PATRIM_LIQ),
                            CAPTC_DIA = Convert.ToDouble(x.CAPTC_DIA),
                            RESG_DIA = Convert.ToDouble(x.RESG_DIA),
                            NR_COTST = Convert.ToInt32(x.NR_COTST)
                        })
                        .ToList();
                    int rowsAffected = _fundDALService.BulkInsertToMySQL(fileRecords);
                }
                return (true, "All records were successfully inserted into the Database.");
            }
            catch (Exception e)
            {
                return (false, "Record insertion into Database failed.");
            }
        }

        public async Task<(bool, string)> DownloadMultipleFilesAsync(List<string> urls)
        {
            try
            {
                await Task.WhenAll(urls.Select(url => DownloadFileAsync(url)));
                return (true, "");
            }
            catch (Exception e)
            {
                return (false, "Error downloading files.");
            }


        }

        public async Task<(bool, string)> UnzippingAllFiles()
        {
            try
            {
                System.IO.DirectoryInfo extractPath = new System.IO.DirectoryInfo("DownloadedZipFiles\\ExtractedCsvFiles");
                if (!extractPath.Exists)
                {
                    extractPath.Create();
                }

                DirectoryInfo d = new DirectoryInfo("DownloadedZipFiles");

                FileInfo[] Files = d.GetFiles("*.zip");

                foreach (FileInfo file in Files)
                {
                    await Task.Run(() => ZipFile.ExtractToDirectory(file.FullName, extractPath.FullName, true));
                }
                return (true, "");
            }
            catch (Exception e)
            {
                return (false, "Error unzipping files.");
            }
        }

        public List<string> getFileList()
        {
            var fileList = new List<string>()
            {
                    System.Configuration.ConfigurationManager.AppSettings["urlFilesHist"]+"inf_diario_fi_2017.zip",
                    System.Configuration.ConfigurationManager.AppSettings["urlFilesHist"]+"inf_diario_fi_2018.zip",
                    System.Configuration.ConfigurationManager.AppSettings["urlFilesHist"]+"inf_diario_fi_2019.zip",
                    System.Configuration.ConfigurationManager.AppSettings["urlFilesHist"]+"inf_diario_fi_2020.zip",
            };
            for (int year = 2021; year <= 2023; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    var monthPadded = month.ToString().PadLeft(2, '0');
                    if (year == DateTime.Today.Year && month == DateTime.Today.Month)
                    {
                        fileList.Add(System.Configuration.ConfigurationManager.AppSettings["urlFiles"] + "inf_diario_fi_" + year + monthPadded + ".zip");
                        return fileList;
                    }
                    fileList.Add(System.Configuration.ConfigurationManager.AppSettings["urlFiles"] + "inf_diario_fi_" + year + monthPadded + ".zip");
                }
            }
            return fileList;
        }
    }
}
