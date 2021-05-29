import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NgxPaginationModule } from 'ngx-pagination';
import { HttpClientModule } from '@angular/common/http';
import { HackerNewsComponent } from './hacker-news.component';

describe('HackerNewsComponent', () => {
  let fixture: ComponentFixture<HackerNewsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HackerNewsComponent ],
      imports: [ NgxPaginationModule, HttpClientModule ],
      providers: [ { provide: 'BASE_URL', useValue: 'http://localhost:5000/' } ]
    })
    .compileComponents();
  }));
  
  beforeEach(() => {
    fixture = TestBed.createComponent(HackerNewsComponent);
    fixture.detectChanges();
  });

  it('should have a input box', async(() => {
    const inputBox = fixture.nativeElement.querySelector('#searchTerm');
    expect(inputBox).toBeDefined();
  }));

  it('should have a search button', async() =>{
    const button = fixture.nativeElement.querySelector('.btn-primary');
    expect(button).toBeDefined();
  });

  it('should have a reset button', async() =>{
    const button = fixture.nativeElement.querySelector('#searchTerm');
    expect(button).toBeDefined();
  });

  it('should show empty table message if not articles', async() =>{
    const component = fixture.componentInstance;
    expect(component.pages).toBeUndefined();

    const table = fixture.nativeElement.querySelector('#noData');
    expect(table).toBeDefined();
  });
});
