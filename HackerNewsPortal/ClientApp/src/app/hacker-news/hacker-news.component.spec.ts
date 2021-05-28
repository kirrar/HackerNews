import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HackerNewsComponent } from './hacker-news.component';

describe('HackerNewsComponent', () => {
  let fixture: ComponentFixture<HackerNewsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [HackerNewsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HackerNewsComponent);
    fixture.detectChanges();
  });

  it('go to page 2 if there are paination buttons', async(() => {
    var = element(by.cssContainingText('span.ngx-pagination', '100'))

    const countElement = fixture.nativeElement.querySelector('strong');
    expect(countElement.textContent).toEqual('0');

    const incrementButton = fixture.nativeElement.querySelector('button');
    incrementButton.click();
    fixture.detectChanges();
    expect(countElement.textContent).toEqual('1');
  }));
});
