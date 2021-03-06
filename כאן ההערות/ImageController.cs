﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.ImageService;
using ImageService.Logging.Modal;

namespace ImageService
{
    class ImageController : IImageController
    {

        private IImageServiceModal imageModal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal) {
            imageModal = modal;                                    // Storing the Modal Of The System
            
            // set commands dictionary.
            commands = new Dictionary<int, ICommand>();
            ICommand newFile = new NewFileCommand(imageModal); //  command for adding a file in dirtcory
            commands.Add(0, newFile); 
        }
        /**
         * The functio ExecuteCommandn accepts parameters comaand (int) agrs (string)
         * resultSuccesful (boolan)
         * The function triggers the corresponding command according to the ID
         **/
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful) {
               return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}

      