import axios from '@/libs/api.request'

//获取班级列表
export const getClassGradeList = (data) => {
  return axios.request({
    url: 'classgrade/list',
    method: 'post',
    data
  });
};
//创建班级
export const createClassGrade = (data) => {
  return axios.request({
    url: 'classgrade/create',
    method: 'post',
    data
  });
};
//编辑班级信息加载
export const loadClassGrade = (data) => {
  return axios.request({
    url: 'classgrade/edit/' + data.guid,
    method: 'get'
  });
};
// 编辑班级信息提交
export const editClassGrade = (data) => {
  return axios.request({
    url: 'classgrade/edit',
    method: 'post',
    data
  });
};
// 删除班级
export const deleteClassGrade = (ids) => {
  return axios.request({
    url: 'classgrade/delete/' + ids,
    method: 'get'
  });
};
// 恢复班级
export const recoverClassGrade = (ids) => {
  return axios.request({
    url: 'classgrade/recover/' + ids,
    method: 'get',
  });
};

// 按关键字查找班级列表数据源
export const findClassGradeDataSourceByKeyword = (data) => {
  return axios.request({
    url: 'classgrade/findbykeyword/' + data.keyword,
    method: 'get'
  })
};
// 按科目代码查找班级列表数据源
export const findClassGradeDataSourceByCourseCode = (code) => {
  return axios.request({
    url: 'classgrade/findbycoursecode/' + code,
    method: 'get'
  })
};

// 按科目代码和是否特约班查找班级列表数据源
export const findClassGradeDataSourceByCourseCodeAndIsSpecial = (date) => {
  //console.log(date);
  return axios.request({
    url: 'classgrade/findbycoursecodeandisspecial/' + date.code + "?isspecial=" + date.isspecial,
    method: 'get',
  })
};

// 根据课表Guid和科目代码查询班级列表数据源
export const loadClassGradeListByCoursesCheduleGuid = (date) => {
  //console.log(date);
  return axios.request({
    url: 'classgrade/findlistbycoursescheduleguid/' + date.guid + "?code=" + date.code,
    method: 'get',
  })
};