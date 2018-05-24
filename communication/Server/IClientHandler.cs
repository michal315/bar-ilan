using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.IPEndPoint;

namespace communication.Server {
    interface IClientHandler {
        void HandleClient(TcpClient client); 
    }
}
