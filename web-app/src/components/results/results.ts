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

  goBack() {
    this.router.navigateBack();
  }

  rerun() {
    APIRequest.postInputs(this.data.inputs)
      .then(data => {
        Data.instance = data as Data;
        Data.instance.inputs = this.data.inputs;
        this.data = Data.instance;
        this.ea.publish("reload charts");
    });
  }

  setStocksDistributionType(type: string) {
    this.data.inputs.stocksDistributionType = type;
  }

  setBondsDistributionType(type: string) {
    this.data.inputs.bondsDistributionType = type;
  }

  setStocksStart(date: string) {
    this.data.inputs.stocksDataStartDate = date;
  }

  setBondsStart(date: string) {
    this.data.inputs.bondsDataStartDate = date;
  }

}
