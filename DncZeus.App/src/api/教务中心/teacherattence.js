import axios from '@/libs/api.request'

//获取教师上课记录列表
export const getTeacherAttenceList = (data) => {
  return axios.request({
    url: 'teacherattence/list',
    method: 'post',
    data
  });
};

//设置代课教师
export const setSubstituteTeacher = (data) => {
  return axios.request({
    url: 'teacherattence/setsubstituteteacher/' + data.attenctGuid + '?teacherGuid=' + data.teacherGuid,
    method: 'get',
  });
};