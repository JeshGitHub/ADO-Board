import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkItemTableComponent } from './work-item-table.component';

describe('WorkItemTableComponent', () => {
  let component: WorkItemTableComponent;
  let fixture: ComponentFixture<WorkItemTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkItemTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkItemTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
