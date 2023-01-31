import { Directive, ElementRef, HostListener, Renderer2  } from '@angular/core';

@Directive({
  selector: '[appDropdown]'
})
export class DropdownDirective {
  isOpen = false;

  constructor(private el: ElementRef, private renderer: Renderer2) { }

  @HostListener('click', ['$event']) onClick(event:any) {   
    let dropdownContent = this.el.nativeElement.querySelector('.dropdown-content');

    if(this.isOpen){
      this.renderer.removeClass(dropdownContent, 'open');
      this.isOpen = false;
    }else{
      this.renderer.addClass(dropdownContent, 'open');
      this.isOpen = true;
    }
  }

  @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    let dropdownContent = this.el.nativeElement.querySelector('.dropdown-content');

    if(this.el.nativeElement.contains(event.target) == false){
      this.renderer.removeClass(dropdownContent, 'open');
      this.isOpen = false;
    }
  }
}
