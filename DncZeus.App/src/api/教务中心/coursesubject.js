import axios from '@/libs/api.request'

//获取课程科目列表
export const getCourseSubjectList = (data) => {
  return axios.request({
    url: 'coursesubject/list',
    method: 'post',
    data
  });
};
//创建课程科目
export const createCourseSubject = (data) => {
  return axios.request({
    url: 'coursesubject/create',
    method: 'post',
    data
  });
};
//编辑课程科目信息加载
export const loadCourseSubject = (data) => {
  return axios.request({
    url: 'coursesubject/edit/' + data.guid,
    method: 'get'
  });
};
// 编辑课程科目信息提交
export const editCourseSubject = (data) => {
  return axios.request({
    url: 'coursesubject/edit',
    method: 'post',
    data
  });
};
// 删除课程科目
export const deleteCourseSubject = (ids) => {
  return axios.request({
    url: 'coursesubject/delete/' + ids,
    method: 'get'
  });
};
// 恢复课程科目
export const recoverCourseSubject = (ids) => {
  return axios.request({
    url: 'coursesubject/recover/' + ids,
    method: 'get',
  });
};

// 按关键字查找课程科目数据源
export const findCourseSubjectDataSourceByKeyword = (data) => {
  return axios.request({
    url: 'coursesubject/findbykeyword/' + data.keyword,
    method: 'get'
  })
};