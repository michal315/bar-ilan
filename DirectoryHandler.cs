using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using System.Security.Permissions;


namespace ImageService
{
     public class DirectoryHandler : IDirectoryHandler
    {
        private IImageController controller;              // The Image Processing Controller
        private ILoggingService logging;
        private FileSystemWatcher dirWatcher;             // The Watcher of the Dir
        private string path;                              // The Path of directory
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoryHandler(string path, IImageController controller, ILoggingService logging) {
            this.path = path;
            this.logging = logging;
            this.controller = controller;
            this.dirWatcher = new FileSystemWatcher();
            dirWatcher.Path = path;
            // Only watch this files:
            dirWatcher.Filter = "*.jpg";
            dirWatcher.Filter = "*.bmp";
            dirWatcher.Filter = "*.png";
            dirWatcher.Filter = "*.gif";
            dirWatcher.Created += new FileSystemEventHandler(OnCreated);
            dirWatcher.Deleted += new FileSystemEventHandler(OnClosed);
            dirWatcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object source, FileSystemEventArgs e) {
            string[] args = new string[2];
            args[0] = e.FullPath;
            args[0] = e.Name;
            bool result;
            string message = controller.ExecuteCommand(0, args, out result);
            if (result) {
                this.logging.Log(message, MessageTypeEnum.INFO);
            } else {
                this.logging.Log(message, MessageTypeEnum.FAIL);
            }
        }

        private void OnClosed(object source, FileSystemEventArgs e) {
            DirectoryCloseEventArgs dirClose = new DirectoryCloseEventArgs(this.path, "close");
            DirectoryClose.Invoke(this, dirClose);
        }


        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e) {
            if (e.CommandID == 0) {
                try {
                    dirWatcher.EnableRaisingEvents = false;
                    this.logging.Log(path+" closed", MessageTypeEnum.INFO);
                }
                catch (Exception exception) {
                    this.logging.Log("close", MessageTypeEnum.FAIL);
                }
            }
        }
		
    }
}
