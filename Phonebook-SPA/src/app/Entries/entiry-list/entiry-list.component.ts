import { Component, OnInit } from '@angular/core';
import { Entry } from 'src/app/_models/entry';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { EntryService } from 'src/app/_services/entry.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-entiry-list',
  templateUrl: './entiry-list.component.html',
  styleUrls: ['./entiry-list.component.css']
})
export class EntiryListComponent implements OnInit {
  entries: Entry[];
  entryParams: any = {};
  pagination: Pagination;
  user: any;

  constructor(
    private route: ActivatedRoute,
    private entryService: EntryService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.entries = data['entries'].result;
      this.pagination = data['entries'].pagination;
      this.user = JSON.parse(localStorage.getItem('user'));
      console.log(this.entries);
    })
  }

  pagedChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    const user = JSON.parse(localStorage.getItem('user'));
    this.entryService.getEntries(user.phonebookId, this.pagination.currentPage, this.pagination.itemsPerPage, this.entryParams).subscribe(
      (res: PaginatedResult<Entry[]>) => {
        this.entries = res.result;
        this.pagination = res.pagination;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

}
