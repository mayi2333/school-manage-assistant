import axios from '@/libs/api.request'

//获取课表列表
export const getCourseScheduleList = (data) => {
    return axios.request({
      url: 'courseschedule/list',
      method: 'post',
      data
    });
  };
export const getCourseScheduleGrid = (data) =>{
  return axios.request({
    url: 'courseschedule/calendargrid',
    method: 'post',
    data
  });
};
  //创建课表
  export const createCourseSchedule = (data) => {
    return axios.request({
      url: 'courseschedule/create',
      method: 'post',
      data
    });
  };
  //编辑课表信息加载
  export const loadCourseSchedule = (data) => {
    return axios.request({
      url: 'courseschedule/edit/' + data.guid,
      method: 'get'
    });
  };
  // 编辑课表信息提交
  export const editCourseSchedule = (data) => {
    return axios.request({
      url: 'courseschedule/edit',
      method: 'post',
      data
    });
  };
  //保存班级-课表，学员课时-课表 关系映射
  export const saveAssignCoursesChedule = (data) => {
    return axios.request({
      url: 'courseschedule/saveassigncourseschedule',
      method: 'post',
      data
    });
  };
  //获取单个课表详情
  export const getCourseDetail = (guid) => {
    return axios.request({
      url: 'courseschedule/getcoursedetail/' + guid,
      method: 'get',
    });
  };

  //获取签到页面数据列表和统计信息
  export const getSignInDeskListAndStatis = () => {
    return axios.request({
      url: 'courseschedule/getsignindesklistandstatis',
      method: 'get',
    });
  };
  //获取签到页面统计信息
  export const getSignInDeskStatis = () => {
    return axios.request({
      url: 'courseschedule/getsignindeskstatis',
      method: 'get',
    });
  };