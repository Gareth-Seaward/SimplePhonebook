import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Entry } from 'src/app/_models/entry';
import { Router } from '@angular/router';
import { EntryService } from 'src/app/_services/entry.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-entry-new',
  templateUrl: './entry-new.component.html',
  styleUrls: ['./entry-new.component.css']
})
export class EntryNewComponent implements OnInit {
  entry: Entry;
  editItemForm: FormGroup;

  constructor(private fb: FormBuilder,
    private router: Router,
    private entryService: EntryService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.entry = {id:0,name:'',phoneNumber:''};
    this.createEditItemForm();
  }

  createEditItemForm(){
    this.editItemForm = this.fb.group({
      name: [this.entry.name, Validators.required],
      phoneNumber: [this.entry.phoneNumber, [Validators.required,
        Validators.pattern('[(\+0-9]{1,3}[ ]*[0-9]{2}[ ]*[0-9]{3}[ ]*[0-9]{4}')]]
    });
  }

  saveEntry(){
    if(this.editItemForm.valid){
      this.entry = Object.assign({},this.editItemForm.value,{id: this.entry.id * -1});
      const user = JSON.parse(localStorage.getItem('user'));
      this.entryService.addEntry(user.phonebookId, this.entry)
      .subscribe(data =>{
        this.closeNew();
      },error => {
        this.alertify.error('Problems saving data');
      })
    }
  }

  closeNew(){
    this.router.navigate(['/entries']);
  }

}
