using CycleManagement.Data;
using CycleManagement.DTO.ProductDTO;
using CycleManagement.Models;
using CycleManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CycleManagement.Controllers
{
    public class ProductsController : BaseController<Product>
    {
        ImageServerService _imageService;
        public ProductsController(ApplicationDbContext context, ImageServerService imageService) : base(context)
        {
            _imageService = imageService;
        }

        [NonAction]
        public override Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return base.GetAll();

        }

        [NonAction]
        public override Task<ActionResult<Product>> Create([FromBody] Product entity)
        {
            return base.Create(entity);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(IFormFile photo, 
            [FromForm] string name, 
            [FromForm] string price, 
            [FromForm] string brand, 
            [FromForm] string modelNumber, 
            [FromForm] Guid categoryId)
        {
            //Console.WriteLine(request);
            string imageUrl = await _imageService.uploadImage(photo, "products");

            Product entity = new Product();

            entity.PhotoURL = imageUrl;
            entity.Name = name;
            entity.Brand = brand;
            entity.ModelNumber = modelNumber;
            entity.CategoryId = categoryId;
            entity.Price = int.Parse(price);


            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll([FromQuery] ProductQueryParameters parameters)
        {
            IQueryable<Product> query = _dbSet.AsQueryable();

            if(!string.IsNullOrEmpty(parameters.Category))
            {
                Category categoryEntity = await _context.Set<Category>().FirstOrDefaultAsync(cat => cat.Name == parameters.Category);

                if (categoryEntity == null)
                    return BadRequest("Invalid category");

                query = query.Where(product => product.CategoryId == categoryEntity.Id);
            }
     


            if (parameters.MaxPrice.HasValue)
                query = query.Where(product => (product.Price <= parameters.MaxPrice));

            if (parameters.MinPrice.HasValue)
                query = query.Where(product => product.Price >= parameters.MinPrice);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(product => product.Name.Contains(parameters.Search));

            IEnumerable<Product> result = await query.ToListAsync();

            return Ok(result);

        }
    }
}
