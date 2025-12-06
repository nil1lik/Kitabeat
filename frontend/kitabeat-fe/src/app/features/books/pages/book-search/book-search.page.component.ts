import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject, switchMap, takeUntil } from 'rxjs';
import { BooksApiService } from '../../../../core/services/api/books-api.service';
import { Book } from '../../../../core/models/book.model';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';

@Component({
  selector: 'kb-book-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, InputTextModule, IconFieldModule, InputIconModule],
  templateUrl: './book-search.page.component.html',
  styleUrls: ['./book-search.page.component.scss'],
})
export class BookSearchComponent implements OnInit, OnDestroy {
  private booksService = inject(BooksApiService);
  private destroy$ = new Subject<void>();

  searchControl = new FormControl<string>('', { nonNullable: true });
  books: Book[] = [];
  loading = false;
  error: string | null = null;
  hasSearched = false;
  skeletonItems = Array(6).fill(0); // 6 skeleton cards

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        switchMap((query) => {
          this.loading = true;
          this.error = null;
          this.hasSearched = true;
          return this.booksService.search(query);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (books) => {
          this.books = books;
          this.loading = false;
        },
        error: (err) => {
          console.error(err);
          this.error = 'Kitaplar alınırken bir hata oluştu.';
          this.loading = false;
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
