import { Component, EventEmitter, Output, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonCreate } from '../person-create/person-create';
import { AstronautCreate } from '../astronaut-create/astronaut-create';
import { AstronautModify } from '../astronaut-modify/astronaut-modify';

@Component({
  selector: 'app-rocket-launch',
  standalone: true,
  imports: [CommonModule, PersonCreate, AstronautCreate, AstronautModify],
  templateUrl: './rocket-launch.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './rocket-launch.css',
})
export class RocketLaunch {
  @Output() rosterUpdated = new EventEmitter<void>();
  isLaunching = false;
  isExploding = false;
  showModal = false;
  modalMode: 'create' | 'modify' = 'create';
  selectedAstronaut: any = null;

  launchRocket(mode: 'create' | 'modify' = 'create', astronaut: any = null) {
    if (this.isLaunching) return;
    this.modalMode = mode;
    this.selectedAstronaut = astronaut;
    this.isLaunching = true;
    this.isExploding = false;
    this.showModal = true;

    setTimeout(() => {
      this.isLaunching = false;
      this.isExploding = false;
    }, 500);
  }

  closeModal() {
    this.showModal = false;
  }

  onSaved() {
    this.closeModal();
    this.rosterUpdated.emit();
  }
}
