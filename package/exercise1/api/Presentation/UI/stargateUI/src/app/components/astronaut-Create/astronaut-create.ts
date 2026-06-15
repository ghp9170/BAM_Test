import { Component, EventEmitter, Input, Output, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AstronautCreateService } from './astronaut-create.service';
import { forkJoin, timer } from 'rxjs';

@Component({
  selector: 'app-astronaut-create',
  imports: [CommonModule, FormsModule],
  templateUrl: './astronaut-create.html',
  styleUrl: './astronaut-create.css',
})
export class AstronautCreate implements OnInit {
  @Input() astronaut: any;
  @Output() saved = new EventEmitter<void>();

  newAstronaut: any = {
    name: '',
    currentRank: '',
    currentDutyTitle: '',
    careerStartDate: ''
  };

  isSaving: boolean = false;
  private astronautCreateService = inject(AstronautCreateService);

  ngOnInit() {
    if (this.astronaut && this.astronaut.name) {
      this.newAstronaut.name = this.astronaut.name;
    }
  }

  saveAstronaut() {
    if (!this.newAstronaut.name.trim()) return;
    this.isSaving = true;

    // Enforce a minimum 0.5 second spin time for the galaxy animation
    const minDelay$ = timer(500);
    const save$ = this.astronautCreateService.create(this.newAstronaut);

    forkJoin([minDelay$, save$]).subscribe({
      next: () => {
        this.isSaving = false;
        this.saved.emit();
      },
      error: (err) => {
        this.isSaving = false;
        console.error('Error saving astronaut:', err);
      }
    });
  }
}
