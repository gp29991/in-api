using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Models
{
    public static class DefaultCategories
    {
        public static readonly List<Category> defaultCategories = new List<Category>()
        {
            new Category{ CategoryId = 0, CategoryName = "Zakupy", Username = "default" },
            new Category{ CategoryId = 0, CategoryName = "Samochód", Username = "default" },
            new Category{ CategoryId = 0, CategoryName = "Dom", Username = "default" },
            new Category{ CategoryId = 0, CategoryName = "Hobby", Username = "default" },
            new Category{ CategoryId = 0, CategoryName = "Praca", Username = "default" }
        };
    }
}
