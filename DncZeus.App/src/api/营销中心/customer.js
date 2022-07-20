import axios from '@/libs/api.request'

//获取客户列表
export const getCustomerList = (data) => {
  return axios.request({
    url: 'customer/list',
    method: 'post',
    data
  });
};