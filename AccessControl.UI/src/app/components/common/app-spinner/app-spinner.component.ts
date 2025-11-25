import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-spinner',
  templateUrl: './app-spinner.component.html',
  styleUrls: ['./app-spinner.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppSpinnerComponent {
  /** Show/hide the spinner */
  @Input() visible = false;

  /** Size in pixels (PrimeNG default is 50) */
  @Input() size = 40;

  /** Stroke width (PrimeNG default is 2) */
  @Input() strokeWidth = '3';

  /** Optional text shown under the spinner */
  @Input() message: string | null = null;

  /** If true, covers parent with a dim overlay */
  @Input() overlay = false;

  /** If true, makes a fullscreen overlay (fixed) */
  @Input() fullscreen = false;
}
