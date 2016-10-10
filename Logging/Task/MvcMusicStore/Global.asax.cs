using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using MvcMusicStore.Controllers;
using NLog;
using LogManager = NLog.LogManager;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
	    private readonly ILog _logger;

	    public MvcApplication()
	    {
		    _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	    }
        protected void Application_Start()
        {
			log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

			var builder = new ContainerBuilder();
	        builder.RegisterControllers(typeof(AccountController).Assembly);
	        //builder.Register(f => LogManager.GetLogger("ForControllers")).As<ILogger>();
	        builder.Register(
		        f => log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)).As<ILog>();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			_logger.Info("Application started");
        }

	    protected void Application_Error()
	    {
		    var ex = Server.GetLastError();
			_logger.Error(ex.Message);
	    }
    }
}
