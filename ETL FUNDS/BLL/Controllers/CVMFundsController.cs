using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using BLL.Services;
using DAL.IService;
using DAL.Service;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVMFundsController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFundDALService _fundDALService;
        public CVMFundsController(IFileService fileService, IFundDALService fundDALService)
        {
            _fileService = fileService;
            _fundDALService = fundDALService;
        }

        [HttpGet("FetchDataAndLoadDatabase")]
        public async Task<ActionResult> FetchDataAndLoadDatabase()
        {
            try
            {
                List<string> fileList = _fileService.getFileList();

                var downloadResponse = await _fileService.DownloadMultipleFilesAsync(fileList);
                if (!downloadResponse.Item1)
                {
                    return BadRequest(downloadResponse.Item2);
                }

                var unzipResponse = await _fileService.UnzippingAllFiles();
                if (!unzipResponse.Item1)
                {
                    return BadRequest(unzipResponse.Item2);
                }

                var response = _fileService.InsertAllCsvRecordsIntoDB();
                if (response.Item1)
                {
                    return Ok(response.Item2);
                }
                else
                {
                    return BadRequest(response.Item2);
                }
            }
            catch (Exception e)
            {
                return BadRequest("Error in controller.");
            }
        }

        [HttpGet("GetRecords")]
        public ActionResult<List<FundDALService>> Get(string CNPJ_FUNDO, DateTime startDate, DateTime endDate)
        {
            (var selectedRecords, var errorMessage) = _fundDALService.SelectRecords(CNPJ_FUNDO, startDate, endDate);
            if (selectedRecords.Count > 0)
            {
                return Ok(selectedRecords);
            }else if (!String.IsNullOrEmpty(errorMessage))
            {
                return BadRequest(errorMessage);
            }
            else
            {
                return BadRequest("Error while selecting records from Database.");
            }
        }
    }
}
