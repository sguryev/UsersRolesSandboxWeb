using Microsoft.AspNetCore.Mvc;

namespace UsersRolesSandboxWeb.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Model;
    using Microsoft.EntityFrameworkCore;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Groups([FromBody]UserGroupsPostModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _dbContext.Users
                .Include(u => u.UserGroups)
                .SingleOrDefaultAsync(u => u.Id == model.UserId);

            if (entity.UserGroups.Any())
            {
                _dbContext.RemoveRange(entity.UserGroups);
            }

            _dbContext.UserGroups.AddRange(model.GroupIds.Select(id => new UserGroup {GroupId = id, UserId = model.UserId}));

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}