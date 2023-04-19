import { Component, Input } from '@angular/core';
import { WorkItem } from '../work-item';

@Component({
  selector: 'app-work-item-table',
  templateUrl: './work-item-table.component.html',
  styleUrls: ['./work-item-table.component.scss']
})
export class WorkItemTableComponent {
  @Input() workItems: WorkItem[] = [];

  constructor() {}

  ngOnInit() {
  }

  open(workitem: WorkItem) {
    workitem.isOpen = !workitem.isOpen;
  }

  getIcon(workItem: WorkItem): string
  {
    let icon: string = "bi-pentagon";
    if (workItem.workItemType == "Epic")
    {
      icon = "bi-pentagon";
    }
    else if (workItem.workItemType == "Feature")
    {
      icon = "bi-trophy";
    } 
    else if (workItem.workItemType == "User Story")
    {
      icon = "bi-book";
    }
    else if (workItem.workItemType == "Task")
    {
      icon = "bi-card-checklist";
    }

    return icon;
  }
}
