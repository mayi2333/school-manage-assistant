<template>
  <div class="container"></div>
</template>

<script>
import { mapActions } from 'vuex'
export default {
  data () {
    return {
      code: '' // 前端获取 code 传给后端调用相应接口
    }
  },
  created () {
    // 从 window.location.href 中截取 code 并且赋值
    this.$wechatAuth.returnFromWechat(window.location.href)
    this.code = this.$wechatAuth.code
    if (this.code) {
      // 存在 code 直接调用接口
      this.handLogin()
    } else {
      this.$wechatAuth.redirectUri = window.location.href
      this.handWeChatAuth()
    }
  },
  methods: {
    handWeChatAuth () {
      // 重定向地址重定到当前页面，在路径获取 code
      // console.log(this.$wechatAuth.redirectUri);
      // console.log(this.$wechatAuth.authUrl);
      // console.log(this.code);
      if (!this.code) {
        window.location.href = this.$wechatAuth.authUrl
      }
    },
    handLogin () {
      // 调用后端接口，参数为 code 剩下工作量交给后端即可
      const data = {
        code: this.code,
        $router: this.$router,
        $route: this.$route
      }
      this.login(data)
    },
    ...mapActions({
      login: 'user/login'
    })
  }
}
</script>
