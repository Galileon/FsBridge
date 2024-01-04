using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.WpfClient.Models
{
    public class CallModel : ModelBase
    {
        public Guid CallId { get; set; }
        public FsClient.Protocol.FsCallState CallState { get; set; }    
    }
}
