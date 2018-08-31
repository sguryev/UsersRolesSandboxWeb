using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UsersRolesSandboxWeb.Pages
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Model;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGet()
        {
            UserModels = await _dbContext.Users
                .Include(u => u.Agency)
                .Include(u => u.UserGroups)
                .Select(u => new UserModel(u))
                .ToArrayAsync();

            Groups = await _dbContext.Groups.ToArrayAsync();

        }

        public Group[] Groups { get; set; }

        public UserModel[] UserModels { get; set; }
    }
}
