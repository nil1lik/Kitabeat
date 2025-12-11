import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnalyzeEmotionResponse, Book } from '../../models';
import { environment } from '../../../../environments/environment';

const Endpoints = {
  search: '/search',
  analyzeEmotion: (id: string) => `/${id}/analyze-emotion`,
} as const;

/**
 * Service for interacting with the books API.
 */
@Injectable({
  providedIn: 'root',
})
export class BooksApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/books`;

  /**
   * Search books by query string.
   */
  search(query: string): Observable<Book[]> {
    const params = query ? new HttpParams().set('query', query) : new HttpParams();
    return this.http.get<Book[]>(`${this.baseUrl}${Endpoints.search}`, { params });
  }

  /**
   * Analyze emotions for a specific book.
   */
  analyzeEmotion(id: string): Observable<AnalyzeEmotionResponse> {
    return this.http.post<AnalyzeEmotionResponse>(
      `${this.baseUrl}${Endpoints.analyzeEmotion(id)}`,
      {}
    );
  }
}
