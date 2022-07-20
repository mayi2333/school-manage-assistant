export default [
/*  {
    path: '/login',
    name: 'login',
    component: () => import('views/user/Login.vue'),
    meta: {
      title: '登录'
      // auth: true,
      // keepAlive: true
    }
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('views/user/Register.vue'),
    meta: {
      title: '注册'
      // auth: true,
      // keepAlive: true
    }
  }, */
  {
    path: '/wechatauth',
    name: 'wechatauth',
    component: () => import('views/user/WeChatAuth.vue'),
    meta: {
      title: '微信授权登录',
      auth: false,
      keepAlive: false
    }
  }
]
