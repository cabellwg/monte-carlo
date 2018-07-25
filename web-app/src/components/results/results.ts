import { Data } from 'resources/scripts/data';
import { inject, NewInstance } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { APIRequest } from 'resources/scripts/api';
import { EventAggregator } from '../../../node_modules/aurelia-event-aggregator';

@inject(Router, EventAggregator)
export class Results {
  data: Data;

  router: Router;
  ea: EventAggregator;

  constructor(router, ea: EventAggregator){
    this.router = router;
    this.data = Data.instance;
    this.ea = ea;
  }

  rerun() {
    APIRequest.postInputs(this.data.inputs)
      .then(data => {
        Data.instance = data as Data;
        Data.instance.inputs = this.data.inputs;
        this.data = Data.instance;
        this.router.navigateToRoute(
          this.router.currentInstruction.config.name,
          this.router.currentInstruction.params,
          { replace: true }
        );
    });
  }
}
