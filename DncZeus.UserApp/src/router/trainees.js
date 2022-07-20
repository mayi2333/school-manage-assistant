export default [
  {
    path: '/trainees',
    name: 'trainees',
    component: () => import('views/trainees/Trainees.vue'),
    meta: {
      auth: true,
      title: '宝贝信息',
      keepAlive: true
    }
  }/*,
    {
        path: '/addtrainees',
        name: 'addtrainees',
        component: () => import('views/trainees/AddTrainees.vue'),
        meta: {
            auth: true,
            title: '添加宝贝信息',
            keepAlive: true
        }
    } */
]
