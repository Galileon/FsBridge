using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.WpfClient.Models
{
    public class MakeCallParams
    {
        public string Destination { get; set; }
        public string CallingName { get; set; }
        public string CallingNumber { get; set; }
    }
}
