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
    /// <summary>
    /// the main context of the plupload component
    /// PluploadContex is the central(singleton) context, wich
    /// contains the common funtionality. For example the configuration of plupload.net.
    /// Use the static Instance property to get the singleton instance of the PluploadContext
    /// </summary>
    public class PluploadContext
    {
        private static PluploadContext _instance = null;
        private static object lockThis = new object();

        /// <summary>
        /// creates a new context. 
        /// loads all configuration files.
        /// </summary>
        private PluploadContext() 
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

        /// <summary>
        /// sets the instancesspecific configuration.
        /// this configuration will be merged with the application and global configuration.
        /// the merged configuration will be stored to the current session.
        /// </summary>
        /// <param name="configuration"></param>
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

        /// <summary>
        /// gets or sets the custom-/applicationspecific configuration
        /// </summary>
        private PluploadConfiguration CustomConfiguration { get; set; }
        
        /// <summary>
        /// gets or sets the global/embedded configuration
        /// </summary>
        private PluploadConfiguration GlobalEmbedConfiguration { get; set; }
        
        /// <summary>
        /// gets or sets the merged sessionspecific configuration
        /// </summary>
        private PluploadConfiguration MergedConfiguration { get; set; }
        

        /// <summary>
        /// gets the singleton instance of the PluploadContext. If the Instance not exists a new one will be created.
        /// </summary>
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

        /// <summary>
        /// merges the configuration given by config witch the current merged configuration.
        /// </summary>
        /// <param name="config">the configuration to be merged</param>
        /// <returns>a merged result a new PluploadConfiguration instance</returns>
        private PluploadConfiguration Merge(PluploadConfiguration config) 
        {
            return this.MergedConfiguration.Merge(config);
        }
    }
}
