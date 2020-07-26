import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/api/services';
import { MenuItem } from 'primeng/api';


@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {

  items: MenuItem[];

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.items = [
      {
        label: 'Recreate',
        icon: 'pi pi-fw pi-refresh',
        command: () => {
          this.save();
        }
      }
    ];
  }

  save() {

  }
}
