using in_api.Models;
using in_api.Services;
using in_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace in_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFinancialDataService _dataService;

        public CategoryController(ICategoryService categoryService, IFinancialDataService dataService)
        {
            _categoryService = categoryService;
            _dataService = dataService;
        }

        [HttpPost("add")] // /api/category/add
        public async Task<IActionResult> AddCategory(Category category)
        {
            var result = await _categoryService.AddCategory(category);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getall")] // /api/category/getall
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetAllUserCategories();
            return Ok(result);
        }

        [HttpGet("get/{id}")] //api/category/get/(id)
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _categoryService.GetCategory(id);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("edit/{id}")] //api/category/edit/(id)
        public async Task<IActionResult> EditCategory(int id, Category categoryToEdit)
        { 
            var categoryForEditing = await _categoryService.GetCategory(id);

            if (!categoryForEditing.Success)
            {
                return BadRequest(categoryForEditing);
            }

            var categoryCheckResult = await _categoryService.EditCategoryCheck(id, categoryToEdit);

            if (!categoryCheckResult.Success)
            {
                return BadRequest(categoryCheckResult);
            }

            var data = await _dataService.GetAllUserData();
            IEnumerable<FinancialData> dataForEditing = data.Obj.Where(data => data.CategoryName == categoryForEditing.Obj.CategoryName).Select(data => { data.CategoryName = categoryToEdit.CategoryName; return data; });

            var dataEditingResult = _dataService.MarkDataForEditing(dataForEditing);

            if (!dataEditingResult.Success)
            {
                return BadRequest(new ResponseObj<Category>
                {
                    Success = false,
                    Message = "FailedToeditData"
                });
            }

            var result = await _categoryService.EditCategory(id, categoryToEdit);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")] //api/category/delete/(id)
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryForDeletion = await _categoryService.GetCategory(id);

             if (!categoryForDeletion.Success)
             {
                 return BadRequest(categoryForDeletion);
             }

             var data = await _dataService.GetAllUserData();
             IEnumerable<FinancialData> dataForDeletion = data.Obj.Where(data => data.CategoryName == categoryForDeletion.Obj.CategoryName);

             var dataDeletionResult = _dataService.MarkDataForDeletion(dataForDeletion);

             if(!dataDeletionResult.Success)
             {
                return BadRequest(new ResponseObj<Category>
                {
                    Success = false,
                    Message = "FailedToDeleteData"
                });
             }

             var result = await _categoryService.DeleteCategory(id);

             if (!result.Success)
             {
                 return BadRequest(result);
             }

             return Ok(result);
        }

        [HttpDelete("clear/{defcat}")] //api/category/clear/(defcat)
        public async Task<IActionResult> ClearDefaultCategoryEntries(string defcat)
        {
            var categoryCheckResult = _categoryService.CheckDefaultCategory(defcat);

            if (!categoryCheckResult.Success)
            {
                return BadRequest(categoryCheckResult);
            }

            var data = await _dataService.GetAllUserData();
            IEnumerable<FinancialData> dataForDeletion = data.Obj.Where(data => data.CategoryName == defcat);

            var dataDeletionResult = _dataService.MarkDataForDeletion(dataForDeletion);

            if (!dataDeletionResult.Success)
            {
                return BadRequest(new ResponseObj<Category>
                {
                    Success = false,
                    Message = "FailedToDeleteData"
                });
            }

            await _dataService.SaveData();

            return Ok(new ResponseObj<Category>
            {
                Success = true,
                Message = "Success"
            });
        }
    }
}
