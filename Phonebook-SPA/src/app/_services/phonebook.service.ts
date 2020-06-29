import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Phonebook } from '../_models/phonebook';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PhonebookService {
  baseUrl = environment.apiUrl + 'users/';

  constructor(private http: HttpClient) {}

  getPhonebook(userid): Observable<Phonebook> {
    return this.http.get<Phonebook>(this.baseUrl + userid + '/phonebooks');
  }

  updatePhonebook(userid: number, phonebook: Phonebook){
    const phonebookToUpdate = {Name: phonebook.phonebookName};
    return this.http.put(this.baseUrl + userid + '/phonebooks',phonebookToUpdate);
  }
}
