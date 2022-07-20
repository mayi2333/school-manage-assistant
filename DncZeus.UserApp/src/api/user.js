import _axios from 'axios'
import axios from '@/utils/api.request'
import settings from '@/settings'
// import request from '@/utils/request'

const authUrl = process.env.NODE_ENV === 'development' ? settings.authUrl.dev : settings.authUrl.pro
// const authUrl = process.env.VUE_APP_AUTH_URL
/* export function login (data) {
  return request({
    url: '/user/login',
    method: 'post',
    data
  })
} */
export const weChatAuth = (code) => {
  return _axios.get(authUrl + '?code=' + code)
}

export const getUserInfo = () => {
  return axios.request({
    url: 'account/profile',
    method: 'get',
    // 是否在请求资源中添加资源的前缀
    withPrefix: false, // 设置为true或者不设置此属性，将默认添加配置文件的前缀，如果设置下面这个属性[prefix]，默认配置文件中的默认前缀将不生效
    // 请求资源的前缀重写
    prefix: 'api/v1/' // 设此属性权重最高，将覆盖配置文件中的前缀，withPrefix对此属性不起作用(也就是说只要设置了此属性，都将在请求中添加设置的前缀)
  })
}

/* export function getInfo (token) {
  return request({
    url: '/user/info',
    method: 'get',
    params: { token }
  })
} */
