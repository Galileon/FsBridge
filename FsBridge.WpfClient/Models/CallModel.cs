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
        public FsClient.Protocol.FsCallState CallState
        {
            get { return GetValue<FsClient.Protocol.FsCallState>(CallStateProperty); }
            set { SetValue(CallStateProperty, value); }
        }
        public static readonly IPropertyData CallStateProperty = RegisterProperty(nameof(CallState),() => FsClient.Protocol.FsCallState.Disconnected);

        public string Ani
        {
            get { return GetValue<string>(AniProperty); }
            set { SetValue(AniProperty, value); }
        }
        public static readonly IPropertyData AniProperty = RegisterProperty(nameof(Ani), () => "");

        public string Dnis
        {
            get { return GetValue<string>(DnisProperty); }
            set { SetValue(DnisProperty, value); }
        }
        public static readonly IPropertyData DnisProperty = RegisterProperty(nameof(Dnis), () => "");




        public Catel.Collections.FastObservableCollection<FsBridge.FsClient.Protocol.Events.EventBase> Events { get; set; } = new Catel.Collections.FastObservableCollection<FsClient.Protocol.Events.EventBase>();
    }
}
