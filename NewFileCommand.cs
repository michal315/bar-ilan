using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ImageService.Infrastructure;


namespace ImageService {
    class NewFileCommand : ICommand {
        private IImageServiceModal modal;

        public NewFileCommand(IImageServiceModal modal) {
            this.modal = modal;             
        }

        public string Execute(string[] args, out bool result) {
			// The function Will Return the New Path if result = true, and will return the error message
            return modal.AddFile(args[0], out result);
        }
    }
}

