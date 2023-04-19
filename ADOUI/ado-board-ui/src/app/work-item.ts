export class WorkItem {
    id: number = 0;
    title: string = "";
    workItemType: string = ""
    state: string = ""
    tags: string = ""
    hasChildren: boolean = false;
    children: WorkItem[] = [];
    isOpen: boolean =false;
  }