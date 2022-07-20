<template>
  <div class="container">
    <van-cell
      center
      v-for="item in stores.trainees.dataList"
      :key="item.guid"
      :title="item.fullName"
      :value="'手机号:' + item.telephone"
    />
    <div class="button-margin">
      <van-button
        round
        icon="plus"
        type="info"
        size="large"
        @click="formModel.opened = true"
        >添加宝贝信息</van-button
      >
    </div>
    <van-popup
      closeable
      round
      v-model="formModel.opened"
      position="bottom"
      :close-on-click-overlay="false"
      :style="{ height: '55%' }"
    >
      <van-form @submit="addTrainees()">
        <van-field
          style="margin-top: 15px"
          v-model="formModel.fields.fullName"
          label="姓名"
          placeholder="宝贝姓名"
          :rules="[{ required: true, message: '请填写宝贝名称' }]"
        />
        <van-field
          v-model="formModel.fields.telephone"
          type="tel"
          label="手机号"
          placeholder="联系人手机号"
          :rules="[{ required: true, message: '请填写联系人手机号' }]"
        />
        <van-field
          v-model="formModel.fields.age"
          type="digit"
          label="年龄"
          placeholder="宝贝年龄"
        />
        <van-field
          v-model="formModel.fields.address"
          label="联系地址"
          placeholder="联系地址"
        />
        <div style="margin: 16px">
          <van-button round block type="info" native-type="submit"
            >提交</van-button
          >
        </div>
      </van-form>
    </van-popup>
    <footer-tabbar />
  </div>
</template>

<script>
import { getTraineesList, addTrainees } from 'api/trainees'
import { Toast } from 'vant'
import FooterTabbar from 'components/FooterTabbar'

export default {
  name: 'trainees',
  data () {
    return {
      formModel: {
        opened: false,
        fields: {
          guid: '00000000-0000-0000-0000-000000000000',
          fullName: '',
          age: 0,
          telephone: '',
          address: '',
          memo: ''
        }
      },
      stores: {
        trainees: {
          dataList: []
        }
      }
    }
  },
  components: {
    /* [Popup.name]: Popup,
    [Form.name]: Form, */
    FooterTabbar
  },
  computed: {},
  methods: {
    loadTraineesList () {
      getTraineesList().then((res) => {
        console.log(res)
        this.stores.trainees.dataList = res.data.data
      })
    },
    addTrainees () {
      Toast.loading({
        duration: 0,
        message: '正在提交...',
        forbidClick: true
      })
      addTrainees(this.formModel.fields).then((res) => {
        Toast.clear()
        console.log(res)
        if (res.data.code === 200) {
          Toast.success({
            message: '提交成功',
            duration: 1500
          })
          this.loadTraineesList()
        } else {
          Toast.fail({
            message: res.data.message,
            duration: 1500
          })
        }
        this.formModel.opened = false
      })
    }
  },
  mounted () {
    this.loadTraineesList()
  }
}
</script>
<style lang="scss" scoped>
.button-margin {
  margin-top: 50px;
  margin-right: 15px;
  margin-bottom: 0px;
  margin-left: 15px;
}
</style>
