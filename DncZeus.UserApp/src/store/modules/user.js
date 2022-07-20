import { weChatAuth, getInfo } from 'api/user'
import { Toast } from 'vant'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { resetRouter } from '@/router'
// import router from '@/router'

const LOGIN = 'LOGIN'// 获取用户信息
const SetUserData = 'SetUserData'// 获取用户信息
const LOGOUT = 'LOGOUT'// 退出登录、清除用户数据
const USER_DATA = 'userDate'// 用户数据

export default {
  namespaced: true,
  state: {
    token: getToken() || '',
    user: JSON.parse(localStorage.getItem(USER_DATA) || null)
  },
  mutations: {

    [LOGIN] (state, data) {
      let userToken = data.token
      state.token = userToken
      setToken(userToken)
    },

    [SetUserData] (state, userData = {}) {
      state.user = userData
      localStorage.setItem(USER_DATA, JSON.stringify(userData))
    },
    [LOGOUT] (state) {
      // state.user = null
      state.token = null
      removeToken()
      localStorage.removeItem(USER_DATA)
      resetRouter()
    }
  },
  actions: {
    async login (state, data) {
      try {
        Toast.loading({
          duration: 0,
          message: '登录中...',
          forbidClick: true
        })
        let res = await weChatAuth(data.code)
        Toast.clear()
        // console.log(res)
        if (res.data.code === 200) {
          state.commit(LOGIN, res.data.data)
          state.commit(SetUserData, res.data.data.user)
          Toast.success({
            message: '登录成功',
            position: 'middle',
            duration: 1500
          })
        } else {
          Toast.fail({
            message: res.data.message,
            duration: 1500
          })
        }
        setTimeout(() => {
          const redirect = data.$route.query.redirect || '/'
          data.$router.replace({
            path: redirect
          })
        }, 500)
      } catch (error) {
        Toast.fail({
          message: error.message,
          duration: 1500
        })
      }
    },
    // get user info
    getInfo ({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo(state.token).then(response => {
          const { data } = response

          if (!data) {
            // eslint-disable-next-line
            reject('Verification failed, please Login again.')
          }
          commit(SetUserData, data)
          resolve(data)
        }).catch(error => {
          reject(error)
        })
      })
    }
  },
  getters: {
    token (state) {
      return state.token
    },
    user (state) {
      return state.user
    }
  }
}
