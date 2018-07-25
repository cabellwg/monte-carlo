import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Router } from 'aurelia-router';

@inject(EventAggregator, Router)
export class Results {
  data: Data = Data.instance;

  router: Router

  constructor(router){
    this.router = router;
  }

  inputsReturnButton(){
    this.router.navigateToRoute("home");
  }
  
}
