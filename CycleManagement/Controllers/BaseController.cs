using CycleManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CycleManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity> : ControllerBase where TEntity : class

    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> GetById([FromRoute] int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return NotFound();
            return entity;
        }


        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            var entityId = _context.Entry(entity).Property("Id").CurrentValue; // Get generated ID

            return CreatedAtAction(nameof(GetById), new { id = entityId }, entity);
        }


        //[HttpPut("{id}")]
        //public virtual async Task<IActionResult> Update([FromRoute] int id, [FromBody] TEntity entity)
        //{
        //    if (id != GetEntityId(entity))
        //        return BadRequest();

        //    _context.Entry(entity).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!await EntityExists(id))
        //            return NotFound();
        //        else
        //            throw;
        //    }
        //    return NoContent();
        //}


        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return NotFound();

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //protected Guid GetEntityId(TEntity entity) => entity['Id'];

        protected virtual async Task<bool> EntityExists(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
