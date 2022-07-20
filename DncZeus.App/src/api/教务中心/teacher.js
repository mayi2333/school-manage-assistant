import axios from '@/libs/api.request'

//获取教师列表
export const getTeacherList = (data) => {
    return axios.request({
      url: 'teacher/list',
      method: 'post',
      data
    });
  };
  //创建教师
  export const createTeacher = (data) => {
    return axios.request({
      url: 'teacher/create',
      method: 'post',
      data
    });
  };
  //编辑教师信息加载
  export const loadTeacher = (data) => {
    return axios.request({
      url: 'teacher/edit/' + data.guid,
      method: 'get'
    });
  };
  // 编辑教师信息提交
  export const editTeacher = (data) => {
    return axios.request({
      url: 'teacher/edit',
      method: 'post',
      data
    });
  };

  // 按关键字查找教师数据源
export const findTeacherDataSourceByKeyword = (data) => {
  return axios.request({
    url: 'teacher/findbykeyword',
    method: 'get',
    params: data
  })
};

//教师绑定ID卡
export const teacherBindingCard = (data) => {
  return axios.request({
    url: 'teacher/teacherbindingcard/' + data.guid + '?card=' + data.card,
    method: 'get',
  })
};

//教师解绑ID卡
export const teacherUnBindingCard = (guid) => {
  return axios.request({
    url: 'teacher/teacherunbindingcard/' + guid,
    method: 'get',
  })
};

//教师绑定人脸信息 imgbase64值为空的时候为解绑
export const teacherBindingFace = (data) => {
  return axios.request({
    url: 'teacher/teacherbindingface',
    method: 'post',
    data
  })
};