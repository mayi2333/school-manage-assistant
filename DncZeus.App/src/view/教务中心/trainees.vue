<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.trainees.query.totalCount"
        :pageSize="stores.trainees.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.trainees.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchTrainees()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.trainees.query.isDeleted"
                        @on-change="handleSearchTrainees"
                        placeholder="删除状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.trainees.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.trainees.query.isBindClassGrade"
                        @on-change="handleSearchTrainees"
                        placeholder="班级分配状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.trainees.sources.isBindClassGradeSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.trainees.query.isBindCourseHour"
                        @on-change="handleSearchTrainees"
                        placeholder="课程购买状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.trainees.sources.isBindCourseHourSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'trainees_delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('删除')"
                  ></Button>
                  <Button
                    v-can="'trainees_recover'"
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('恢复')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="''"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增学员"
                >新增学员</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
            slot="table"
            ref="tables"
            :border="false"
            size="small"
            :highlight-row="true"
            :data="stores.trainees.data"
            :columns="stores.trainees.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
          <template slot-scope="{row,index}" slot="action">
            <Tooltip
              placement="top"
              :content="row.idCardBind ? 'ID卡已绑定' : 'ID卡绑定'"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'trainees_bind_card'"
                :type="row.idCardBind ? 'info' : 'dashed'"
                size="small"
                shape="circle"
                icon="md-card"
                @click="handleBindingCardButtonOnclick(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              :content="row.faceBind ? '人脸信息已绑定' : '人脸信息绑定'"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'trainees_bind_face'"
                :type="row.faceBind ? 'info' : 'dashed'"
                size="small"
                shape="circle"
                icon="ios-contact"
                @click="handleBindingFaceButtonOnclick(row)"
              ></Button>
            </Tooltip>
            <Divider type="vertical" />
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
              >
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'trainees_delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button v-can="'trainees_edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
            </Tooltip>
            
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formTrainees" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="学员姓名" prop="fullName">
              <Input v-model="formModel.fields.fullName" placeholder="请输入学员姓名"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="学员年龄" prop="age">
              <Input type="number" v-model="formModel.fields.age" placeholder="请输入学员年龄"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="联系电话" prop="telephone">
              <Input v-model="formModel.fields.telephone" placeholder="请输入联系电话"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="联系地址" prop="address">
              <Input v-model="formModel.fields.address" placeholder="请输入联系地址"/>
            </FormItem>
          </Col>
        </Row>
        <FormItem label="备注" label-position="top">
          <Input type="textarea" v-model="formModel.fields.memo" :rows="4" placeholder="学员备注信息"/>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitTrainees">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      :title="faceBindDrawerModel.title"
      v-model="faceBindDrawerModel.opened"
      width="500"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Row :gutter="12">
        <Button @click="handleAlterCamera">切换摄像头</Button>
      </Row>
      <Row :gutter="12">
        <FaceScan
          :videoWidth="480"
          :videoHeight="320"
          :canvasIsShow="true"
          ref="faceScan"
          @faceupload="handleFaceScan"
        ></FaceScan>
      </Row>
      <div class="demo-drawer-footer">
        <Row :gutter="16">
          <Col span="6">
            <Button
              icon="md-checkmark-circle"
              type="info"
              @click="handleTakePictureAgain"
              >重 拍</Button
            >
          </Col>
          <Col span="6">
            <Button
              icon="md-checkmark-circle"
              type="warning"
              @click="handleClearFaceImage"
              >清 空</Button
            >
          </Col>
          <Col span="6">
            <Button
              icon="md-checkmark-circle"
              type="primary"
              @click="handleSaveFaceImage"
              >保 存</Button
            >
          </Col>
          <Col span="6">
            <Button
              style="margin-left: 8px"
              icon="md-close"
              @click="handleFaceBindDrawerClose"
              >取 消</Button
            >
          </Col>
        </Row>
      </div>
    </Drawer>
    <el-dialog
      :title="idCardBindDialogModel.title"
      :visible.sync="idCardBindDialogModel.opened"
      width="30%"
      :before-close="handleIdCardBindDialogClose"
    >
      <div>
        <div class="text item">
          请将卡片放在刷卡机上开始识别绑定
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" :loading="idCardBindDialogModel.loading" @click="handleUnBindingCardButtonOnclick"
          >解绑ID卡</el-button
        >
        <el-button @click="handleIdCardBindDialogClose">关 闭</el-button>
      </span>
    </el-dialog>
    <IdCardScan ref="idCardScan" @handle="handleIdCardScan"></IdCardScan>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import IdCardScan from "_c/idcardscan/idcard-scan.vue";
