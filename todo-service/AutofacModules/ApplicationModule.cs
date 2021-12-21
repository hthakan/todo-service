using Autofac;
using ErrorHandler;
using Microsoft.Extensions.Logging;
using MongoDBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_service.OrderService;

namespace todo_service.AutofacModules
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

            builder.RegisterType<OrderOperations>()
             .As<IOrderOperations>()
             .InstancePerLifetimeScope();

        }
    }
}
