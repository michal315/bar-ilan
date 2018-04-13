

using WindowsService5.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.String;
using WindowsService5;
using System.Configuration;


namespace WindowsService5
{
    class ImageServer
    {
        private IImageController controller;
        private ILoggingService logging;
        private Dictionary<string, DirectoryHandler> handlers;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.handlers = new Dictionary<string, DirectoryHandler>();
            this.logging = logging;
            this.controller = controller;
            createHandlers();
        }

        public void createHandlers()
        {
            string handlersList = ConfigurationManager.AppSettings["Handlers"];
            string[] path = handlersList.Split(';');
            foreach (string onePath in path)
            {
                DirectoryHandler dHandler = new DirectoryHandler(onePath, controller, logging);
                handlers.Add(onePath, dHandler);
                CommandRecieved += dHandler.OnCommandRecieved;          // when server close.
                dHandler.DirectoryClose += OnHandlerClose;              // when handler close.
            }
        }

        public void closeServer()
        {
            string[] args = new string[2];
            args[0] = "close";
            foreach (KeyValuePair<string, DirectoryHandler> handler in handlers)
            {
                CommandRecievedEventArgs newCommand = new CommandRecievedEventArgs(0, args, handler.Key);
                this.CommandRecieved.Invoke(this, newCommand);
            }
        }

        public void sendCommand()
        {
            string[] args = new string[2];
            args[0] = "command";
            foreach (KeyValuePair<string, DirectoryHandler> handler in handlers)
            {
                CommandRecievedEventArgs newCommand = new CommandRecievedEventArgs(0, args, handler.Key);
                this.CommandRecieved.Invoke(this, newCommand);
            }
        }

        public void OnHandlerClose(object sender, DirectoryCloseEventArgs e)
        {
            foreach (KeyValuePair<string, DirectoryHandler> handler in handlers)
            {
                if (handler.Key == e.DirectoryPath)
                {
                    handlers.Remove(handler.Key);
                }
            }
        }
    }
}

