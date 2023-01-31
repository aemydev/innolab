import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appZoom]'
})
export class ZoomDirDirective {
  public zoom : number = 1;

  constructor(private el: ElementRef) { }
  
  @HostListener('wheel', ['$event']) onWheel(event:any) {   
    this.el.nativeElement.style.transformOrigin =`${event.offsetX}px ${event.offsetY}px`;
    this.zoom += event.deltaY * -0.01;
    this.zoom = Math.min(Math.max(1, this.zoom), 5);
    this.el.nativeElement.style.transform=`scale(${this.zoom})`;
  }
}
