import axios from '@/libs/api.request'

//刷卡签到
export const signInByCard = (card) => {
    return axios.request({
        url: 'signindesk/signinbycard/' + card,
        method: 'get',
    });
};

//手动提交教师签到
export const saveSignInTeacher = (guid) => {
    return axios.request({
        url: 'signindesk/signinteacher/' + guid,
        method: 'get',
    });
};

//刷脸签到
export const signInByFace = (data) => {
    return axios.request({
        url: 'signindesk/signinbyface',
        method: 'post',
        data,
    });
};