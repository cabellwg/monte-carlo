import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(EventAggregator)
export class Results {
 
  inputsReturnButton(){
    window.location.href="/"
   }
}
