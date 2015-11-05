using Microsoft.AspNet.WebHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CaptainHook.Web.Hook.Handlers
{
    public class Basic : WebHookHandler
    {
        public Basic()
        {
            this.Receiver = "github";
        }

        public override Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            string action = context.Actions.First();

            JObject data = context.GetDataOrDefault<JObject>();

            return Task.FromResult(true);
        }
    }
}