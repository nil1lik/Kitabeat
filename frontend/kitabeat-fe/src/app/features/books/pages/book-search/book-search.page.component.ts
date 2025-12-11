import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject, switchMap, takeUntil } from 'rxjs';
import { BooksApiService } from '../../../../core/services/api/books-api.service';
import { Book } from '../../../../core/models';
import { getEmotionIcon, getEmotionSeverity, EmotionSeverity } from '../../../../shared';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { SkeletonModule } from 'primeng/skeleton';
import { MessageModule } from 'primeng/message';
import { DividerModule } from 'primeng/divider';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TooltipModule } from 'primeng/tooltip';

const SEARCH_DEBOUNCE_TIME = 400;
const DEFAULT_SKELETON_COUNT = 6;

@Component({
  selector: 'kb-book-search',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    CardModule,
    ButtonModule,
    TagModule,
    SkeletonModule,
    MessageModule,
    DividerModule,
    ProgressSpinnerModule,
    TooltipModule,
  ],
  templateUrl: './book-search.page.component.html',
  styleUrls: ['./book-search.page.component.scss'],
})
export class BookSearchComponent implements OnInit, OnDestroy {
  private readonly booksService = inject(BooksApiService);
  private readonly destroy$ = new Subject<void>();

  readonly searchControl = new FormControl<string>('', { nonNullable: true });
  readonly skeletonItems = Array(DEFAULT_SKELETON_COUNT).fill(0);

  books: Book[] = [];
  loading = false;
  error: string | null = null;
  hasSearched = false;
  analyzingBookId: string | null = null;

  ngOnInit(): void {
    this.initializeSearch();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  analyzeBook(book: Book): void {
    this.analyzingBookId = book.id;

    this.booksService.analyzeEmotion(book.id).subscribe({
      next: (response) => {
        book.emotionLabel = response.primaryLabel;
        book.emotionScore = response.primaryScore;
        this.analyzingBookId = null;
      },
      error: (error) => {
        console.error('Emotion analysis failed:', error);
        this.error = 'Duygu analizi yapılırken bir hata oluştu.';
        this.analyzingBookId = null;
      },
    });
  }

  getEmotionIcon(emotionLabel: string): string {
    return getEmotionIcon(emotionLabel);
  }

  getEmotionSeverity(emotionLabel: string): EmotionSeverity {
    return getEmotionSeverity(emotionLabel);
  }

  private initializeSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(SEARCH_DEBOUNCE_TIME),
        distinctUntilChanged(),
        switchMap((query) => this.performSearch(query)),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (books) => this.handleSearchSuccess(books),
        error: (error) => this.handleSearchError(error),
      });
  }

  private performSearch(query: string) {
    this.loading = true;
    this.error = null;
    this.hasSearched = true;
    return this.booksService.search(query);
  }

  private handleSearchSuccess(books: Book[]): void {
    this.books = books;
    this.loading = false;
  }

  private handleSearchError(error: unknown): void {
    console.error('Search failed:', error);
    this.error = 'Kitaplar alınırken bir hata oluştu.';
    this.loading = false;
  }
}
