import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Router } from 'aurelia-router';

@inject(EventAggregator, Router)
export class Results {

  router: Router
  data: Data = Data.instance;

  constructor(router){
    this.router = router
  }

  attached(){
  }


}
