import axios from '@/libs/api.request'

//获取学员列表
export const getTraineesList = (data) => {
  return axios.request({
    url: 'trainees/list',
    method: 'post',
    data
  });
};
//创建学员
export const createTrainees = (data) => {
  return axios.request({
    url: 'trainees/create',
    method: 'post',
    data
  });
};
//编辑学员信息加载
export const loadTrainees = (data) => {
  return axios.request({
    url: 'trainees/edit/' + data.guid,
    method: 'get'
  });
};
// 编辑学员信息提交
export const editTrainees = (data) => {
  return axios.request({
    url: 'trainees/edit',
    method: 'post',
    data
  });
};
// 删除学员
export const deleteTrainees = (ids) => {
  return axios.request({
    url: 'trainees/delete/' + ids,
    method: 'get'
  });
};
// 恢复学员
export const recoverTrainees = (ids) => {
  return axios.request({
    url: 'trainees/recover/' + ids,
    method: 'get',
  });
};

// 按关键字查找学生列表数据源
export const findTraineesDataSourceByKeyword = (data) => {
  return axios.request({
    url: 'trainees/findbykeyword',
    method: 'get',
    params: data
  })
};

//学员绑定ID卡
export const traineesBindingCard = (data) => {
  return axios.request({
    url: 'trainees/traineesbindingcard/' + data.guid + '?card=' + data.card,
    method: 'get',
  })
};

//学员解绑ID卡
export const traineesUnBindingCard = (guid) => {
  return axios.request({
    url: 'trainees/traineesunbindingcard/' + guid,
    method: 'get',
  })
};

//学员绑定人脸信息 imgbase64值为空的时候为解绑
export const traineesBindingFace = (data) => {
  return axios.request({
    url: 'trainees/traineesbindingface',
    method: 'post',
    data
  })
};