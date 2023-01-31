import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appExpandable]',
})
export class ExpandableDirective {
  isOpen = false;

  constructor(private _element: ElementRef, private _renderer: Renderer2) {}

  @HostListener('click', ['$event']) onClick(event: any) {
    event.stopPropagation();

    let content = this._element.nativeElement.querySelector('.content');

    if (event.target.matches('.toggleBtn')) {
      if (this.isOpen) {
        this._renderer.removeClass(content, 'show');
        this.isOpen = false;
      } else {
        this._renderer.addClass(content, 'show');
        this.isOpen = true;
      }
    }
  }
}
