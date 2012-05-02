using System;     
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plupload.Net.Model;
using Plupload.Net.Utils;
using System.Xml;
using System.Web;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.IO; 

namespace Plupload.Net
{
    public class PluploadContext
    {
        private static PluploadContext _instance = null;
        private static object lockThis = new object();

        public PluploadContext() 
        {
            this.CustomConfiguration = XMLSerializer<PluploadConfiguration>.Load();

            string config = RessourceHelper.GetTextResource(PluploadConstants.CONFIGURATION_MAIN);
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(config);
            this.GlobalEmbedConfiguration = XMLSerializer<PluploadConfiguration>.Load(xdoc);

            this.MergedConfiguration = (this.CustomConfiguration != null) ? this.GlobalEmbedConfiguration.Merge(this.CustomConfiguration) : this.GlobalEmbedConfiguration;

            this.Initialize();
        }

        private void Initialize() 
        {
            //LogWriter.Debug("register VirtualPathProvider...");
            HostingEnvironment.RegisterVirtualPathProvider(new SVirtualPathProvider());
            //LogWriter.Debug("VirtualPathProvider successfull registered.");
        }

        public void SetConfiguration(PluploadConfiguration configuration)
        {
            if (configuration != null)
                HttpContext.Current.Session[PluploadConstants.REQUEST_CONFIGURATION] = this.Merge(configuration);
        }

        /// <summary>
        /// gets the merged plupload configuration. 
        /// There are many levels(default => application => instance), 
        /// where you can configure the plupload.net. 
        /// this instance of configuration is merged. You should always working
        /// with the merged configuration.
        /// </summary>
        public PluploadConfiguration GetConfiguration() 
        {
            PluploadConfiguration config = HttpContext.Current.Session[PluploadConstants.REQUEST_CONFIGURATION] as PluploadConfiguration;

            if (config == null) return this.MergedConfiguration;

            return config;
        }

        private PluploadConfiguration CustomConfiguration { get; set; }
        
        private PluploadConfiguration GlobalEmbedConfiguration { get; set; }
        
        private PluploadConfiguration MergedConfiguration { get; set; }
        

        public static PluploadContext Instance 
        {
            get 
            {
                if (_instance == null) 
                {
                    lock (lockThis) 
                    {
                        if (_instance == null) 
                        {
                            _instance = new PluploadContext();
                            _instance.CheckState();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// checks the state of plupload context!!!
        /// the debug informations will be written only if setting Debug set to tru within custom application configuration!!!
        /// Default config has Debug=false and custom instance configuration was not loaded at this time!
        /// </summary>
        private void CheckState()
        {
            LogWriter.Debug("plupload context initialized:");

            LogWriter.Debug(String.Format("embedded configuration: {0}", this.GlobalEmbedConfiguration));
            
            if (this.CustomConfiguration != null)
                LogWriter.Debug(String.Format("custom application configuration: {0}", this.CustomConfiguration));
            else
                LogWriter.Debug("no custom application configuration available.");

            LogWriter.Debug(String.Format("merged configuration: {0}", this.MergedConfiguration));
        }

        private PluploadConfiguration Merge(PluploadConfiguration config) 
        {
            return this.MergedConfiguration.Merge(config);
        }
    }
}
