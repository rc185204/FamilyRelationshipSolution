import React from 'react'
import { Route, Redirect } from 'react-router-dom'
import { message,  } from 'antd';



//这个组件将根据登录的情况, 返回一个路由
const PrivateRoute = ({ component: Component, ...props }) => {
     // 结构赋值 将 props 里面的 component 赋值给 Component
    return <Route {...props} render={(p) => {
        const login = document.cookie.includes('login=true')
        if (login) { 
            return <Component />
        } else { 
            // alert("您还没有登录，请您登录!");      
            message.info('your need login.');
            return <Redirect to={{
                pathname: '/login',
                state: {
                    from: p.location.pathname
                }
            }} />
        }
    }} />
}
export default PrivateRoute