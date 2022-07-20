<template>
  <div>
    <Card>
      <Row>
        <Col span="17">
          <Tabs v-model="stores.signindesk.mode" @on-click="handleTabsOnClick">
            <TabPane label="人脸考勤" icon="ios-contact" name="face-view">
              <Card>
                <Row>
                  <Col span="20">
                    <FaceScan
                      :videoWidth="600"
                      :videoHeight="380"
                      ref="faceScan"
                      @faceupload="handleFaceScan"
                    ></FaceScan>
                  </Col>
                  <Col span="4">
                    <Button @click="handleAlterCamera">切换摄像头</Button>
                    <br /><br />
                    <Button @click="handleOpenCamera">打开摄像头</Button>
                    <br /><br />
                    <Button @click="handleCloseCamera">关闭摄像头</Button>
                  </Col>
                </Row>
              </Card>
            </TabPane>
            <TabPane label="刷卡考勤" icon="ios-card" name="card-view">
              <Card>
                <div style="text-align: center">
                  <img width="450px" src="../../assets/images/刷卡.svg" />
                  <h3>请刷卡(刷卡功能仅在Windows系统下有效)</h3>
                  <Spin
                    size="large"
                    fix
                    v-if="stores.signindesk.cardScanFlag"
                  ></Spin>
                </div>
              </Card>
            </TabPane>
            <TabPane label="手动考勤" icon="md-list-box" name="list-view">
              <Card>
                <List>
                  <ListItem
                    v-for="(item, index) in stores.signindesk.data.listItem"
                    :key="index"
                  >
                    <ListItemMeta
                      :title="item.title"
                      :description="item.description"
                    />
                    <template slot="action">
                      <li>
                        <Button
                          type="default"
                          :loading="loadingDic[index]"
                          @click="
                            handleButtonSignInTeacherOnClick(
                              item.courseScheduleGuid,
                              index
                            )
                          "
                          >教师签到</Button
                        >
                      </li>
                      <li>
                        <Button
                          type="default"
                          @click="
                            handleButtonSignInTraineesOnClick(
                              item.courseScheduleGuid,
                              item.title
                            )
                          "
                          >学员签到</Button
                        >
                      </li>
                    </template>
                  </ListItem>
                </List>
              </Card>
            </TabPane>
          </Tabs>
        </Col>
        <Col span="1">
          <Divider type="vertical" />
        </Col>
        <Col span="6">
          <Card dis-hover>
            <p slot="title">
              <Icon type="ios-pie-outline"></Icon>
              数据统计
            </p>
            <p>
              今日待签到教师
              {{ stores.signindesk.data.statis.readyToSignInTeacher }} 人
            </p>
            <p>
              今日已签到教师
              {{ stores.signindesk.data.statis.signInTeacher }} 人
            </p>
            <p>
              今日待签到学员
              {{ stores.signindesk.data.statis.readyToSignInTrainees }} 人
            </p>
            <p>
              今日已签到学员
              {{ stores.signindesk.data.statis.signInTrainees }} 人
            </p>
          </Card>
        </Col>
      </Row>
    </Card>
    <Drawer
      :title="formSignInTrainees.title"
      v-model="formSignInTrainees.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <Form>
        <FormItem>
          <Transfer
            :data="formSignInTrainees.traineesAttences"
            :target-keys="formSignInTrainees.ownedTraineesAttences"
            :render-format="renderOwnedTraineesAttences"
            :titles="['未签到学员', '已签到学员']"
            filterable
            :list-style="{ width: '200px', height: '500px' }"
            @on-change="handleOwnedTraineesAttencesChanged"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 15px">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSaveSignInTrainees"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formSignInTrainees.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <IdCardScan ref="idCardScan" @handle="handleIdCardScan"></IdCardScan>
  </div>
</template>
<script>
import IdCardScan from "_c/idcardscan/idcard-scan.vue";
import FaceScan from "_c/facescan/face-scan.vue";

import {
  getSignInDeskListAndStatis,
  getSignInDeskStatis,
} from "@/api/教务中心/courseschedule";
import {
  loadSignInTrainees,
  saveSignInTrainees,
} from "@/api/教务中心/traineesattence";
import {
  saveSignInTeacher,
  signInByCard,
  signInByFace,
} from "@/api/教务中心/signindesk";
import { handleSpeak } from "@/libs/tools";

