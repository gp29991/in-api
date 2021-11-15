using in_api.Models;
using in_api.ViewModels;
using in_API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace in_api.Services
{

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;
        private readonly IHttpContextAccessor _accessor;

        public CategoryService(IRepository<Category> repository, IHttpContextAccessor accessor)
        {
            _repository = repository;
            _accessor = accessor;
        }

        public async Task<ResponseObj<Category>> AddCategory(Category category)
        {
            if (category == null)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "NullError"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (category.Username != username)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            var categories = await _repository.GetAllEntities();
            var categoriesall = categories.ToList();
            categoriesall.AddRange(DefaultCategories.defaultCategories);

            if (categoriesall.Any(cat => cat.CategoryName == category.CategoryName && (cat.Username == category.Username || cat.Username == "default")))
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "CategoryAlreadyExists"
                };
            }

            var obj =  await _repository.AddEntity(category);
            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<List<Category>>> GetAllUserCategories()
        {
            var categories = await _repository.GetAllEntities();
            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            var obj = categories.Where(cat => cat.Username == username).ToList();
            obj.AddRange(DefaultCategories.defaultCategories);
            return new ResponseObj<List<Category>>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<Category>> GetCategory(int id)
        {
            var obj = await _repository.GetEntity(id);

            if (obj == null)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (obj.Username != username)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<Category>> EditCategoryCheck(int id, Category category)
        {
            if (category == null)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "NullError"
                };
            }

            if (category.CategoryId != id)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "IdMismatch"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (category.Username != username)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            var categories = await _repository.GetAllEntities();
            var categoriesall = categories.ToList();
            categoriesall.AddRange(DefaultCategories.defaultCategories);

            if (categoriesall.Any(cat => cat.CategoryName == category.CategoryName && (cat.Username == category.Username || cat.Username == "default") && (cat.CategoryId != category.CategoryId || cat.CategoryId == 0)))
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "CategoryAlreadyExists"
                };
            }

            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success"
            };
        }


        public async Task<ResponseObj<Category>> EditCategory(int id, Category category)
        {
            var obj = await _repository.EditEntity(category);
            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<Category>> DeleteCategory(int id)
        {
            var obj = await _repository.GetEntity(id);

            if (obj == null)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (obj.Username != username)
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            await _repository.DeleteEntity(obj);

            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success",
            };
        }

        public ResponseObj<Category> CheckDefaultCategory(string defcat)
        {
            if (!DefaultCategories.defaultCategories.Any(cat => cat.CategoryName == defcat))
            {
                return new ResponseObj<Category>
                {
                    Success = false,
                    Message = "NotADefaultCategory"
                };
            }

            return new ResponseObj<Category>
            {
                Success = true,
                Message = "Success",
            };
        }
    }
}
