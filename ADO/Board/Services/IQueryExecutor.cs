using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Board.Services
{
	public interface IQueryExecutor
	{
		Task<IList<WorkItem>> QueryByTag(string project, string tag);
	}
}
