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

    public class FinancialDataService : IFinancialDataService
    {
        private readonly IRepository<FinancialData> _repository;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _accessor;

        public FinancialDataService(IRepository<FinancialData> repository, ICategoryService categoryService, IHttpContextAccessor accessor)
        {
            _repository = repository;
            _categoryService = categoryService;
            _accessor = accessor;
        }

        public async Task<ResponseObj<FinancialData>> AddData(FinancialData data)
        {
            if (data == null)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "NullError"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (data.Username != username)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            var categories = await _categoryService.GetAllUserCategories();

            if (!categories.Obj.Any(cat => cat.CategoryName == data.CategoryName))
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "CategoryError"
                };
            }

            var obj =  await _repository.AddEntity(data);
            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<List<FinancialData>>> GetAllUserData()
        {
            var data = await _repository.GetAllEntities();
            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            var obj = data.Where(data => data.Username == username).ToList();
            return new ResponseObj<List<FinancialData>>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<FinancialData>> GetData(int id)
        {
            var obj = await _repository.GetEntity(id);

            if (obj == null)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (obj.Username != username)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<FinancialData>> EditData(int id, FinancialData data)
        {
            if (data == null)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "NullError"
                };
            }

            if(data.FinancialDataId != id)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "IdMismatch"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (data.Username != username)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            var obj = await _repository.EditEntity(data);
            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
                Obj = obj
            };
        }

        public async Task<ResponseObj<FinancialData>> DeleteData(int id)
        {
            var obj = await _repository.GetEntity(id);

            if (obj == null)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var username = _accessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

            if (obj.Username != username)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "UsernameMismatch"
                };
            }

            await _repository.DeleteEntity(obj);

            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
            };
        }

        public ResponseObj<FinancialData> MarkDataForEditing(IEnumerable<FinancialData> data)
        {
            try
            {
                _repository.MarkEntitiesForUpdate(data);
            }
            catch (Exception)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "ServerError",
                };
            }

            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
            };
        }

        public ResponseObj<FinancialData> MarkDataForDeletion(IEnumerable<FinancialData> data)
        {
            try
            {
                _repository.MarkEntitiesForDeletion(data);
            }
            catch (Exception)
            {
                return new ResponseObj<FinancialData>
                {
                    Success = false,
                    Message = "ServerError",
                };
            }

            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
            };
        }

        public async Task<ResponseObj<FinancialData>> SaveData()
        {
            await _repository.SaveEntities();

            return new ResponseObj<FinancialData>
            {
                Success = true,
                Message = "Success",
            };
        }
    }
}
