using in_api.Models;
using in_api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Services
{
    public interface ICategoryService
    {
        Task<ResponseObj<Category>> AddCategory(Category category);

        Task<ResponseObj<List<Category>>> GetAllUserCategories();

        Task<ResponseObj<Category>> GetCategory(int id);

        Task<ResponseObj<Category>> EditCategoryCheck(int id, Category category);

        Task<ResponseObj<Category>> EditCategory(int id, Category category);

        Task<ResponseObj<Category>> DeleteCategory(int id);

        ResponseObj<Category> CheckDefaultCategory(string defcat);

    }
}
