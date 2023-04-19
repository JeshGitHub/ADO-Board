using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

namespace Board.Services
{
	public class QueryExecutor : IQueryExecutor
	{
        private readonly Uri uri;
        private readonly string personalAccessToken;
        private readonly AppSettings appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExecutor" /> class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        public QueryExecutor(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
            this.uri = new Uri("https://dev.azure.com/" + this.appSettings.OrgName);
            this.personalAccessToken = this.appSettings.PersonalAccessToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryExecutor" /> class.
        /// </summary>
        /// <param name="orgName">
        ///     An organization in Azure DevOps Services. If you don't have one, you can create one for free:
        ///     <see href="https://go.microsoft.com/fwlink/?LinkId=307137" />.
        /// </param>
        /// <param name="personalAccessToken">
        ///     A Personal Access Token, find out how to create one:
        ///     <see href="/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops" />.
        /// </param>
        public QueryExecutor(string orgName, string personalAccessToken)
        {
            this.uri = new Uri("https://dev.azure.com/" + orgName);
            this.personalAccessToken = personalAccessToken;
        }

        /// <summary>
        ///     Execute a WIQL (Work Item Query Language) query to return a list of work items.
        /// </summary>
        /// <param name="project">The name of your project within your organization.</param>
        /// <param name="tag">The name of tag applied to work item.</param>
        /// <returns>A list of <see cref="WorkItem"/> objects representing all the work items.</returns>
        public async Task<IList<WorkItem>> QueryByTag(string project, string tag)
        {
            var credentials = new VssBasicCredential(string.Empty, this.personalAccessToken);

            // create a wiql object and build our query
            var wiql = new Wiql()
            {
                // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [System.TeamProject] = '" + project + "' "
            };

            // Apply filter based on tag
            wiql.Query = !string.IsNullOrEmpty(tag) ? wiql.Query + "AND[System.Tags] Contains '" + tag + "' " : wiql.Query;

            // create instance of work item tracking http client
            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<WorkItem>();
                }

                // build a list of the fields we want to see
                //var fields = new[] { "System.Id", "System.Title", "System.State", "System.Tags" };

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, null, result.AsOf, expand: WorkItemExpand.All).ConfigureAwait(false);
            }
        }
    }
}
