# FamilyrelationshinAngular

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 13.1.2.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.


安装 Angular CLI
npm install -g @angular/cli
安装最新版本angular
npm install -g @angular/cli@latest

安装指定版本angular
npm install -g @angular/cli@13.0.0

卸载angular
npm uninstall -g @angular/cli

清理缓存
npm cache verify

创建项目
ng new my-app

进入到项目根目录，启动项目：

进入项目
cd my-app

启动项目
ng serve --open
指定端口启动服务
ng serve --port 0 --open

创建组件
ng generate component TodoList
ng generate component flord/TodoList

指定組件屬於app 的 module
ng generate component mycomponent --flat --module=app

创建默认路由
ng new routing-app --routing --defaults
创建自定义路由模块
ng generate module app-routing --flat --module=app

创建可注入的服务
ng generate service user

创建路由守卫
ng generate guard your-guard

ng-zorro-antd