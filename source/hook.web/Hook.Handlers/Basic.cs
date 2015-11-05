using Microsoft.AspNet.WebHooks;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Octokit;

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

            return AssignPendingToCommit(data);
        }

        /// <summary>
        /// Setting GitHub Status using Octokit
        /// </summary>
        /// <returns>
        ///     
        /// </returns>
        public async Task AssignPendingToCommit(JObject data)
        {
            var client = new GitHubClient(new ProductHeaderValue("CaptainHook"));

            var token = ConfigurationManager.AppSettings["github_token"];
            if (token != null)
            {
                var tokenAuth = new Credentials(token);
                client.Credentials = tokenAuth;

                var status = new NewCommitStatus
                {
                    State = CommitState.Pending,
                    Description = "This is a test status"
                };

                var repoOwner = data["repository"]["owner"]["login"].Value<string>();
                var repoName = data["repository"]["name"].Value<string>();
                var commitSha = data["pull_request"]["head"]["sha"].Value<string>();

                var result = await client.Repository.CommitStatus.Create(repoOwner, repoName, commitSha, status);
            }
        }

    }
}