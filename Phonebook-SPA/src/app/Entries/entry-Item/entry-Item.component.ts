import { Component, OnInit, Input } from '@angular/core';
import { Entry } from 'src/app/_models/entry';

@Component({
  selector: 'app-Entry-Item',
  templateUrl: './Entry-Item.component.html',
  styleUrls: ['./Entry-Item.component.css']
})
export class EntryItemComponent implements OnInit {
  @Input() entry: Entry;

  constructor() { }

  ngOnInit() {
  }

}
