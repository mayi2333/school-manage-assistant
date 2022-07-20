import axios from '@/libs/api.request'

//获取学员课时列表
export const getCourseHourList = (data) => {
  return axios.request({
    url: 'coursehour/list',
    method: 'post',
    data
  });
};
//创建学员课时
export const createCourseHour = (data) => {
  return axios.request({
    url: 'coursehour/create',
    method: 'post',
    data
  });
};
//编辑学员课时信息加载
export const loadCourseHour = (data) => {
  return axios.request({
    url: 'coursehour/edit/' + data.guid,
    method: 'get'
  });
};
// 编辑学员课时信息提交
export const editCourseHour = (data) => {
  return axios.request({
    url: 'coursehour/edit',
    method: 'post',
    data
  });
};
//按科目代码和是否特约班查找学员课时列表数据源
export const isSpecialOfFindCourseHourDataSourceByCourseCode = (code) => {
  return axios.request({
    url: 'coursehour/isspecialOffindbycoursecode/' + code,
    method: 'get'
  });
};
//根据课表Guid和科目代码查询学员课时列表
export const loadCourseHourListByCoursesCheduleGuid = (data) => {
  return axios.request({
    url: 'coursehour/findlistbycoursescheduleguid/' + data.guid + "?code=" + data.code,
    method: 'get'
  });
};