<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.classgrade.query.totalCount"
        :pageSize="stores.classgrade.query.pageSize"
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
                      v-model="stores.classgrade.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchClassGrade()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.classgrade.query.isDeleted"
                        @on-change="handleSearchClassGrade"
                        placeholder="删除状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.classgrade.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.classgrade.query.isFull"
                        @on-change="handleSearchClassGrade"
                        placeholder="班级分配状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.classgrade.sources.isFullSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.classgrade.query.isSpecial"
                        @on-change="handleSearchClassGrade"
                        placeholder="特约班状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.classgrade.sources.isSpecialSources"
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
                  v-can="'classgrade_create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增班级"
                >新增班级</Button>
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
            :data="stores.classgrade.data"
            :columns="stores.classgrade.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
          <template slot-scope="{row,index}" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
              >
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button v-can="'classgrade_edit'" type="primary" size="small" shape="circle" icon="md-create" @click="handleEdit(row)"></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="500"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form :model="formModel.fields" ref="formClassGrade" :rules="formModel.rules" label-position="top">
        <Row :gutter="16">
          <Col span="15">
            <FormItem label="班级名称" prop="className">
              <Input v-model="formModel.fields.className" placeholder="请输入班级名称"/>
            </FormItem>
          </Col>
          <Col span="9">
            <FormItem label="是特约班">
              <i-switch
                size="large"
                v-model="formModel.fields.isSpecial"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="15">
            <FormItem prop="courseCode">
              <Select
                v-model="formModel.fields.courseCode"
                filterable
                remote
                :remote-method="handleLoadCourseSubjectDataSource"
                :loading="stores.classgrade.sources.courseSubjectSources.loading"
                placeholder="请输入关键字搜索课程科目..."
                v-bind:disabled="formModel.courseCodeIsDisabled"
              >
                <Option
                  v-for="(item, index) in stores.classgrade.sources.courseSubjectSources.data"
                  :value="item.code"
                  :key="index"
                >{{item.courseName + ' ' + item.code}}</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="9">
            <FormItem>
              <span>当前科目代码: {{formModel.fields.courseCode}}</span>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="额定人数" prop="totalPeople">
              <InputNumber :min="1" v-model="formModel.fields.totalPeople"></InputNumber>
            </FormItem>
          </Col>
        </Row>
        <FormItem label="备注" label-position="top">
          <Input type="textarea" v-model="formModel.fields.memo" :rows="4" placeholder="班级备注信息"/>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitClassGrade">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getClassGradeList,
  createClassGrade,
  loadClassGrade,
  editClassGrade,
  deleteClassGrade,
  recoverClassGrade,
} from "@/api/教务中心/classgrade";
import { findCourseSubjectDataSourceByKeyword } from "@/api/教务中心/coursesubject";
export default {
  name: "classgrade_page",
  components: {
    DzTable
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "创建班级",
        mode: "create",
        selection: [],
        courseCodeIsDisabled: false,//当班级还有绑定的学员时 禁止修改班级绑定的课程编码
        fields: {
          guid: "00000000-0000-0000-0000-000000000000",
          className: "",
          totalPeople: 0,
          courseCode: "",
          isSpecial: 0,
          memo: ""
        },
        rules: {
          className: [
            { type: "string", required: true, message: "请输入班级名称", min: 1 }
          ],
          courseCode: [
            { type: "string", required: true, message: "请选择课程科目" }
          ],
        }
      },
      stores: {
        classgrade: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isDeleted: 0,
            isFull: -1,
            isSpecial: -1,
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
            isFullSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "未满员" },
              { value: 1, text: "已满员" }
            ],
            isSpecialSources:[
              { value: -1, text: "全部" },
              { value: 0, text: "不是特约班" },
              { value: 1, text: "是特约班" }
            ],
            courseSubjectSources: {
              loading: false,
              data: []
            },
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "班级名称", key: "className", width: 200, sortable: true },
            { title: "额定人数", key: "totalPeople", width: 85, sortable: true},
            { title: "当前人数", key: "traineesCount", width: 70},
            { title: "是特约班", key: "isSpecial", width:70, 
              render: (h, params) => {
                let isSpecial = params.row.isSpecial;
                let statusColor = "success";
                let statusText = "未知";
                let tooltipText = "";
                switch (isSpecial) {
                  case 0:
                    statusText = "否";
                    statusColor = "default";
                    tooltipText = "非特约班按班级分配课表"
                    break;
                    case 1:
                    statusText = "是";
                    statusColor = "primary";
                    tooltipText = "特约班按照学员分配课表"
                    break;
                }
                return h(
                  "Tooltip",
                  {
                    props: {
                      placement: "top",
                      transfer: true,
                      delay: 500
                    }
                  },
                  [
                    //这个中括号表示是Tooltip标签的子标签
                    h(
                      "Tag",
                      {
                        props: {
                          //type: "dot",
                          color: statusColor
                        }
                      },
                      statusText
                    ), //表格列显示文字
                    h(
                      "p",
                      {
                        slot: "content",
                        style: {
                          whiteSpace: "normal"
                        }
                      },
                      tooltipText //整个的信息即气泡内文字
                    )
                  ]
                );
              }
            },
            { title: "课程编码", width: 100, key: "courseCode", sortable: true },
            { title: "课程名称", width: 220, ellipsis: true, tooltip: true, key: "courseName" },
            { title: "创建时间", width: 120, ellipsis: true, tooltip: true, key: "createdOn",sortable: true },
            { title: "备注", width:120, ellipsis: true, tooltip: true, key: "memo" },
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
        return "创建班级";
      }
      if (this.formModel.mode === "edit") {
        return "编辑班级";
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
    loadClassGradeList() {
      getClassGradeList(this.stores.classgrade.query).then(res => {
        this.stores.classgrade.data = res.data.data;
        this.stores.classgrade.query.totalCount = res.data.totalCount;
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
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      this.handleResetFormClassGrade();
      if(row.traineesCount > 0){
        this.formModel.courseCodeIsDisabled = true;
      }
      this.doLoadClassGrade(row.guid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadClassGradeList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormClassGrade();
    },
    async handleSubmitClassGrade() {
      let valid = await this.validateClassGradeForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateClassGrade();
        }
        if (this.formModel.mode === "edit") {
          this.doEditClassGrade();
        }
      }
    },
    handleResetFormClassGrade() {
      this.$refs["formClassGrade"].resetFields();
      this.formModel.courseCodeIsDisabled = false;
    },
    doCreateClassGrade() {
      //this.formModel.fields.guid = null;
      createClassGrade(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadClassGradeList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditClassGrade() {
      editClassGrade(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadClassGradeList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    async validateClassGradeForm() {
      let _valid = false;
      await this.$refs["formClassGrade"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadClassGrade(guid) {
      loadClassGrade({ guid: guid }).then(res => {
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
      deleteClassGrade(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadClassGradeList();
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
      recoverClassGrade(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadClassGradeList();
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
    handleSearchClassGrade() {
      this.loadClassGradeList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.classgrade.query.sort.direction = column.order;
      this.stores.classgrade.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.classgrade.query.currentPage = page;
      this.loadClassGradeList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.classgrade.query.pageSize = pageSize;
      this.loadClassGradeList();
    },
    handleLoadCourseSubjectDataSource(keyword) {
      //if (this.formModel.fields.courseCode && keyword !== this.formModel.fields.courseCode) {
      //  this.formModel.fields.courseCode = '';
      //  return;
      //}
      this.stores.classgrade.sources.courseSubjectSources.loading = true;
      let query = { keyword: keyword };
      findCourseSubjectDataSourceByKeyword(query).then(res => {
        this.stores.classgrade.sources.courseSubjectSources.data = res.data.data;
        this.stores.classgrade.sources.courseSubjectSources.loading = false;
      });
    }
  },
  mounted() {
    this.loadClassGradeList();
  }
};
</script>

<style>
</style>
