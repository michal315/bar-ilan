using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsService5
using ImageService.Logging.Modal;

namespace ImageService
{
    class ImageController : IImageController
    {
        private IImageServiceModal imageModal;                
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {

            // set commands dictionary.
            commands = new Dictionary<int, ICommand>();
            ICommand newFile = new NewFileCommand(imageModal);
            commands.Add(0, newFile);
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}

