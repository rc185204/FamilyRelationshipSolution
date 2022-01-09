import { Component, OnInit } from '@angular/core';

import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-second',
  templateUrl: './second.component.html',
  styleUrls: ['./second.component.css']
})
export class SecondComponent implements OnInit {

  
  login = document.cookie.includes('login2=true');

  constructor( 
    private router: Router,
    ) { 
      
      
    }

  

  ngOnInit(): void {
    if(!this.login){
      this.router.navigate(['/app-authentication']); // 如果没有登录，跳转到登录画面
    }
  }

}
