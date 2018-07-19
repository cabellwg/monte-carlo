import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(EventAggregator)
export class Results {

  ea: EventAggregator
  data: Data;

  constructor(EventAggregator){
    this.ea = EventAggregator;
  }

  attached(){
    this.ea.subscribe("dataStream", result =>{
      this.data = result;
      console.log("Got the data")
    })
  }

 
  inputsReturnButton(){
    window.location.href="/"
   }
}
