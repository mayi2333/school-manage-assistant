module.exports = {

  title: 'H5Vue',
  wechatauth: {
    appid: 'wx1e88d517b6e0d890',
    // appsecret:'',
    scope: 'snsapi_userinfo' // 'snsapi_base', 'snsapi_userinfo'
  },
  /**
   * @description api请求基础路径
   */
  baseUrl: {
    dev: 'http://192.168.2.86:54321/',
    pro: 'http://localhost:54321/',
    defaultPrefix: 'api/v1/'
  },
  authUrl: {
    dev: 'http://192.168.2.86:54321/api/oauth/wechatauth',
    pro: 'http://localhost:54321/api/oauth/wechatauth'
  },
  /**
   * @type {boolean} true | false
   * @description Whether fix the header
   */
  fixedHeader: false,
  vconsole: true,
  needPageTrans: true,

  /**
   * @type {boolean} true | false
   * @description Whether show the logo in sidebar
   */
  sidebarLogo: false
}
