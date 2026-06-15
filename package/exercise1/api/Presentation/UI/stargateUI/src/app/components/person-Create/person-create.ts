import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PersonCreateService } from './person-create.service';
import { forkJoin, timer } from 'rxjs';

@Component({
  selector: 'app-person-create',
  imports: [CommonModule, FormsModule],
  templateUrl: './person-create.html',
  styleUrl: './person-create.css',
})
export class PersonCreate {
  @Output() saved = new EventEmitter<void>();

  name: string = '';
  isSaving: boolean = false;
  private personCreateService = inject(PersonCreateService);

  save() {
    if (!this.name.trim()) return;
    this.isSaving = true;

    // Enforce a minimum 0.5 second spin time for the galaxy animation
    const minDelay$ = timer(500);
    const save$ = this.personCreateService.create(this.name);

    forkJoin([minDelay$, save$]).subscribe({
      next: () => {
        this.isSaving = false;
        this.saved.emit();
      },
      error: (err) => {
        this.isSaving = false;
        console.error('Error saving person:', err);
      }
    });
  }
}
