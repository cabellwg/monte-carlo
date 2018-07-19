import { APIRequest } from './../../resources/scripts/api';
import { Data } from './../../resources/scripts/data';
import { Inputs } from './../../resources/scripts/inputs';
import {EventAggregator} from 'aurelia-event-aggregator';
import { inject } from 'aurelia-framework';

/*import {inject} from 'aurelia-dependency-injection';
import { ValidationControllerFactory, ValidationController, ValidationRules, validateTrigger, ValidationRenderer } from 'aurelia-validation';
import { BootstrapFormRenderer } from './bootstrap-form-renderer';

@inject(ValidationControllerFactory)*/
@inject(EventAggregator)
export class Form{

  inputs: Inputs
  ea: EventAggregator

  constructor(EventAggregator){
    this.ea = EventAggregator;
  }

  /*
  currentAge = '';
  retireAge = '';
  controller = null;

  constructor(controllerFactory){
    this.controller = controllerFactory.createForCurrentScope();
    this.controller.addRenderer(new BootstrapFormRenderer());
    this.controller.validateTrigger= validateTrigger.change;
  }

  bind(){
    ValidationRules
      .ensure((m: Form) => m.currentAge).required()
      .ensure((m: Form) => m.retireAge).required()
      .on(this);
  }
  
  submitFormButton(){
  if(this.controller.validate()){
    window.location.href="/results"
  }

  }*/

  submitFormButton(){
    APIRequest.postInputs(this.inputs)
      .then(data => {
        this.ea.publish("dataStream", data)
        window.location.href="/results"
      })
    
  }


}






