using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Board.Models;
using Board.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Board.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WorkItemController : ControllerBase
	{
		private readonly AppSettings appSettings;
		private readonly IQueryExecutor queryExecutor;
		public WorkItemController(IOptions<AppSettings> appSettings, IQueryExecutor queryExecutor)
		{
			this.appSettings = appSettings.Value;
			this.queryExecutor = queryExecutor;
		}

		// GET: api/<WorkItemController>
		[HttpGet]
		public async Task<List<WorkItem>> Get([FromQuery] string tag)
		{
			var workItems = await queryExecutor.QueryByTag(appSettings.Project, tag).ConfigureAwait(false);

			var workItemsList = workItems.Select(workItem => new WorkItem()
			{
				Id = workItem.Id,
				Title = workItem.Fields["System.Title"].ToString(),
				WorkItemType = workItem.Fields["System.WorkItemType"].ToString(),
				State = workItem.Fields["System.State"].ToString(),
				ParentId = GetParentId(workItem),
				Tags = workItem.Fields.ContainsKey("System.Tags") ? workItem.Fields["System.Tags"].ToString() : string.Empty
			}).ToList();

			Action<WorkItem> SetChildren = null;
			SetChildren = parent =>
			{
				parent.Children = workItemsList
					.Where(childItem => childItem.ParentId == parent.Id)
					.ToList();
				
				parent.Children
					.ForEach(SetChildren);
			};

			List<WorkItem> hierarchicalItems = new List<WorkItem>();

			if (string.IsNullOrEmpty(tag))
			{
				hierarchicalItems = workItemsList
					.Where(rootItem => rootItem.ParentId == workItemsList.Min(w => w.ParentId))
					.ToList();

				hierarchicalItems.ForEach(SetChildren);
			}
			else
			{
				foreach(var workItem in workItemsList)
				{
					if (!workItemsList.Where(w => w.Id == workItem.ParentId).Any())
					{
						hierarchicalItems.Add(workItem);
						continue;
					}
				}

				hierarchicalItems.ForEach(SetChildren);
			}

			return hierarchicalItems;
		}

		// GET api/<WorkItemController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<WorkItemController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<WorkItemController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<WorkItemController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		#region Private methods

		private int? GetParentId(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem workItem)
		{
			if (workItem.Fields.ContainsKey("System.Parent")) return int.Parse(workItem.Fields["System.Parent"].ToString());

			var parent = workItem.Relations?.Where(rel => rel.Rel == "System.LinkTypes.Hierarchy-Reverse").ToList().FirstOrDefault()?.Url;

			if (parent == null) return 0;

			return int.Parse(parent.Split('/').Last());
		}

		#endregion
	}
}