import FaceScan from "_c/facescan/face-scan.vue";

import {
  getTraineesList,
  createTrainees,
  loadTrainees,
  editTrainees,
  deleteTrainees,
  recoverTrainees,
  traineesBindingCard,
  traineesUnBindingCard,
  traineesBindingFace,
} from "@/api/教务中心/trainees";
import { 
  handleSpeak,
} from "@/libs/tools";

export default {
  name: "trainees_page",
  components: {
    DzTable,
    IdCardScan,
    FaceScan,
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "创建学员",
        mode: "create",
        selection: [],
        fields: {
          guid: "00000000-0000-0000-0000-000000000000",
          fullName: "",
          age: 0,
          telephone: "",
          address: "",
          memo: ""
        },
        rules: {
          fullName: [
            { type: "string", required: true, message: "请输入学员姓名", min: 1 }
          ],
          telephone: [
            { type: "string", required: true, message: "请输入联系电话", min: 5 }
          ],
          address: [],
          age: []
        }
      },
      idCardBindDialogModel: {
        title: "ID卡绑定",
        opened: false,
        traineesGuid: "",
        loading: false,
      },
      faceBindDrawerModel: {
        opened: false,
        title: "人脸信息绑定",
        imgBase64: "",
        traineesGuid: "",
      },
      stores: {
        trainees: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            isBindClassGrade: -1,//是否已分配班级
            isBindCourseHour: -1,//是否购买了课时且未上完课
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn"
              }
            ]
          },
          sources: {
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" }
            ],
            isBindClassGradeSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "未分配班级" },
              { value: 1, text: "已分配班级" }
            ],
            isBindCourseHourSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "未购买课程" },
              { value: 1, text: "已购买课程" }
            ],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "学员姓名", key: "fullName", width: 85, sortable: true },
            { title: "年龄", key: "age", width: 60, sortable: true},
            { title: "联系电话", width: 90, key: "telephone" },
            { title: "家庭住址", width: 260, ellipsis: true, tooltip: true, key: "address" },
            { title: "当前课程", ellipsis: true, tooltip: true,width: 110, key: "courseName"},
            { title: "当前班级", ellipsis: true, tooltip: true,width: 150, key: "className"},
            { title: "创建时间", width: 120, ellipsis: true, tooltip: true, key: "createdOn",sortable: true },
            { title: "备注", width: 110,ellipsis: true, tooltip: true, key: "memo" },
            { title: " " },
            { title: "操作", width:140, align: "center", className: "table-command-column",slot:"action" }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建学员";
      }
      if (this.formModel.mode === "edit") {
        return "编辑学员";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.guid);
    }
  },
  methods: {
    loadTraineesList() {
      getTraineesList(this.stores.trainees.query).then(res => {
        this.stores.trainees.data = res.data.data;
        this.stores.trainees.query.totalCount = res.data.totalCount;
      });
      //this.$refs.idCardScan.bindOnKeyUp();
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormTrainees();
      this.doLoadTrainees(row.guid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadTraineesList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormTrainees();
    },
    async handleSubmitTrainees() {
      let valid = await this.validateTraineesForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateTrainees();
        }
        if (this.formModel.mode === "edit") {
          this.doEditTrainees();
        }
      }
    },
    handleResetFormTrainees() {
      this.$refs["formTrainees"].resetFields();
    },
    doCreateTrainees() {
      //this.formModel.fields.guid = null;
      createTrainees(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadTraineesList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditTrainees() {
      editTrainees(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadTraineesList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    async validateTraineesForm() {
      let _valid = false;
      await this.$refs["formTrainees"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadTrainees(guid) {
      loadTrainees({ guid: guid }).then(res => {
        this.formModel.fields = res.data.data;
      });
    },
    handleDelete(row) {
      this.doDelete(row.guid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteTrainees(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadTraineesList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doRecover(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      recoverTrainees(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadTraineesList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          command +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    doBatchCommand(command) {
      if(command == '删除'){
        this.doDelete(this.selectedRowsId.join(","));
      }
      else if(command == '恢复'){
        this.doRecover(this.selectedRowsId.join(","));
      }
      this.$Modal.remove();
    },
    handleSearchTrainees() {
      this.loadTraineesList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.trainees.query.sort.direction = column.order;
      this.stores.trainees.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.trainees.query.currentPage = page;
      this.loadTraineesList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.trainees.query.pageSize = pageSize;
      this.loadTraineesList();
    },
    handleIdCardBindDialogClose() {
      this.idCardBindDialogModel.title = "";
      this.idCardBindDialogModel.traineesGuid = "";
      this.idCardBindDialogModel.opened = false;
      this.idCardBindDialogModel.loading = false;
      //this.idCardBindDialogModel.buttonText = "解绑ID卡";
    },
    handleUnBindingCardButtonOnclick() {
      if(!this.idCardBindDialogModel.loading){
      this.idCardBindDialogModel.loading = true;
      //this.idCardBindDialogModel.buttonText = "正在解绑";
      let guid = this.idCardBindDialogModel.traineesGuid;
      traineesUnBindingCard(guid).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleIdCardBindDialogClose();
          this.loadTraineesList();
        } else {
          this.$Message.warning(res.data.message);
        }
        handleSpeak(res.data.message);
      });
      //this.handleIdCardBindDialogClose();
      this.idCardBindDialogModel.loading = false;
      //this.idCardBindDialogModel.buttonText = "解绑ID卡";
      }
    },
    handleBindingCardButtonOnclick(row) {
      this.idCardBindDialogModel.title = row.fullName + "-ID卡绑定";
      this.idCardBindDialogModel.traineesGuid = row.guid;
      this.idCardBindDialogModel.opened = true;
      this.idCardBindDialogModel.loading = false;
      this.$refs.idCardScan.bindOnKeyUp();
      //this.idCardBindDialogModel.buttonText = "解绑ID卡";
    },
    handleIdCardScan(cardinfo) {
      //console.log(this.idCardBindDialogModel);
      if (
        this.idCardBindDialogModel.opened &&
        !this.idCardBindDialogModel.loading
      ) {
        this.idCardBindDialogModel.loading = true;
        //this.idCardBindDialogModel.buttonText = "正在绑定";
        let data = {
          guid: this.idCardBindDialogModel.traineesGuid,
          card: cardinfo.card,
        };
        traineesBindingCard(data).then((res) => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleIdCardBindDialogClose();
            this.loadTraineesList();
          } else {
            this.$Message.warning(res.data.message);
          }
          handleSpeak(res.data.message);
        });
        this.idCardBindDialogModel.loading = false;
      }
      //console.log("学员页面");
    },
    handleBindingFaceButtonOnclick(row) {
      this.faceBindDrawerModel.title = row.fullName + "-人脸信息绑定";
      this.faceBindDrawerModel.traineesGuid = row.guid;
      this.faceBindDrawerModel.opened = true;
      setTimeout(() => {
        this.$refs.faceScan.openCamera();
      }, 700);
    },
    handleFaceBindDrawerClose() {
      this.$refs.faceScan.clearCanvas();
      this.$refs.faceScan.closeCamera();
      this.faceBindDrawerModel.imgBase64 = "";
      this.faceBindDrawerModel.title = "";
      this.faceBindDrawerModel.opened = false;
    },
    handleAlterCamera() {
      this.$refs.faceScan.switchCamera();
    },
    handleFaceScan(data) {
      this.faceBindDrawerModel.imgBase64 = data;
      //console.log(data);
    },
    handleClearFaceImage() {
      this.faceBindDrawerModel.imgBase64 = "";
      this.$refs.faceScan.clearCanvas();
    },
    handleTakePictureAgain() {
      this.$refs.faceScan.clearCanvas();
      this.$refs.faceScan.isReadyReset();
    },
    handleSaveFaceImage() {
      let data = {
          guid: this.faceBindDrawerModel.traineesGuid,
          img: this.faceBindDrawerModel.imgBase64,
        };
        traineesBindingFace(data).then((res) => {
          if (res.data.code === 200) {
            this.$Message.success(res.data.message);
            this.handleFaceBindDrawerClose();
            this.loadTraineesList();
          } else {
            this.$Message.warning(res.data.message);
          }
          handleSpeak(res.data.message);
        });
    },
  },
  mounted() {
    this.loadTraineesList();
  }
};
</script>

<style>
</style>
