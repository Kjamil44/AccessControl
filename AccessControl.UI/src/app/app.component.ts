import { Component, Renderer2, ViewEncapsulation } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { SpinnerService } from './services/spinner.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AppComponent {
  title = 'AccessControl.UI';
  isMainContent: boolean = false;

  constructor(private router: Router, private renderer: Renderer2, public spinner: SpinnerService) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isMainContent = !['/', '/login', '/register'].includes(event.urlAfterRedirects);
        if (!this.isMainContent) {
          this.renderer.removeClass(document.body, 'main-content');
        } else {
          this.renderer.addClass(document.body, 'main-content');
        }
      }
    });
  }
}
