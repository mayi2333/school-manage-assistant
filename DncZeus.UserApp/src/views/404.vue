<template>
  <div>
    <van-empty image="error" description="404错误,网页走丢了" />
    <div class="button">
      <van-button size="small" type="primary" to="/">返回首页</van-button>
    </div>
  </div>
</template>
<script>
import { Empty } from 'vant'

export default {
  name: 'page404',
  data () {
    return {
      /* headimgurl:"",
      nickname:"", */
    }
  },
  components: {
    [Empty.name]: Empty
  }
}
</script>

<style lang="scss" scoped>
.button{
  text-align: center;
}
</style>
