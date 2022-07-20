import axios from '@/utils/api.request'

export const getTraineesList = () => {
  // console.log(process.env)
  return axios.request({
    url: 'personal/gettraineeslist',
    method: 'get',
    // 是否在请求资源中添加资源的前缀
    withPrefix: false, // 设置为true或者不设置此属性，将默认添加配置文件的前缀，如果设置下面这个属性[prefix]，默认配置文件中的默认前缀将不生效
    // 请求资源的前缀重写
    prefix: 'api/v1/' // 设此属性权重最高，将覆盖配置文件中的前缀，withPrefix对此属性不起作用(也就是说只要设置了此属性，都将在请求中添加设置的前缀)
  })
}

export const addTrainees = (data) => {
  return axios.request({
    url: 'personal/addtrainees',
    method: 'post',
    data
  })
}
