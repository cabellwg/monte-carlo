import {inject} from 'aurelia-dependency-injection';
import { ValidationControllerFactory, ValidationController, ValidationRules, validateTrigger, ValidationRenderer } from 'aurelia-validation';
import { BootstrapFormRenderer } from './bootstrap-form-renderer';

@inject(ValidationControllerFactory)
export class Form{

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

  }
}






