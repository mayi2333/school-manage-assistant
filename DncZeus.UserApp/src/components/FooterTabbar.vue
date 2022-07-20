<template>
  <van-tabbar route
              :safe-area-inset-bottom="true">
    <van-tabbar-item icon="home-o"
                     replace
                     to="/personal">首页</van-tabbar-item>
    <van-tabbar-item icon="manager"
                     replace
                     to="/personal">我</van-tabbar-item>
  </van-tabbar>

</template>
<script>
import { Tabbar, TabbarItem } from 'vant'
export default {
  name: 'FooterTabbar',
  components: {
    [Tabbar.name]: Tabbar,
    [TabbarItem.name]: TabbarItem
  }
}
</script>
