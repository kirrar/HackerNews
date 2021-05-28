import { Component, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-hacker-news',
  templateUrl: './hacker-news.component.html'
})
export class HackerNewsComponent {
  public pages: Pagination[];
  private http: HttpClient;
  private baseUrl: string;
  private page: number;
  public searchTerm: string;
  private oldSearchTerm: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.page = 1;
    this.searchTerm = '';
    this.oldSearchTerm = '';

    this.getPage(1);
  
  }

  getPage(page: number) {

    if (this.searchTerm != '') {
      this.getBySearchTerm(page)
        .subscribe(resp => {
          if (resp.status == 200) {
            this.pages = { ...resp.body! };
            this.page = page;
          }
          else {
            alert("No articles with your search term");
            this.searchTerm = '';
          }
        });
    }
    else {
      this.getByPageNumber(page)
        .subscribe(resp => {
          if (resp.status == 200) {
            this.pages = { ...resp.body! };
            this.page = page;
          }
        });
    }
  }

  getByPageNumber(page: number): Observable<HttpResponse<Pagination[]>> {
    return this.http.get<Pagination[]>(
      this.baseUrl + 'HackerNews/GetPage?PageNumber=' + page + '&PageSize=10', { observe: 'response' });
  }

  getBySearchTerm(page: number): Observable<HttpResponse<Pagination[]>> {
    return this.http.get<Pagination[]>(
      this.baseUrl + 'HackerNews/SearchStories?PageNumber=' + page + '&PageSize=20&SearchTerm=' + this.searchTerm + '', { observe: 'response' });
  }

  searchArticles() {

    if (this.searchTerm == '') {
      return alert("Please enter something to search for.");
    }

    if (this.searchTerm != this.oldSearchTerm) {
      this.getPage(1);
    }
    else {
      this.getPage(this.page);
    }
  }

  resetSearch() {
    this.oldSearchTerm = '';
    this.searchTerm = '';
    this.getPage(1);
  }
}

interface Pagination {
  pageNumber: number;
  pageSize: number;
  totalStories: number;
  stories: NewsArticle[];
}

interface NewsArticle {
  id: number;
  title: string;
  url: string;
}
