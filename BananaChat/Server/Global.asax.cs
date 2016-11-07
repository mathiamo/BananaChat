using System;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Chats;
using WebApiContrib.Formatting.Jsonp;

namespace Server
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(configuration =>
                                          {
                                              configuration.AddJsonpFormatter();
                                              configuration.MapHttpAttributeRoutes();
                                              configuration.EnsureInitialized();

                                              var formatters = configuration.Formatters;
                                              var jsonFormatter = formatters.JsonFormatter;
                                              var settings = jsonFormatter.SerializerSettings;
                                              settings.Formatting = Formatting.Indented;
                                              settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                          });

            ChatContext.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            ChatContext.End();
        }
    }
}