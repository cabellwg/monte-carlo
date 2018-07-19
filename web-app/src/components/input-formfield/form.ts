import { Router } from 'aurelia-router';
import { APIRequest } from './../../resources/scripts/api';
import { Data } from './../../resources/scripts/data';
import { Inputs } from './../../resources/scripts/inputs';
import {EventAggregator} from 'aurelia-event-aggregator';
import { inject } from 'aurelia-framework';

/*import {inject} from 'aurelia-dependency-injection';
import { ValidationControllerFactory, ValidationController, ValidationRules, validateTrigger, ValidationRenderer } from 'aurelia-validation';
import { BootstrapFormRenderer } from './bootstrap-form-renderer';

@inject(ValidationControllerFactory)*/
@inject(EventAggregator, Router)
export class Form{

  inputs: Inputs = new Inputs()
  ea: EventAggregator
  router: Router

  constructor(EventAggregator, router){
    this.ea = EventAggregator;
    this.router = router;
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
    console.log(this.inputs)
    APIRequest.postInputs(this.inputs)
      .then(data => {
        Data.portfolioPercentiles = data.portfolioPercentiles
        Data.successRate = data.successRate
        console.log(Data.portfolioPercentiles)
        this.router.navigateToRoute("results")
        // window.location.href="/results"
      })
    
  }


}






