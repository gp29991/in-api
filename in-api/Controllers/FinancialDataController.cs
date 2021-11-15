using in_api.Models;
using in_api.Services;
using in_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FinancialDataController : ControllerBase
    {
        private readonly IFinancialDataService _dataService;

        public FinancialDataController(IFinancialDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("add")] // /api/financialdata/add
        public async Task<IActionResult> AddData(FinancialData data)
        {
            var result = await _dataService.AddData(data);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getall")] // /api/financialdata/getall
        public async Task<IActionResult> GetAllData(DateTime? startDate = null, DateTime? endDate = null, string scope = "all", int minamount = 0, bool aggregate = false, bool total = false)
        {
            var result = await _dataService.GetAllUserData();
            
            IEnumerable<FinancialData> temp = result.Obj;
            
            if(startDate != null && endDate != null)
            {
                temp = temp.Where(data => data.Date >= startDate && data.Date <= endDate);
            }

            if(scope == "exp")
            {
                temp = temp.Where(data => data.Amount < 0);
            }

            if(scope == "inc")
            {
                temp = temp.Where(data => data.Amount >= 0);
            }

            if(minamount > 0)
            {
                minamount *= -1;
                temp = temp.Where(data => data.Amount <= minamount);
            }

            if (aggregate)
            {
                List<FinancialDataGrouped> aggregateTemp = temp
                    .GroupBy(data => data.CategoryName)
                    .Select(agg => new FinancialDataGrouped
                    {
                        Amount = agg.Sum(a => a.Amount),
                        CategoryName = agg.First().CategoryName
                    }).ToList();

                var aggregateResult = new ResponseObj<List<FinancialDataGrouped>> {
                    Success = true,
                    Message = "Success",
                    Obj = aggregateTemp
                };
                
                return Ok(aggregateResult);
            }

            if (total)
            {
                var incsum = temp.Where(data => data.Amount >= 0).Sum(data => data.Amount);
                var expsum = temp.Where(data => data.Amount < 0).Sum(data => data.Amount);
                var totals = new Dictionary<string, decimal>
                {
                    { "incsum", incsum },
                    { "expsum", expsum }
                };

                var totalResult = new ResponseObj<Dictionary<string, decimal>>
                {
                    Success = true,
                    Message = "Success",
                    Obj = totals
                };

                return Ok(totalResult);
            }
            
            result.Obj = temp.ToList();

            return Ok(result);
        }

        [HttpGet("get/{id}")] //api/financialdata/get/(id)
        public async Task<IActionResult> GetData(int id)
        {
            var result = await _dataService.GetData(id);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("edit/{id}")] //api/financialdata/edit/(id)
        public async Task<IActionResult> EditData(int id, FinancialData dataToEdit)
        { 
            var result = await _dataService.GetData(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            result = await _dataService.EditData(id, dataToEdit);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("delete/{id}")] //api/financialdata/delete/(id)
        public async Task<IActionResult> DeleteData(int id)
        {
            var result = await _dataService.DeleteData(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
