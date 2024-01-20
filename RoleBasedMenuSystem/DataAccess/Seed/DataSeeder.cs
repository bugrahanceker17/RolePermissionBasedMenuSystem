using System.Reflection;
using RoleBasedMenuSystem.DataAccess.Concrete;

namespace RoleBasedMenuSystem.DataAccess.Seed;

public static class DataSeeder
{
    public static void PermissionSeedOnRun(this ProjectDbContext dbContext)
    {
        PermissionSeed seed = new PermissionSeed();
        
        Type type = typeof(PermissionsCode);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        if (seed.PermissionList.Count != fields.Length)
            throw new Exception("PermissionSeed verileri arasında tanımlanmayan yetki kodları mevcut!");
        
        List<string> allPermissionCode = dbContext.Permissions.ToList().Select(c => c.Code).ToList();

        List<string> seedPermissionListCode = seed.PermissionList.Select(c => c.Code).ToList();

        List<string> differenceValues = seedPermissionListCode.Except(allPermissionCode).ToList();

        foreach (var data in seed.PermissionList.Where(c => differenceValues.Contains(c.Code)))
        {
            dbContext.Permissions.Add(data);
            Console.WriteLine($"Permission with code : [{data.Code}] is created!");
        }

        dbContext.SaveChanges();
    }
}