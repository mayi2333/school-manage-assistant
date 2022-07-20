export default [
  {
    path: '/personal',
    name: 'personal',
    component: () => import('views/personal/Personal.vue'),
    meta: {
      auth: true,
      title: '个人中心',
      keepAlive: true
    }
  }
]
