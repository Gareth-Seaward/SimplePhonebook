import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup,
  FormControl,
  Validators,
  FormBuilder  } from '@angular/forms';
import { PhonebookService } from '../_services/phonebook.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Phonebook } from '../_models/phonebook';

@Component({
  selector: 'app-phonebook',
  templateUrl: './phonebook.component.html',
  styleUrls: ['./phonebook.component.css']
})
export class PhonebookComponent implements OnInit {
  phonebookForm: FormGroup;
  phonebook: Phonebook;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private phonebookService: PhonebookService,
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.phonebook = data.phonebook;
      this.createPhonebookForm();
    });
  }

  createPhonebookForm(){
    this.phonebookForm = this.fb.group({
      name: [this.phonebook.phonebookName, [Validators.required, Validators.minLength(4), Validators.maxLength(24)]]
    });
  }

  updatePhonebook() {
    console.log(this.phonebook);
    this.phonebookService.updatePhonebook(this.authService.decodedToken.nameid, this.phonebook)
    .subscribe(next => {
      this.alertify.success('Phonebook updated successfully.');
      console.log(this.phonebook);
      this.router.navigate(['/entries']);
    },
    error => {
      this.alertify.error(error);
    })
  }

}
