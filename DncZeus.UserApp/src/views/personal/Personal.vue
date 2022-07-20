<template>
  <div class="container">
    <van-row>
      <div class="headinfo">
        <van-col span="8"
          ><van-image
            round
            fit="cover"
            width="100"
            height="100"
            :src="headimgurl"
            v-if="headimgurl"
        /></van-col>
        <van-col span="16">{{ nickname }}</van-col>
      </div>
    </van-row>
    <van-row>
      <van-grid clickable :column-num="3">
        <van-grid-item icon="user-circle-o" text="宝贝信息" to="/trainees" />
        <van-grid-item icon="gift-o" text="领取试课" to="/" />
        <van-grid-item icon="todo-list-o" text="课表信息" to="/" />
        <van-grid-item icon="orders-o" text="订单管理" to="/" />
      </van-grid>
    </van-row>
    <van-row>
      <div class="button-margin">
        <van-button type="warning" size="large" @click="logout"
          >退出登录</van-button
        >
      </div>
    </van-row>
    <footer-tabbar />
  </div>
</template>

<script>
import { Grid, GridItem } from 'vant'
// import { mapActions, mapMutations, mapState, mapGetters } from 'vuex' // createNamespacedHelpers
import { mapMutations, mapState } from 'vuex'
import FooterTabbar from 'components/FooterTabbar'

export default {
  name: 'personal',
  data () {
    return {
      /* headimgurl:"",
      nickname:"", */
    }
  },
  components: {
    [Grid.name]: Grid,
    [GridItem.name]: GridItem,
    /* //[Tabbar.name]: Tabbar,
    //[TabbarItem.name]: TabbarItem, */
    FooterTabbar
  },
  computed: {
    ...mapState({
      headimgurl: (state) => state.user.user.headimgurl,
      nickname: (state) => state.user.user.name
    })
    /* ...mapGetters(["userData"]),
    headimgurl = userData.headimgurl,
    nickname = userData.name */
  },
  methods: {
    ...mapMutations({
      logout: 'user/LOGOUT'
    })
  }
}
</script>
<style lang="scss" scoped>
.headinfo {
  height: 120px;
  margin: 10px;
  font-size: 18px;
  text-align: center;
}
.button-margin {
  margin-top: 50px;
  margin-right: 15px;
  margin-bottom: 0px;
  margin-left: 15px;
}
</style>
