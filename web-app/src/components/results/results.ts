import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { APIRequest } from 'resources/scripts/api';

@inject(Router)
export class Results {
  data: Data;
  
  get lineChartMax() {
    return 50000 * Math.floor(2.5 * (this.data.historical.stocksRetirementAmounts[2] + this.data.historical.bondsRetirementAmounts[2]) / 50000);
  }

  router: Router;

  constructor(router){
    this.router = router;
    this.data = Data.instance;
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
      });
  }

  setStocksDistributionType(type: string) {
    this.data.inputs.stocksDistributionType = type;
    this.rerun();
  }

  setBondsDistributionType(type: string) {
    this.data.inputs.bondsDistributionType = type;
    this.rerun();
  }

  setStocksStart(date: string) {
    this.data.inputs.stocksDataStartDate = date;
    this.rerun();
  }

  setBondsStart(date: string) {
    this.data.inputs.bondsDataStartDate = date;
    this.rerun();
  }

}
