import { Component, OnInit } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient,HttpHeaders } from '@angular/common/http';

import { Config, ConfigService } from '../config.service';
import {HttpParams} from "@angular/common/http";

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {
  loginResponse: HttpResponse | undefined;
  user: User | undefined;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8','authorization':' Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3NlcmlhbG51bWJlciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2FkbWluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIlN5c3RlbSBhZG1pbiIsIlN5c3RlbSBhZG1pbiJdLCJyZWFsbmFtZSI6InNhZG1pbiIsIm5iZiI6MTY0MDM5ODYxMSwiZXhwIjoxNjQwMzk5NTExLCJpc3MiOiJjaGVucm9uZ2h1YSIsImF1ZCI6ImNoZW5yb25naHVhIn0.o7zDg3K6eBB1mnNT2X9hIAxvgObAAuRLB9MCKfdNNHs' }),
    params: new HttpParams(),
  };
 // ;

  error: any;
  headers: string[] = [];
  config: Config|undefined ;
  

  constructor(
    private http: HttpClient,
    private configService: ConfigService
    ) { }


  ngOnInit(): void {
    this.showConfig();
  }

  login(username: string ,password : string): void {
    //
    // let url = 'https://localhost:44323/api/'+'Authentication?uname='+username+'&pwd='+password;
    // if (this.config != undefined)
    //   url = this.config.apiUrl +'Authentication?uname='+username+'&pwd='+password;

    // 通过参数形式赋值
    let url = 'https://localhost:44323/api/'+'Authentication';
    if (this.config != undefined)
      url = this.config.apiUrl +'Authentication';
    this.httpOptions.params = new HttpParams({fromString: 'uname='+username + '&pwd='+password}); 
      
    this.http.get<HttpResponse>(url, this.httpOptions).subscribe(
      result => {
      this.loginResponse = result;
      console.log("ErrorCode="+this.loginResponse.ErrorCode);
      console.log("Success="+this.loginResponse.Success);
      console.log("Description="+this.loginResponse.Description);
      console.log("JsonValue="+ JSON.stringify( this.loginResponse.JsonData));
     
      if (this.loginResponse.Success){
      // 将
      var jsonData = <JsonData>(this.loginResponse.JsonData.Value);    
      
      console.log("JsonData.value="+ JSON.stringify(jsonData));
      this.user = jsonData.user;
      
      console.log("user="+ this.user.UserName);
      console.log("RoleName="+ this.user.Role.RoleName);
      }
    },
     error => console.error(error)  ) ;

  }

  test():void{
    console.log('test called');
  }

  getData<T>(value:T):T{
    return value;
  }

  clear() {
    this.config = undefined;
    this.error = undefined;
    this.headers = [];
  }

  showConfig() {
    this.configService.getConfig()
      .subscribe(
        (data: Config) => this.config = { ...data }, // success path
        error => this.error = error // error path
      );
  }

  showConfig_v1() {
    this.configService.getConfig_1()
      .subscribe((data: Config) => this.config = {
        debugging: data.debugging,
        apiUrl:  data.apiUrl,
        
      });
  }

  showConfig_v2() {
    this.configService.getConfig()
      // clone the data object, using its known Config shape
      .subscribe((data: Config) => this.config = { ...data });
  }

  /**
   * 获取http的response所有数据
   */
  showConfigResponse() {
    this.configService.getConfigResponse()
      // resp is of type `HttpResponse<Config>`
      .subscribe(resp => {
        // display its headers
        const keys = resp.headers.keys();
        this.headers = keys.map(key =>
          `${key}: ${resp.headers.get(key)}`);

        // access the body directly, which is typed as `Config`.
        this.config = { ...resp.body! };// 从body中拿config
      });
  }

  makeError() {
    this.configService.makeIntentionalError().subscribe(null, error => this.error = error );
  }

}

/**
 * http 请求返回的数据包
 */
interface HttpResponse
{
    ErrorCode:string;
    Description:string;
    /**
     * JsonData 数据 
     * 以JsonData.Value形式存在
     */
    JsonData:{Value:any;};  
    Success:boolean;    
}

/**
 * 根据api的特性 实例化各种请求期望数据类型
 */
interface JsonData
{
  token:string;
  user:User;
}

interface User
{
    UserId:Number

    UserName:string;

    UserTrueName:string;

    Description :string;

    Role:Role;

}

interface Role
{
    RoleId:Number

    RoleName  :string;

    Description :string;
}