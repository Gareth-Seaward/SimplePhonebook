import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-phonebook',
  templateUrl: './phonebook.component.html',
  styleUrls: ['./phonebook.component.css']
})
export class PhonebookComponent implements OnInit {
  phonebooks: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getPhonebooks();
  }
  

  getPhonebooks(){
    this.http.get('http://localhost:5000/v1/phonebooks').subscribe(response => {
      this.phonebooks = response;
    }, error => {
      console.log(error);
    });
  }

}
