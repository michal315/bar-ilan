﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService {
    public interface IImageController {
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
        void addICommand(ICommand newCommand, int CommandID);
    }

}

