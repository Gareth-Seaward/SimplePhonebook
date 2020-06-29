import { Component, OnInit } from '@angular/core';
import { Entry } from 'src/app/_models/entry';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-entiry-list',
  templateUrl: './entiry-list.component.html',
  styleUrls: ['./entiry-list.component.css']
})
export class EntiryListComponent implements OnInit {
  entries: Entry[];
  pagination: Pagination;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.entries = data['entries'].result;
      this.pagination = data['entries'].pagination;
      console.log(this.entries);
    })
  }

}
