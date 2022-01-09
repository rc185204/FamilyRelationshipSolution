import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { FormArray } from '@angular/forms';

@Component({
  selector: 'app-profile-editor',
  templateUrl: './profile-editor.component.html',
  styleUrls: ['./profile-editor.component.css']
})
export class ProfileEditorComponent implements OnInit {
  profileForm = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
  });

  // 更加复杂的表达数据数组
  profileFormarray = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    address: new FormGroup({
      street: new FormControl(''),
      city: new FormControl(''),
      state: new FormControl(''),
      zip: new FormControl('')
    })
  });

  //使用 FormBuilder 服务生成控件, 使得上一种方式变得简便
  profileFormBuilder = this.fb.group({
    firstName: ['', Validators.required],//必输项
    lastName: [''],
    address: this.fb.group({
      street: [''],
      city: [''],
      state: [''],
      zip: ['']
    }),
  });

  //动态表单
  profileFormBuilderArray = this.fb.group({
    firstName: ['', Validators.required],
    lastName: [''],
    address: this.fb.group({
      street: [''],
      city: [''],
      state: [''],
      zip: ['']
    }),
    aliases: this.fb.array([ // 动态表单数组
      this.fb.control('')
    ])
  });

  get aliases() {
    return this.profileFormBuilderArray.get('aliases') as FormArray;
  }

  // 组件的构造函数中添加的一般是可注入的服务，
  // 这里的FormBuilder是由ReactiveFormModule提供的
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit() {
    // TODO: Use EventEmitter with form value
    console.warn("this.profileForm.value.firstName="+this.profileForm.value.firstName);
    console.warn("this.profileForm.value.lastName="+this.profileForm.value.lastName);
  }




  updateProfile() {
    this.profileFormarray.patchValue({
      firstName: 'Nancy',
      address: {
        street: '123 Drew Street'
      }
    });
  }

  updateProfilebuilder() {
    this.profileFormBuilder.patchValue({
      firstName: 'Nancy',
      address: {
        street: '123 Drew Street'
      }
    });
  }

  addAlias() {
    this.aliases.push(this.fb.control(''));
  }

}
