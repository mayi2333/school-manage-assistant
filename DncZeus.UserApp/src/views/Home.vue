<template>
  <div class="container">
    <div class="buttons">
      <van-button type="warning"
                  @click="logout">退出登录</van-button>
      <router-link to="/404">
        <svg-icon icon-class="404" />前往404页面</router-link>
    </div>
    <footer-tabbar />
  </div>
</template>

<script>
import { Button, Tabbar, TabbarItem, Swipe, SwipeItem } from 'vant'
import { mapActions, mapMutations, mapState } from 'vuex' // createNamespacedHelpers
import FooterTabbar from 'components/FooterTabbar'
import img from 'assets/webpack.png'
// const { mapActions } = createNamespacedHelpers('test') // 可使用这种方式直接获得test模板

export default {
  name: 'home',
  data () {
    return {
      img,
      value: 1,
      images: [
        'https://img.yzcdn.cn/vant/apple-1.jpg',
        'https://img.yzcdn.cn/vant/apple-2.jpg'
      ],
      iconList: [
        'dashboard',
        'example',
        'eye-open',
        'eye',
        'form',
        'link',
        'nested',
        'password',
        'table',
        'tree',
        'user',
        '404'
      ]
    }
  },
  components: {
    [Button.name]: Button,
    [Tabbar.name]: Tabbar,
    [TabbarItem.name]: TabbarItem,
    [Swipe.name]: Swipe,
    [SwipeItem.name]: SwipeItem,
    FooterTabbar
  },
  computed: {
    ...mapState({
      loading: state => state['@@loading'].effects['test/onePlusAsync']
    })
  },
  methods: {
    add () {
      this.onePlusAsync(this.value)
    },
    addOne () {
      this.onePlus(1)
    },
    // ...mapActions('home', ['initData', 'plusPage', 'initPage']),
    ...mapActions({
      onePlusAsync: 'test/onePlusAsync'
    }),
    ...mapMutations({
      onePlus: 'test/onePlus',
      logout: 'user/LOGOUT'
    })
  }
}
</script>
<style lang="scss" scoped>
.container{
  height: auto;
  width: 100%;
  padding-bottom: 50px;
}
.banner {
  width: 100%;
  height: auto;
  img {
    width: 100%;
    height: auto;
  }
}
.now-value {
  padding: 0 15px;
  box-sizing: border-box;
  font-size: 16px;
  // line-height: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.icon-list {
  margin: 15px;
  font-size: 24px;
  .icon {
    margin: 0 10px;
  }
}
.buttons {
  padding: 0 15px;
  font-size: 18px;
  width: 100%;
  box-sizing: border-box;
  display: flex;
  justify-content: space-around;
  align-items: center;
  a {
    color: #333;
  }
}
</style>
