import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Entry } from '../_models/entry';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EntryService {
  baseUrl = environment.apiUrl + 'phonebooks/';
constructor(private http: HttpClient) { }

getEntries(phonebookid: number, page?,itemsPerPage?, entryParams?)
  : Observable<PaginatedResult<Entry[]>> {
  const paginatedResult: PaginatedResult<Entry[]> = new PaginatedResult<Entry[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  return this.http.get<Entry[]>(this.baseUrl + phonebookid + '/entries', {observe: 'response', params})
  .pipe(
    map(response => {
      paginatedResult.result = response.body;
      if (response.headers.get('pagination') !== null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('pagination'));
      }
      return paginatedResult;
    })
  );
}

getEntry(phonebookid: number, entryid: number) : Observable<Entry> {
  return this.http.get<Entry>(this.baseUrl + phonebookid + '/entries/' + entryid);
}

updateEntry(phonebookid: number, entry: Entry){
  return this.http.put(this.baseUrl + phonebookid + '/entries/' + entry.id,entry);
}

addEntry(phonebookid: number, entry: Entry){
  return this.http.post(this.baseUrl + phonebookid + '/entries',entry );
}

}
