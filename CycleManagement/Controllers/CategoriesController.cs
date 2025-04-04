using CycleManagement.Data;
using CycleManagement.Models;

namespace CycleManagement.Controllers
{
    public class CategoriesController : BaseController<Category>
    {
        public CategoriesController(ApplicationDbContext context) : base(context)
        {
        }
    }
}
