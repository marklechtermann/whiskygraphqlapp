import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Apollo } from 'apollo-angular';
import { Observable, Subscription } from 'rxjs';
import gql from 'graphql-tag';
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-whiskys',
  templateUrl: './whiskys.component.html'
})
export class WhiskyComponent {
  public whiskys: any;
  private querySubscription: Subscription;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private apollo: Apollo) {
    this.querySubscription = this.apollo.watchQuery<any>({
      query: gql` {
        whiskys {
          name
          age
          destillery {
            name
            owner
          }
        }
      }
      `
    })
    .valueChanges.subscribe(({data}) => {
      this.whiskys = data.whiskys;
    });
  }

  ngOnDestroy() {
    this.querySubscription.unsubscribe();
  }
}