export default {
  name: "sign_in_desk_page",
  components: {
    IdCardScan,
    FaceScan,
  },
  data() {
    return {
      formSignInTrainees: {
        opened: false,
        title: "学员签到",
        traineesAttences: [],
        ownedTraineesAttences: [],
        courseScheduleGuid: "",
      },
      stores: {
        signindesk: {
          mode: "face-view",
          cardScanFlag: false,
          //sources: {},
          data: {
            listItem: [],
            statis: {},
          },
        },
      },
      loadingDic: [],
      buttonSignInTeacher: false,
      //buttonSignInTrainees:false,
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
    };
  },
  computed: {},
  methods: {
    loadSignindesk() {
      //this.handleCloseCamera();
      //console.log(this.stores.signindesk.mode);
      if (this.stores.signindesk.mode === "face-view") {
        this.doLoadSignInDeskStatis();
        //this.handleOpenCamera();
      } else if (this.stores.signindesk.mode === "card-view") {
        this.handleCloseCamera();
        this.doLoadSignInDeskStatis();
        this.$refs.idCardScan.bindOnKeyUp();
      } else if (this.stores.signindesk.mode === "list-view") {
        this.handleCloseCamera();
        getSignInDeskListAndStatis().then((res) => {
          this.stores.signindesk.data.listItem = res.data.data.listItem;
          this.stores.signindesk.data.statis = res.data.data.statis;
          //this.loadingDic = [String:Boolean]();
          for (let i = 0; i < res.data.data.listItem.length; i++) {
            this.loadingDic[i] = false;
          }
        });
      } else {
        this.$Message.warning("参数错误");
      }
    },
    doLoadSignInDeskStatis() {
      getSignInDeskStatis().then((res) => {
        this.stores.signindesk.data.statis = res.data.data;
      });
    },
    handleTabsOnClick() {
      this.loadSignindesk();
    },
    handleButtonSignInTeacherOnClick(guid, index) {
      this.$set(this.loadingDic, index, true);
      saveSignInTeacher(guid).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadSignindesk();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$set(this.loadingDic, index, false);
      });
    },
    handleButtonSignInTraineesOnClick(guid, title) {
      this.formSignInTrainees.title = "学员签到-" + title;
      this.formSignInTrainees.courseScheduleGuid = guid;
      this.formSignInTrainees.traineesAttences = [];
      this.formSignInTrainees.ownedTraineesAttences = [];
      loadSignInTrainees(guid).then((res) => {
        if (res.data.code === 200) {
          this.formSignInTrainees.traineesAttences = res.data.data.trainees;
          this.formSignInTrainees.ownedTraineesAttences =
            res.data.data.attendTrainees;
          this.formSignInTrainees.opened = true;
        } else {
          this.$Message.warning(res.data.message);
        }
      });
      //this.loadSignindesk();
    },
    renderOwnedTraineesAttences(item) {
      return item.label;
    },
    handleOwnedTraineesAttencesChanged(newTargetKeys, direction, moveKeys) {
      this.formSignInTrainees.ownedTraineesAttences = newTargetKeys;
    },
    handleSaveSignInTrainees() {
      var data = {
        courseScheduleGuid: this.formSignInTrainees.courseScheduleGuid,
        attendTrainees: this.formSignInTrainees.ownedTraineesAttences,
      };
      saveSignInTrainees(data).then((res) => {
        this.formSignInTrainees.opened = false;
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadSignindesk();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleIdCardScan(cardinfo) {
      //console.log(this.idCardBindDialogModel);
      if (
        this.stores.signindesk.mode === "card-view" &&
        !this.stores.signindesk.cardScanFlag
      ) {
        this.stores.signindesk.cardScanFlag = true;
        signInByCard(cardinfo.card).then((res) => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.loadSignindesk();
          } else {
            this.$Message.warning(res.data.message);
          }
          handleSpeak(res.data.message);
          this.stores.signindesk.cardScanFlag = false;
        });
        console.log("签到台");
      }
    },
    handleAlterCamera() {
      console.log(this.$refs.faceScan);
      this.$refs.faceScan.switchCamera();
    },
    handleOpenCamera() {
      this.$refs.faceScan.openCamera();
    },
    handleCloseCamera() {
      this.$refs.faceScan.closeCamera();
    },
    handleFaceScan(base64) {
      //console.log(data);
      this.$refs.faceScan.clearCanvas();
      this.$Spin.show();
      let data = { img: base64 };
      signInByFace(data).then((res) => {
        this.$Spin.hide();
        //this.$refs.faceScan.isRepeatReset();
        if (res.data.code === 200) {
          this.$Message.success(
            {
              background: true,
              content: res.data.message,
              duration: 8,
              closable: true
            });
          this.loadSignindesk();
        } else {
          this.$Message.warning(
            {
              background: true,
              content: res.data.message,
              duration: 5,
              closable: true
            });
          //this.$Message.warning(res.data.message);
        }
        handleSpeak(res.data.message);
        setTimeout(() => {
          console.log("开始下一次扫描脸部信息")
          this.$refs.faceScan.isReadyReset();
          //this.handleOpenCamera();
        }, 3500);
      });
    },
  },
  mounted() {
    this.loadSignindesk();
  },
};
</script>