using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.ApiControllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IMapStore Data;

        public CategoryController(IMapStore data)
        {
            Data = data;
        }

        // GET: api/values
        [HttpGet]
        public CategoryListDto Get()
        {
            var categories = Data.GetCategories().Select(c => new CategoryDto
            {
                ItemId = c.Key,
                Name = c.Value.Name
            });

            return new CategoryListDto
            {
                Categories = categories.ToList()
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CategoryDto Get(ulong id)
        {
            Category category;
            if (!Data.TryGetCategory(id, out category))
                return new CategoryDto { ErrorMessage = "Category does not existed" };

            return new CategoryDto
            {
                ItemId = id,
                Name = category.Name
            };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]CategoryDto categoryDto)
        {
            //var category = new Category
            //{
            //    Name = categoryDto.Name,
            //    ParentItemId = categoryDto.ParentItemId
            //};

            var defaultCategories = new List<Category>
            {
                new Category {Name = "Nhạc Việt", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Hàn Quốc", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Nhật", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Âu Mỹ", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Rock", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Pop", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Dance", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc Rap", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc R&B", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc trẻ", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc trữ tình", CategoryType = CategoryType.Music},
                new Category {Name = "Nhạc hòa tấu", CategoryType = CategoryType.Music},

                new Category {Name = "Phim Việt", CategoryType = CategoryType.Film},
                new Category {Name = "Phim Hàn Quốc", CategoryType = CategoryType.Film},
                new Category {Name = "Phim Hồng Kông", CategoryType = CategoryType.Film},
                new Category {Name = "Phim Mỹ", CategoryType = CategoryType.Film},
                new Category {Name = "Phim tình cảm", CategoryType = CategoryType.Film},
                new Category {Name = "Phim hành động", CategoryType = CategoryType.Film},
                new Category {Name = "Phim kinh dị", CategoryType = CategoryType.Film},
                new Category {Name = "Phim viễn tưởng", CategoryType = CategoryType.Film},
                new Category {Name = "Phim hài", CategoryType = CategoryType.Film},
                new Category {Name = "Phim hoạt hình", CategoryType = CategoryType.Film},
                new Category {Name = "Phim TVB", CategoryType = CategoryType.Film},
            };

            foreach (var defaultCategory in defaultCategories)
            {
                Data.StoreCategory(0, defaultCategory);
            }

            Data.Commit();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(ulong id)
        {
            Data.RemoveCategory(id);
        }
    }
}
