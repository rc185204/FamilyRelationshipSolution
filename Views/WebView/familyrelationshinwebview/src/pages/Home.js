import React, { Component } from 'react';
import { Route, } from "react-router-dom";
import { Layout, Menu, Breadcrumb, Avatar, Tooltip } from 'antd';
import cookie from 'react-cookies'
import { UserOutlined, LaptopOutlined, NotificationOutlined ,
    MenuUnfoldOutlined,
    MenuFoldOutlined,
} from '@ant-design/icons';

// import { Role } from '../pages/role/Role';
// import { UserInfo } from '../pages/user/UserInfo';
// import { Files } from '../pages/file/Files';
import history from '../history/History';

const { SubMenu } = Menu;
const { Header, Content, Footer, Sider } = Layout;


export class Home extends Component {

    constructor(props) {
        super(props);

        let access_user = cookie.load('access_user');
        let headimgae =<Avatar style={{ backgroundColor: "#7265e6", }} icon={<UserOutlined />} size="large" />
        if (access_user.HeaderImage){
            let value = "data:image/jpeg;base64," + access_user.HeaderImage;
            headimgae = <Avatar style={{ backgroundColor: "#7265e6", }}  src={value} size="large" />
        }
        this.state = {
            name: access_user.UserTrueName,            
            collapsed: false,
            content:'',
            headIco:  headimgae
        };
    }


    toggle = () => {
        this.setState({collapsed: !this.state.collapsed,});
    };

    hide = () => {
        this.setState({visible: false,});
    };

    contentChang=()=>{
        let component = '';
        this.setState({content: component});
    }

    menuSelect = (item) => {
        console.log("menuSelect: key=" + item.key);
        history.push({ pathname: item.key, state: {} });
    }

    
      
    render() {
        return (
            <Layout>
                <Header className="header">
                    <div className="logo" />
                   

                    {/* <Menu theme="dark" mode="horizontal">
                        <Menu.Item key="Account Magage 1.1">Account Magage 1.1</Menu.Item>
                    </Menu> */}
                    <Tooltip title= {this.state.name} placement="top">
                        {this.state.headIco}
                    </Tooltip>


                </Header>



                <Content style={{ padding: '0 50px' }}>
                    <Breadcrumb style={{ margin: '16px 0' }}>
                        <Breadcrumb.Item><a href="/home">Home</a></Breadcrumb.Item>
                        <Breadcrumb.Item>List</Breadcrumb.Item>
                        <Breadcrumb.Item>App</Breadcrumb.Item>
                    </Breadcrumb>

                    <Layout className="site-layout-background" style={{ padding: '24px 0' }}>
                        <Sider className="site-layout-background" width={200} trigger={null} collapsible collapsed={this.state.collapsed}>
                            <Menu
                                theme ='dark'
                                mode="inline"
                                defaultSelectedKeys={['1']}
                                defaultOpenKeys={['sub1']}
                                style={{ height: '100%' }}
                                onSelect={this.menuSelect}
                                >

                                {React.createElement(this.state.collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
                                className: 'trigger',
                                onClick: this.toggle,
                                })}


                                <SubMenu key="sub1" icon={<UserOutlined />} title="subnav 1">
                                    <Menu.Item key="/home/role">Roles</Menu.Item>
                                    <Menu.Item key="/home/userinfo">UserInfo</Menu.Item>
                                    <Menu.Item key="/home/files">Files</Menu.Item>
                                    <Menu.Item key="4">option4</Menu.Item>
                                </SubMenu>
                                <SubMenu key="sub2" icon={<LaptopOutlined />} title="subnav 2">
                                    <Menu.Item key="5">option5</Menu.Item>
                                    <Menu.Item key="6">option6</Menu.Item>
                                    <Menu.Item key="7">option7</Menu.Item>
                                    <Menu.Item key="8">option8</Menu.Item>
                                </SubMenu>
                                <SubMenu key="sub3" icon={<NotificationOutlined />} title="subnav 3">
                                    <Menu.Item key="9">option9</Menu.Item>
                                    <Menu.Item key="10">option10</Menu.Item>
                                    <Menu.Item key="11">option11</Menu.Item>
                                    <Menu.Item key="12">option12</Menu.Item>
                                </SubMenu>
                            </Menu>
                        </Sider>

                        {/* <Content style={{ padding: '0 24px', minHeight: 280 }}>
                            <Route path="/home/role" exact component={Role} />
                            <Route path="/home/userinfo" exact component={UserInfo} />
                            <Route path="/home/files" exact component={Files} />
                        </Content> */}
                    </Layout>
                </Content>
                <Footer style={{ textAlign: 'center' }}>Ant Design ©2018 Created by Ant UED</Footer>
            </Layout>
        )
    }
}
