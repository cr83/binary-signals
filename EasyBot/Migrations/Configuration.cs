namespace EasyBot.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using EasyBot.Models;

    // Run Update-Database from package manager console
    internal sealed class Configuration : DbMigrationsConfiguration<EasyBot.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "EasyBot.Models.ApplicationDbContext";
        }

        const string admin = "admin";
        const string pwd = "!QAZ1qaz";

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == Common.Constants.Security.Roles.Admin))
            {
                CreateRole(context, Common.Constants.Security.Roles.Admin);
            }
            if (!context.Roles.Any(r => r.Name == Common.Constants.Security.Roles.User))
            {
                CreateRole(context, Common.Constants.Security.Roles.User);
            }

            if (!context.Users.Any(u => u.UserName == admin))
            {
                CreateUser(context, admin, pwd, Common.Constants.Security.Roles.Admin);
            }

            base.Seed(context);
        }

        private void CreateRole(ApplicationDbContext context, string roleName)
        {
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole { Name = roleName };

            manager.Create(role);
        }

        private void CreateUser(ApplicationDbContext context, string userName, string password, string roleName)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = new ApplicationUser { UserName = userName };

            manager.Create(user, password);
            manager.AddToRole(user.Id, roleName);
        }
    }
}
