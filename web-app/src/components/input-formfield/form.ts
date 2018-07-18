import {inject} from 'aurelia-dependency-injection';
import {ValidationControllerFactory, ValidationController, ValidationRules, validateTrigger} from 'aurelia-validation';
import {BootstrapFormRenderer} from './bootstrap-form-renderer';

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




submitFormButton(){
   window.location.href="/results"
  }
}
