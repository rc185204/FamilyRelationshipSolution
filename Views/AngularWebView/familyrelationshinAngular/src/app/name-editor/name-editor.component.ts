import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-name-editor',
  templateUrl: './name-editor.component.html',
  styleUrls: ['./name-editor.component.css']
})
export class NameEditorComponent implements OnInit {
  name2 = new FormControl('');
  constructor() { }

  ngOnInit(): void {
  }

  updateName() {
    this.name2.setValue('Nancy');
  }

  getCurrentName(){
    console.log('name = '+this.name2.value);
  }
}
