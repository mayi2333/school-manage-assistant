import Vue from 'vue'
import 'lib-flexible'
import App from './App.vue'
import router from '@/router'
import store from '@/store'
import 'utils/permission'
import SvgIcon from 'components/SvgIcon'
import '@/icons' // icon
import '@/style/common.scss'
// import { Lazyload } from 'vant'
import defaultSettings from '@/settings'
import wechatAuth from 'utils/wechatauth'
import { Lazyload, Button, Image as VanImage, Col, Row, Cell, CellGroup, Popup, Form, Field } from 'vant'

/**
 * If you don't want to use mock-server
 * you want to use MockJs for mock api
 * you can execute: mockXHR()
 *
 * Currently MockJs will be used in the production environment,
 * please remove it before going online! ! !
 */
import { mockXHR } from '../mock'

if (process.env.NODE_ENV === 'production') {
  mockXHR()
}

/* 有赞组件注入 */
Vue.use(Button)
Vue.use(VanImage)
Vue.use(Col)
Vue.use(Row)
Vue.use(Cell)
Vue.use(CellGroup)
Vue.use(Popup)
Vue.use(Form)
Vue.use(Field)

// options 为可选参数，无则不传
Vue.use(Lazyload)

Vue.component('svg-icon', SvgIcon)

if (process.env.NODE_ENV === 'development' && defaultSettings.vconsole) {
  const VConsole = require('vconsole')
  // eslint-disable-next-line
  const my_console = new VConsole()
}
// var vConsole = new VConsole(option)

// 这里是配置微信公众号的appid
Vue.use(wechatAuth, {
  appid: defaultSettings.wechatauth.appid,
  scope: defaultSettings.wechatauth.scope
})

Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
