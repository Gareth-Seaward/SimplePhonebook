import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Entry } from 'src/app/_models/entry';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { EntryService } from 'src/app/_services/entry.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { of } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Entry-Item',
  templateUrl: './Entry-Item.component.html',
  styleUrls: ['./Entry-Item.component.css']
})
export class EntryItemComponent implements OnInit {
  @Input() entry: Entry;
  @Output() closeEditEmitter = new EventEmitter<number>();
  editItemForm: FormGroup

  constructor(
    private fb: FormBuilder,
    private entryService: EntryService,
    private alertify: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    if(!this.entry){
      this.entry = {id:0,name:"",phoneNumber:""};
    }
    this.createEditItemForm();
  }

  createEditItemForm(){
    this.editItemForm = this.fb.group({
      name: [this.entry.name, Validators.required],
      phoneNumber: [this.entry.phoneNumber, [Validators.required,
        Validators.pattern('(\\+27|27|0)[ ]*[0-9]{2}[ ]*[0-9]{3}[ ]*[0-9]{4}')]]
    });
  }

  saveEntry(){
    if(this.editItemForm.valid){
      this.entry = Object.assign({},this.editItemForm.value,{id: this.entry.id * -1});
      const user = JSON.parse(localStorage.getItem('user'));
      this.entryService.updateEntry(user.phonebookId, this.entry)
      .subscribe(data =>{
        this.closeEdit();
      },error => {
        this.alertify.error('Problems saving data');
      })
    }
  }

  closeEdit(){
    this.closeEditEmitter.emit(this.entry.id);
  }

  cancelNew(){
    this.router.navigate(['/entries']);
  }

}
