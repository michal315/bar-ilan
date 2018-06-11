using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ImageService;

namespace MyService.ImageService.Commands
{
    class GetConfigCommand : ICommand {

        private bool notChange = false;
        private string Handler;
        private string SourceName;
        private string OutputDir;
        private string LogName;
        private string ThumbnailSize;

        public GetConfigCommand()
        {
        }

        /**
         * return a string that includes all app config data by convention.
         **/
        public string Execute(string[] args, out bool result)
        {
            if (!notChange) {
                initialization();
                notChange = true;
            }
            result = true;
            return "config#" +  OutputDir + '#' + SourceName + '#' + LogName + '#' + ThumbnailSize + '#' + Handler;
        }

        /**
         * The function will activats once during the run of the service, 
         * and will exhaust the data from the appconfig to the matching members.
         **/
        private void initialization() {
            Handler = ConfigurationManager.AppSettings["Handler"];
            SourceName = ConfigurationManager.AppSettings["SourceName"];
            OutputDir = ConfigurationManager.AppSettings["OutputDir"];
            LogName = ConfigurationManager.AppSettings["LogName"];
            ThumbnailSize = ConfigurationManager.AppSettings["ThumbnailSize"];
        }

        /**
         *  when an closeDirectory event happens this function activates
         *  and remove from the string of handlers the dir that closed.
         **/
        public void removeHandler(object sender, string dir)
        {
            string[] handlers = Handler.Split(';');
            string newHandler = "";
            for (int i = 0; i < handlers.Length; i++)
            {
                if(handlers[i] == dir)
                {
                    continue;
                }
                newHandler += handlers[i];
                newHandler += ";";
            }
            Handler = newHandler;
            Handler = Handler.Remove(Handler.Length - 1);
        }
    }
}
