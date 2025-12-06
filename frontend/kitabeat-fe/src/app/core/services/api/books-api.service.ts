import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../../models/book.model';
import { environment } from '../../../../environments/environment';

const Endpoints = {
  search: '/search',
};

@Injectable({
  providedIn: 'root',
})
export class BooksApiService {
  private readonly http = inject(HttpClient);

  private baseUrl = `${environment.apiUrl}/books`;

  search(query: string): Observable<Book[]> {
    let params = new HttpParams();

    if (query) {
      params = params.set('query', query);
    }

    return this.http.get<Book[]>(`${this.baseUrl}${Endpoints.search}`, { params });
  }
}
