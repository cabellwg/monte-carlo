import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Router } from 'aurelia-router';

@inject(EventAggregator, Router)
export class Results {

  router: Router
  private Data: typeof Data = Data

  constructor(router){
    this.router = router
  }

  attached(){
    console.log(Data.portfolioPercentiles)
  }


}
