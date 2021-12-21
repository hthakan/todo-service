using Autofac;
using Microsoft.Extensions.Logging;


namespace todo.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }
        ILoggerFactory _logger;
        public ApplicationModule(string qconstr, ILoggerFactory logger)
        {
            QueriesConnectionString = qconstr;
            _logger = logger;
        }

        protected override void Load(ContainerBuilder builder)
        {

            //builder.RegisterType<OrderOperations>()
            // .As<IOrderOperations>()
            // .InstancePerLifetimeScope();

        }
    }
}
