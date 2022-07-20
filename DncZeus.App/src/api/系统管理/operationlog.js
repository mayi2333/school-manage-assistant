import axios from '@/libs/api.request'

//获取操作日志列表
export const getOperationLogList = (data) => {
  return axios.request({
    url: 'operationlog/list',
    method: 'post',
    data
  });
};