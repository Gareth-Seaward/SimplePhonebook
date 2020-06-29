import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
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
  @ViewChild('editPhonebook') editPhonebook: NgForm;
  phonebook: Phonebook;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private phonebookService: PhonebookService,
    private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.phonebook = data.phonebook;
    });
  }

  updatePhonebook() {
    this.phonebookService.updatePhonebook(this.authService.decodedToken.nameid, this.phonebook)
    .subscribe(next => {
      this.alertify.success('Phonebook updated successfully.');
      console.log(this.phonebook);
      this.editPhonebook.reset(this.phonebook);
      this.router.navigate(['/entries']);
    },
    error => {
      this.alertify.error(error);
    })
  }

}
