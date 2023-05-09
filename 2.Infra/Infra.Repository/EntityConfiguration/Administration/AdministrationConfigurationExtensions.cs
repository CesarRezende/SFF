using Microsoft.EntityFrameworkCore;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public static class AdministrationConfigurationExtensions
    {
        public static void RegisterAdministrationDbConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserDbConfiguration());

        }
    }
}
