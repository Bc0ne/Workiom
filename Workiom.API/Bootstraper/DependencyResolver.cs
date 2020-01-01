namespace Workiom.API.Bootstraper
{
    using Autofac;
    using Microsoft.AspNetCore.Hosting;
    using Workiom.Data.Context;
    using Workiom.Data.Repositories;

    public class DependencyResolver : Module
    {
        private readonly IHostingEnvironment _env;

        public DependencyResolver(IHostingEnvironment env)
        {
            _env = env;
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadModules(builder);
        }

        private void LoadModules(ContainerBuilder builder)
        {
            builder.RegisterType<WorkiomDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ContactRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
