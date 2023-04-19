import { Component, OnInit } from '@angular/core';
import { ApiService } from './http-services/http-api.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit  {
  name = 'Angular';
  workItems: any = [];
  tag = '';
  loading = false;

  constructor(public apiService:ApiService){  }
  ngOnInit()
  {
    this.getWorkItems();
  }

  getWorkItems() {
    this.loading = true;
    return this.apiService.getWorkItems().subscribe((data: {}) => {
      this.workItems = data;
      this.loading = false;
    });
  }

  filterWorkItems() {
    this.loading = true;
    return this.apiService.getWorkItemsByTag(this.tag).subscribe((data: {}) => {
      this.workItems = data;
      this.loading = false;
    });
  }
}