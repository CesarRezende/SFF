using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Authentication;
using DryIoc;
using Infra.IoC;
using SFF.Infra.Core.Repository;
using SFF.Infra.Core.Helper;

namespace SFF.Infra.Web.Startup
{
    public static partial class StartupExtensions
    {
        private static IConfiguration _configuration;

        public static IContainer AddDbConfigurations(this IContainer container, IConfiguration configuration)
        {
            _configuration = configuration;

            var conn = _configuration.GetConnectionString("DefaultConnection");

            var dbContextOptions = new DbContextOptionsBuilder()
                .UseNpgsql(conn, o => { o.CommandTimeout(1000); })
                .Options;

            container.UseInstance(dbContextOptions);
            container.UseInstance(_configuration);

            using var scope = container.OpenScope(Reuse.Scoped);

            ContainerManager.SetContainer(container);

            return container;
        }

        public static IContainer AddDefaults(this IContainer container)
        {
            container = container.AddAssembly();

            container.Register<IUnitOfWorkWithTransactionScope, UnitOfWorkWithTransactionScope>(reuse: Reuse.Scoped, ifAlreadyRegistered: IfAlreadyRegistered.Replace);

            //container.Register(typeof(IValidator<>), reuse: Reuse.Singleton, made: Made.Of(factoryMethod: FactoryMethod.ConstructorWithResolvableArguments));

            ContainerManager.SetContainer(container);

            return container;

        }

        private static IContainer AddAssembly(this IContainer container)
        {
            var fullnames = new[] { "SFF" };
            var namespaces = new[] { "SFF" };

            var cConventionsEnds = new[] { "Context" };
            var iConventionsEnds = new[] { "Repository", "Service", "Handler", "ModelBinder", "Factory", "Dispatcher", "Validator" };
            var sConventionsEnds = new[] { "HttpClientService", "AuthService", "HttpRestClientService" };

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWithAny(fullnames)).OrderBy(p => p.FullName).ToList();
            var classes = assemblies.SelectMany(x => x.GetTypes()).Where(y => y.Namespace.StartsWithAny(namespaces) && y.IsPublic && y.IsClass && !y.IsAbstract && y.GetInterfaces().Any()).ToList();

            var cTypes = classes.Where(x => x.Name.EndsWithAny(cConventionsEnds)).OrderBy(p => p.Namespace).ToList();
            var iTypes = classes.Where(x => x.Name.EndsWithAny(iConventionsEnds)).OrderBy(p => p.Namespace).ToList();
            var sTypes = classes.Where(x => x.Name.EndsWithAny(sConventionsEnds)).OrderBy(p => p.Namespace).ToList();

            container.RegisterMany(cTypes, serviceTypeCondition: type => type.IsClass, reuse: Reuse.Scoped);
            container.RegisterMany(iTypes, serviceTypeCondition: type => type.IsInterface, reuse: Reuse.Scoped);
            container.RegisterMany(sTypes, reuse: Reuse.Singleton);

            return container;
        }



        public static IContainer AddLocals(this IContainer container, Action<IContainer> action)
        {
            action.Invoke(container);

            ContainerManager.SetContainer(container);

            return container;
        }

    }
}
