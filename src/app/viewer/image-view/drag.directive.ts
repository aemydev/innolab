import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appDrag]'
})
export class DragDirective {
  private clicked : boolean = false;
  private yAxis;
  private xAxis;
  private x;
  private y;

  constructor(private el: ElementRef, private window: Window) {
  }

  @HostListener('mouseup', ['$event']) onMouseUp(event:any) {   
    this.el.nativeElement.style.cursor = "default";
    this.clicked = false;
  }

  
  @HostListener('mousedown', ['$event']) onMouseDown(event:any) {   
    event.preventDefault();

    this.clicked = true;
    this.xAxis = event.offsetX - this.el.nativeElement.offsetLeft;
    this.yAxis = event.offsetY - this.el.nativeElement.offsetTop;

    this.el.nativeElement.style.cursor="grabbing";
  }

  @HostListener('mousemove', ['$event']) onMouseMove(event:any) {   
    if(!this.clicked) return;

    event.preventDefault();

    this.x = event.offsetX;
    this.y = event.offsetY;



    this.el.nativeElement.children[0].style.left=`${this.x - this.xAxis}px`;
    this.el.nativeElement.children[0].style.top=`${this.y - this.yAxis}px`;

    this.checkSize()

  }

  @HostListener('window:mouseup', ['$event']) onWindowMouseUp($event: MouseEvent) {
    console.log('Through HostListener - Click Event Details: ', $event)
    this.clicked = false;
  }

  checkSize(){
    let containerOut = this.el.nativeElement.getBoundingClientRect();
    let imgIn = this.el.nativeElement.children[0].getBoundingClientRect();

    if(parseInt(this.el.nativeElement.children[0].style.left) > 0){
      this.el.nativeElement.children[0].style.left="0px";
    }else if (imgIn.right < containerOut.right){
      this.el.nativeElement.children[0].style.left=`-${imgIn.width - containerOut.width}px`;
    }

    if(parseInt(this.el.nativeElement.children[0].style.top) > 0){
      this.el.nativeElement.children[0].style.top="0px";
    }else if (imgIn.bottom < containerOut.bottom){
      this.el.nativeElement.children[0].style.top=`-${imgIn.height - containerOut.height}px`;
    }

    
  }
}
