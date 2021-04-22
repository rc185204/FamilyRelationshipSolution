
import history from '../history/History';
import config from '../config/config.json';
import cookie from 'react-cookies'

/**
 * 跨域访问api数据
 * @param {*} method 
 * @param {*} url 
 * @param {*} body 
 * @returns 
 */
function request(method, url, body) {
    console.log('request:', url);
    method = method.toUpperCase();
    if (method === 'GET') {
        body = undefined;
    } else {
        body = body && JSON.stringify(body);
    }
    url = config.apiService + url;
    let token = cookie.load('access_token');
    return fetch(url, {
        method,
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json', //申明传递数据格式为json
            'api-version': '1.0',
            'Authorization': 'Bearer '+ (token === undefined ? '' : token)
        },
        body,        
        mode:'cors',  
    })
    .then((res) => {
        if (res.status >= 200 && res.status < 300) {
            return res.json();
        }
        else if (res.status === 401) {
            let tokenExpired = res.headers.get('Token-Expired');
            if (tokenExpired){
                let json = JSON.parse('{"Success":false,"ErrorCode":"TOKEN_EXPIRED", "Description": ""}');
                json.Description = "status:("+res.status +") Token Expired.";
                return json;
            }else{
                history.push('/login');
                return Promise.reject('Unauthorized.');
            }
        } else if (res.status === 403) {// 权限不足
            let json = JSON.parse('{"Success":false,"ErrorCode":"Request_Rejected", "Description": ""}');
            json.Description = "status:("+res.status +") server rejected the request.";
            return json;
        } else if (res.status === 404) {//url地址不正确
            let json = JSON.parse('{"Success":false,"ErrorCode":"CONNECTION_NOT_FOUND", "Description": ""}');
            json.Description = "status:("+res.status +") url not found.";
            return json;
        } else {
            let json = JSON.parse('{"Success":false,"ErrorCode":"Unknown_Error", "Description": ""}');
            json.Description = "status:("+res.status +") unknown error, please connect the admin";
            return json;
        }
    })
    .catch(error => {
        console.log('error is', error);
        let json = JSON.parse('{"Success":false,"ErrorCode":"Unknown_Error", "Description": ""}');
        json.Description = "unable to connect server.";
        return json;
    });   
}

/**
 * 刷新token,保证长时间在线操作用户不受token失效限制
 * @returns 
 */
function refreshToken(){
    console.log("refreshToken");
    let token = cookie.load('access_token');
    if (token === undefined)
        return false;
    var RefreshTokenBody={
        AccessToken: token,
        RefreshToken: ''
    };
    var method = 'POST';
    var body = JSON.stringify(RefreshTokenBody);
    var url = config.apiService + "Authentication/RefreshToken";
    //var url = "Authentication/RefreshToken"; // 模拟404
    return fetch(url, {
        method,
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',            
            'api-version': '1.0',
        },
        body,
        mode:'cors',
    }).then((res) => {
        if (res.status >= 200 && res.status < 300) {
            return res.json();
        } else {
            let json = JSON.parse('{"Success":false,"ErrorCode":"Unknown_Error", "Description": ""}');
            json.Description = "status:("+res.status +") unknown error, please connect the admin";
            return json;
        }
    })
    .catch(error => {
        console.log('error is', error);
        let json = JSON.parse('{"Success":false,"ErrorCode":"Unknown_Error", "Description": ""}');
        json.Description = "unable to connect server.";
        return json;
    }); 
}

/**
 * 
 * @param {*} url 
 * @param {*} body 
 * @returns 
 */
function tryget(url, body)
{
    return request('GET', url, body);
}

/**
 * 
 * @param {*} url 
 * @param {*} body 
 * @returns 
 */
function trypost(url, body)
{
    return request('POST', url, body);
}

/**
 * 
 * @param {*} url 
 * @param {*} body 
 * @returns 
 */
function tryput(url, body)
{
    return request('PUT', url, body);
}

function trydelete(url, body)
{
    return request('DELETE', url, body);
}


/**
 * GET
 * @param {*} url 
 * @returns 
 */
export const get = url => {
    return tryget(url).then((res) => {
        console.log("tryget first= " + JSON.stringify(res));
        if (res) {
            if (!res.Success && res.ErrorCode === 'TOKEN_EXPIRED'){
                return refreshToken().then((refreshres => {
                    //console.log("refreshToken:" + JSON.stringify(refreshres));
                    if (refreshres.Success){
                        cookie.save('access_token', refreshres.JsonData.Value.token);
                        return tryget(url);
                    }
                    console.log('refreshToken failed');
                    history.push('/login');
                    return Promise.reject('Unauthorized.');
                }));
            }
            return res; // 这里必须有return,否则会一直挂起
        }
    });
}


/**
 * POST
 * 后一个请求不会把第一个请求覆盖掉。（所以Post用来增资源）
 * @param {*} url 
 * @returns 
 */
export const post = url => {
    return trypost(url).then((res) => {
        console.log("trypost first= " + JSON.stringify(res));
        if (res) {
            if (!res.Success && res.ErrorCode === 'TOKEN_EXPIRED'){
                return refreshToken().then((refreshres => {
                    if (refreshres.Success){
                        cookie.save('access_token', refreshres.JsonData.Value.token);
                        return trypost(url);
                    }
                    console.log('refreshToken failed');
                    history.push('/login');
                    return Promise.reject('Unauthorized.');
                }));
            }
            return res; // 这里必须有return,否则会一直挂起
        }
    });
}

/**
 * PUT
 * 如果两个请求相同，后一个请求会把第一个请求覆盖掉。（所以PUT用来改资源）
 * @param {*} url 
 * @returns 
 */
export const put = url => {
    return tryput(url).then((res) => {
        console.log("tryput first= " + JSON.stringify(res));
        if (res) {
            if (!res.Success && res.ErrorCode === 'TOKEN_EXPIRED'){
                return refreshToken().then((refreshres => {
                    if (refreshres.Success){
                        cookie.save('access_token', refreshres.JsonData.Value.token);
                        return tryput(url);
                    }
                    console.log('refreshToken failed');
                    history.push('/login');
                    return Promise.reject('Unauthorized.');
                }));
            }
            return res; // 这里必须有return,否则会一直挂起
        }
    });
}

/**
 * del
 * @param {*} url 
 * @returns 
 */
 export const del = url => {
    return trydelete(url).then((res) => {
        console.log("trydelete first= " + JSON.stringify(res));
        if (res) {
            if (!res.Success && res.ErrorCode === 'TOKEN_EXPIRED'){
                return refreshToken().then((refreshres => {
                    if (refreshres.Success){
                        cookie.save('access_token', refreshres.JsonData.Value.token);
                        return trydelete(url);
                    }
                    console.log('refreshToken failed');
                    history.push('/login');
                    return Promise.reject('Unauthorized.');
                }));
            }
            return res; // 这里必须有return,否则会一直挂起
        }
    });
}