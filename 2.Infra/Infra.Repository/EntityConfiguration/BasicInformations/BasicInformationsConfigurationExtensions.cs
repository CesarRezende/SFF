using Microsoft.EntityFrameworkCore;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public static class BasicInformationsConfigurationExtensions
    {
        public static void RegisterBasicInformationsDbConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FamilyDbConfiguration());

        }
    }
}
