/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PhonebookService } from './phonebook.service';

describe('Service: Phonebook', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PhonebookService]
    });
  });

  it('should ...', inject([PhonebookService], (service: PhonebookService) => {
    expect(service).toBeTruthy();
  }));
});
