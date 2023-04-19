using System.Collections.Generic;

namespace Board.Models
{
	public class WorkItem
	{
		public int? Id { get; set; }
		public int? ParentId { get; set; }
		public string Title { get; set; }
		public string WorkItemType { get; set; }
		public string State { get; set; }
		public string Tags { get; set; }
		public bool? HasChildren => this.Children?.Count > 0;
		public bool? IsOpen { get; set; } = false;
		public List<WorkItem> Children { get; set; }

	}
}
