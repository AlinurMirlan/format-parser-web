using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FormatConverter.Web.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public DataSeeder(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();
        string adminRoleString = _config["Security:Roles:Admin"] ?? throw new InvalidOperationException("Admin Role is not set up.");
        if (!(await _userManager.GetUsersInRoleAsync(adminRoleString)).Any())
        {
            var user = new IdentityUser()
            {
                Email = "alinur.admin@gmail.com",
                UserName = "alinur.admin@gmail.com",
            };

            IdentityResult result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user while seeding database.");

            result = await _userManager.AddPasswordAsync(user, "P@ssw0rd?");
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to set password while seeding database.");

            IdentityRole role = new(adminRoleString);
            if (await _roleManager.FindByNameAsync(adminRoleString) is null)
            {
                result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create the Admin role while seeding database.");
            }

            result = await _userManager.AddToRoleAsync(user, adminRoleString);
            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to add the Admin role to the dummy account while seeding database.");
        }
    }
}
