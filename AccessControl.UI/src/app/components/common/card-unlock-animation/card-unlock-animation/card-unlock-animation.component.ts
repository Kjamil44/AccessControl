import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-card-unlock-animation',
  templateUrl: './card-unlock-animation.component.html',
  styleUrls: ['./card-unlock-animation.component.css']
})
export class CardUnlockAnimationComponent implements OnChanges {
  /** Size of the whole icon (square px) */
  @Input() size = 48;
  /** Duration in ms of the swipe */
  @Input() duration = 900;
  /** 'locked' shows red LED + closed shackle, 'unlocked' shows green LED + rotated shackle */
  @Input() state: 'locked' | 'unlocked' = 'unlocked';
  /** Change this to re-play the swipe (e.g., a timestamp or counter) */
  @Input() playKey: any;

  swipeClass = 'swipe-run';

  ngOnChanges(changes: SimpleChanges) {
    // retrigger CSS animation when playKey changes
    if (changes['playKey']) {
      this.swipeClass = '';
      // next tick
      requestAnimationFrame(() => this.swipeClass = 'swipe-run');
    }
  }
}
