using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication3.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication3.Startup))]
namespace WebApplication3
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUser();
        }

        public void CreateDefaultRolesAndUser()
        {
            //Default Role
            var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManger.RoleExists("Admin"))
            {
                role.Name = "Admin";
                roleManger.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Aymankoo";
                user.Email = "ayman123@yahoo.com";
                var Check = userManger.Create(user, "ayman123@yahoo.com");
                if (Check.Succeeded)
                {
                    userManger.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}

