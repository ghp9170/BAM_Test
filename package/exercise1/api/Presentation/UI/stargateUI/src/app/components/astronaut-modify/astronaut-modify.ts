import { Component, Input, OnInit, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Person } from '../../models/Person';
import { AstronautDutyService } from './personModify.service';
import { AstronautDuty } from '../../models/AstronautDuty';

@Component({
  selector: 'app-astronaut-modify',
  imports: [CommonModule],
  templateUrl: './astronaut-modify.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './astronaut-modify.css',
})
export class AstronautModify implements OnInit {
  @Input() astronaut: Person | undefined;
  duties: AstronautDuty[] = [];
  isLoading = true;
  error = '';

  private dutyService = inject(AstronautDutyService);

  ngOnInit() {
    if (this.astronaut && this.astronaut.name) {
      this.dutyService.getAstronautDuty(this.astronaut.name).subscribe({
        next: (result) => {
          this.isLoading = false;
          if (result.success && result.astronautDuties) {
            // Sort duties descending by dutyStartDate
            this.duties = result.astronautDuties.sort((a, b) => {
              return new Date(b.dutyStartDate).getTime() - new Date(a.dutyStartDate).getTime();
            });
          } else {
            this.error = result.message || 'Failed to load duties.';
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.error = 'Error fetching astronaut duties.';
          console.error('Failed to fetch duties:', err);
        },
      });
    } else {
      this.isLoading = false;
      this.error = 'No astronaut data provided.';
    }
  }
}
