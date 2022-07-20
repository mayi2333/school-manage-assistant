<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.coursesubject.query.totalCount"
        :pageSize="stores.coursesubject.query.pageSize"
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
                      v-model="stores.coursesubject.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchCourseSubject()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.coursesubject.query.isDeleted"
                        @on-change="handleSearchCourseSubject"
                        placeholder="删除状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.coursesubject.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.coursesubject.query.chargeType"
                        @on-change="handleSearchCourseSubject"
                        placeholder="收费方式"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.coursesubject.sources.chargeTypeSources"
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
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('删除')"
                  ></Button>
                  <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('恢复')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'coursesubject_create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增科目"
                >新增科目</Button>
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
            :data="stores.coursesubject.data"
            :columns="stores.coursesubject.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
          <template slot-scope="{row,index}" slot="price">
            <span>{{renderPrice(row.price)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="chargeType">
            <span>{{renderChargeType(row.chargeType)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
              >
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'coursesubject_delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button v-can="'coursesubject_edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
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
      <Form :model="formModel.fields" ref="formCourseSubject" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="科目编码" prop="code">
              <Input v-model="formModel.fields.code" placeholder="请输入课程科目编码" v-bind:disabled="formModel.editModeInputIsDisabled"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="科目名称" prop="courseName">
              <Input v-model="formModel.fields.courseName" placeholder="请输入课程科目名称"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="8">
          <Col span="12">
            <FormItem label="收费方式" prop="chargeType">
              <Select
                v-model="formModel.fields.chargeType"
                placeholder="请选择收费方式"
              >
                <Option
                  v-for="item in stores.coursesubject.sources.chargeTypeSources"
                  :value="item.value"
                  :key="item.value"
                >{{item.text}}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="课程价格" prop="price">
              <Input type="number" min="0" v-model="formModel.inputPrint" @on-blur="handlePriceInputonBlur()"></Input>
            </FormItem>
          </Col>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitCourseSubject">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCourseSubjectList,
  createCourseSubject,
  loadCourseSubject,
  editCourseSubject,
  deleteCourseSubject,
  recoverCourseSubject,
} from "@/api/教务中心/coursesubject";
export default {
  name: "coursesubject_page",
  components: {
    DzTable
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "创建课程科目",
        mode: "create",
        selection: [],
        editModeInputIsDisabled: false,//控制课程代码输入框是否可以编辑
        inputPrint: 0,
        fields: {
          code: "",
          courseName: "",
          chargeType: -1,
          price: 0,
        },
        rules: {
          code: [
            { type: "string", required: true, message: "请输入科目代码", min: 1 }
          ],
          courseName: [
            { type: "string", required: true, message: "请输入科目名称", min: 1 }
          ],
          chargeType: [],
          price: []
        }
      },
      stores: {
        coursesubject: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            chargeType: -1,
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
            chargeTypeSources: [
              { value: -1, text: "未指定" },
              { value: 0, text: "按课时" },
              { value: 1, text: "按学期" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "科目名称", key: "courseName", width: 220, sortable: true },
            { title: "科目编码", key: "code", width: 150, sortable: true },
            { title: "收费方式", key: "chargeType", width: 120, slot: "chargeType"},
            { title: "课程价格", width: 120, key: "price", slot: "price" },
            { title: "班级数量", width: 100, key: "classGradesCount" },
            { title: "创建时间", width: 130, ellipsis: true, tooltip: true, key: "createdOn",sortable: true },
            { title: "修改时间", width: 130, ellipsis: true, tooltip: true, key: "modifiedOn",sortable: true },
            { title: " ",},
            { title: "操作", align: "center", width: 150, className: "table-command-column",slot:"action" }
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
        return "创建课程科目";
      }
      if (this.formModel.mode === "edit") {
        return "编辑课程科目";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.code);
    }
  },
  methods: {
    loadCourseSubjectList() {
      getCourseSubjectList(this.stores.coursesubject.query).then(res => {
        this.stores.coursesubject.data = res.data.data;
        this.stores.coursesubject.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
      this.formModel.editModeInputIsDisabled = false;
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.formModel.editModeInputIsDisabled = true;
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormCourseSubject();
      this.doLoadCourseSubject(row.code);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadCourseSubjectList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormCourseSubject();
    },
    async handleSubmitCourseSubject() {
      let valid = await this.validateCourseSubjectForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateCourseSubject();
        }
        if (this.formModel.mode === "edit") {
          this.doEditCourseSubject();
        }
      }
    },
    handleResetFormCourseSubject() {
      this.$refs["formCourseSubject"].resetFields();
      this.formModel.inputPrint = 0;
    },
    doCreateCourseSubject() {
      //this.formModel.fields.guid = null;
      createCourseSubject(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseSubjectList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditCourseSubject() {
      editCourseSubject(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseSubjectList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    async validateCourseSubjectForm() {
      let _valid = false;
      await this.$refs["formCourseSubject"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadCourseSubject(guid) {
      loadCourseSubject({ guid: guid }).then(res => {
        this.formModel.fields = res.data.data;
        this.formModel.inputPrint = (res.data.data.price / 100).toFixed(2);
      });
    },
    handleDelete(row) {
      this.doDelete(row.code);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteCourseSubject(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCourseSubjectList();
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
      recoverCourseSubject(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCourseSubjectList();
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
    handleSearchCourseSubject() {
      this.loadCourseSubjectList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.coursesubject.query.sort.direct = column.order;
      this.stores.coursesubject.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.coursesubject.query.currentPage = page;
      this.loadCourseSubjectList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.coursesubject.query.pageSize = pageSize;
      this.loadCourseSubjectList();
    },
    renderChargeType(chargeType){
      var chargeTypeText = "未指定";
      switch (chargeType) {
        case -1:
          chargeTypeText = "未指定";
          break;
        case 0:
          chargeTypeText = "按课时";
          break;
        case 1:
          chargeTypeText = "按学期";
          break;
      }
      return chargeTypeText;
    },
    renderPrice(price){
      var priceText = (price / 100).toFixed(2) + " 元";
      return priceText
    },
    handlePriceInputonBlur(){
      if(this.formModel.inputPrint > 0){
        this.formModel.fields.price = (this.formModel.inputPrint * 100).toFixed(0);
      }
      else{
        this.formModel.fields.price = 0;
        this.formModel.inputPrint = 0;
      }
    }
    /**handlePriceInputonChange(e){
      console.log(111);
      var val = e.target.value;
      var regPos = /^\d+(\.\d+)?$/; //非负浮点数
      var regNeg = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/; //负浮点数
      if(regPos.test(val) || regNeg.test(val)) {
        formModel.fields.price = (parseFloat(val) * 100).toFixed(0);
      } else {
        e.target.value = '';
        formModel.fields.price = 0;
      }
    }**/
  },
  mounted() {
    this.loadCourseSubjectList();
  }
};
</script>

<style>
</style>
