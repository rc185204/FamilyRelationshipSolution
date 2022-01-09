import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { TodoListComponent } from './todo-list/todo-list.component';
import {FormsModule} from '@angular/forms';
import { FirstComponent } from './first/first.component';
import { SecondComponent } from './second/second.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ChildaComponent } from './childa/childa.component';
import { ChildbComponent } from './childb/childb.component';
import { LoginComponent } from './auth/login/login.component';
import { FormControlComponent } from './form-control/form-control.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NameEditorComponent } from './name-editor/name-editor.component';
import { ProfileEditorComponent } from './profile-editor/profile-editor.component';
import {  ConfigService } from './config.service';

@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    TodoListComponent,
    FirstComponent,
    SecondComponent,
    PageNotFoundComponent,
    ChildaComponent,
    ChildbComponent,
    LoginComponent,
    FormControlComponent,
    NameEditorComponent,
    ProfileEditorComponent,
  ],
  imports: [
    BrowserModule,    
    HttpClientModule,
    AppRoutingModule,
    FormsModule,    
    ReactiveFormsModule
  ],
  providers: [
    ConfigService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
