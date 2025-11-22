import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from "@angular/material/button";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-empty-state',
  imports: [
    RouterModule,
    MatIcon,
    MatButton
],
  templateUrl: './empty-state.component.html',
  styleUrl: './empty-state.component.scss'
})
export class EmptyStateComponent {

}
