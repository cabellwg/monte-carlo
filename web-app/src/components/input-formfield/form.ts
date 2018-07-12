import{inject, NewInstance} from 'aurelia-dependency-injection';
import{ValidationController, ValidationRules} from 'aurelia-validation';
import {required, email} from 'aurelia-validatejs';

@inject(NewInstance.of(ValidationController))
export class Form{

  controller;
  constructor(controller){
    this.controller = controller;
  }

  @required
  validatelabel1 = '';


 submitFormButton(){
   let errors = this.controller.validate();
   


   window.location.href="/results"
 } 

}
ValidationRules
  .ensure('validatelabel1').required()
  .on(this);
