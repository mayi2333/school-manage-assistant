import axios from '@/libs/api.request'

//获取学员考勤记录列表
export const getTraineesAttenceList = (data) => {
  return axios.request({
    url: 'traineesattence/list',
    method: 'post',
    data
  });
};
//加载签到学员数据
export const loadSignInTrainees = (guid) => {
  return axios.request({
    url: 'traineesattence/signintrainees/' + guid,
    method: 'get',
  });
};
//提交签到学员数据
export const saveSignInTrainees = (data) => {
  return axios.request({
    url: 'traineesattence/signintrainees',
    method: 'post',
    data
  });
};
//扣除学员课时
export const deductCourseHour = (guid) => {
  return axios.request({
    url: 'traineesattence/deductcoursehour/' + guid,
    method: 'get',
  });
};