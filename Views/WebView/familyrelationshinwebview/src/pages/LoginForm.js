import React, { Component } from 'react';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import history from '../history/History';
import { get } from '../utils/request';
import cookie from 'react-cookies'
import config from '../config/config.json';
import '../styles/login.scss';


export class LoginForm extends Component {
    constructor(props) {
        super(props);
        this.login = this.login.bind(this);
        this.state = { error: '', token: 0, name: '', pwd: '', remember: true, loading: false };
    }

    componentDidMount() {
        cookie.save('login', false);
    }

    onChangeName = e => {
        this.setState({ name: e.target.value });
    }
    onChangePwd = e => {
        this.setState({ pwd: e.target.value });
    }

    onChangeRemember = e => {
        this.setState({ remember: e.target.checked });
    }

    onLogin = () => {
        this.login(this.state.name, this.state.pwd, this.state.remember);
    };

    onFinish = (values) => {
        console.log('Received values of form: ', values);
        this.login(values.username, values.password, values.rember);
    };

    async login (username, passworld, rember) {
        //var login = false;
        this.setState({ loading: true }); 
        var url ='Authentication?uname='+username+'&pwd='+passworld;
        get(url)
        .then((res) => {
            if (res) {
                console.log("res= " + JSON.stringify(res));
                this.setState({ loading: false });
                if (!res.Success) {
                    this.setState({ error: res.Description});
                } else {
                    this.setState({ error: 'login success' });
                    // 使用Cookie保存登录信息
                    let inFifteenMinutes = new Date(new Date().getTime() + (config.cookieKeepdays * 24 * 60 * 60 * 1000));
                    //cookie.remove('access_user');
                    cookie.save('access_user', res.JsonData.Value.user, { expires: inFifteenMinutes });
                    cookie.save('access_token', res.JsonData.Value.token, { expires: inFifteenMinutes });
                    cookie.save('login', true, { expires: inFifteenMinutes });
                    if (rember){
                        cookie.save('access_username', username, { expires: inFifteenMinutes });
                        cookie.save('access_passworld', passworld, { expires: inFifteenMinutes });
                    }
                    this.jumpBack();
                }
            }
        });
    }


    jumpBack = () => {        
        const { location } = this.props
        const from = location.state && location.state.from
        let RedirectUrl = from ? from: '/home'; 
        history.push({ pathname: RedirectUrl, state: {} });
    }
  

    render() {
        return (
            <div id = "login">
                <div className="login-wrap">
                    <Form
                        name="normal_login"
                        className="login-form"
                            initialValues={{
                                remember: true,
                            }}
                            onFinish={this.onFinish}
                        >
                            <Form.Item
                            name="username"
                            style={{ width: '300px' }}
                                rules={[
                                    {
                                        required: true,
                                        message: 'Please input your Username!',
                                    },
                                ]}
                            >
                            <Input 
                                prefix={<UserOutlined style={{ color: 'rgba(0,0,0,.25)' }} />}
                                placeholder="Username" autoComplete="off" autoFocus
                                onChange={this.onChangeName}
                            />
                            </Form.Item>
                            <Form.Item
                                name="password"
                                rules={[
                                    {
                                        required: true,
                                        message: 'Please input your Password!',
                                    },
                                ]}
                            >
                            <Input className="login-form-item"
                                prefix={<LockOutlined style={{ color: 'rgba(0,0,0,.25)' }} />}
                                type='password'
                                placeholder="Password"
                                onChange={this.onChangePwd}
                                />
                            </Form.Item>
                            <Form.Item>
                            <Form.Item name="remember" valuePropName="checked" noStyle>
                                <Checkbox onChange={this.onChangeRemember} >Remember me</Checkbox>
                                </Form.Item>

                            </Form.Item>

                            <Form.Item>
                            <Button type="primary" htmlType="submit" className="login-form-button" loading={this.state.loading}>
                                    Log in
                                </Button>
                                {this.state.error}
                            </Form.Item>
                        </Form>
                </div>
            </div>
        )
    }
}
