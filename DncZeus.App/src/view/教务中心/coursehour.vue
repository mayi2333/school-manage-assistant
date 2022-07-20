<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.coursehour.query.totalCount"
        :pageSize="stores.coursehour.query.pageSize"
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
                      v-model="stores.coursehour.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchCourseHour()"
                    >
                    </Input>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.coursehour.query.courseCode"
                      clearable
                      filterable
                      remote
                      :remote-method="handleLoadCourseSubjectDataSource"
                      :loading="
                        stores.coursehour.sources.courseSubjectSources.loading
                      "
                      placeholder="选择课程科目..."
                      @on-change="handleSearchCourseHour()"
                    >
                      <Option
                        v-for="item in stores.coursehour.sources
                          .courseSubjectSources.data"
                        :value="item.code"
                        :key="item.code"
                        :label="item.courseName"
                      ></Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.coursehour.query.classGradeGuid"
                      clearable
                      filterable
                      remote
                      :remote-method="handleLoadClassGradeDataSource"
                      :loading="
                        stores.coursehour.sources.classGradeSources.loading
                      "
                      placeholder="选择班级..."
                      @on-change="handleSearchCourseHour()"
                    >
                      <Option
                        v-for="(item, index) in stores.coursehour.sources
                          .classGradeSources.data"
                        :value="item.guid"
                        :key="index"
                        >{{ item.className }}</Option
                      >
                    </Select>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'coursehour_create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增课时"
                  >新增课时</Button
                >
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
          :data="stores.coursehour.data"
          :columns="stores.coursehour.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'coursehour_edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
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
      <Form
        :model="formModel.fields"
        ref="formCourseHour"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="学员选择" prop="traineesGuid">
              <el-select
                v-model="formModel.fields.traineesGuid"
                v-bind:disabled="formModel.editModeInputIsDisabled"
                clearable
                filterable
                remote
                :remote-method="handleLoadTraineesDataSource"
                :loading="
                  stores.coursehour.sources.formCourseHourSelectSources
                    .traineesSources.loading
                "
                placeholder="选择学员..."
                no-data-text="无匹配数据"
                style="display: block"
              >
                <el-option
                  v-for="item in stores.coursehour.sources
                    .formCourseHourSelectSources.traineesSources.data"
                  :value="item.guid"
                  :key="item.guid"
                  :label="item.fullName"
                  ><span style="float: left">{{ item.fullName }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{
                    item.telephone
                  }}</span></el-option
                >
              </el-select>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="课程科目" prop="courseCode">
              <el-select
                v-model="formModel.fields.courseCode"
                v-bind:disabled="formModel.editModeInputIsDisabled"
                clearable
                filterable
                remote
                :remote-method="handleLoadformCourseHourCourseSubjectDataSource"
                :loading="
                  stores.coursehour.sources.formCourseHourSelectSources
                    .courseSubjectSources.loading
                "
                placeholder="选择课程科目..."
                @change="handleLoadformCourseHourClassGradeDataSource"
                @clear="handleSelectCourseCodeOnClear"
                no-data-text="无匹配数据"
                style="display: block"
              >
                <el-option
                  v-for="item in stores.coursehour.sources
                    .formCourseHourSelectSources.courseSubjectSources.data"
                  :value="item.code"
                  :key="item.code"
                  :label="item.courseName"
                >
                  <span style="float: left">{{ item.courseName }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{
                    item.code
                  }}</span>
                </el-option>
              </el-select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="班级分配" prop="classGradeGuid">
              <Select
                v-model="formModel.fields.classGradeGuid"
                clearable
                filterable
                not-found-text="请先选定课程科目"
                placeholder="请选择班级..."
              >
                <Option
                  v-for="(item, index) in stores.coursehour.sources
                    .formCourseHourSelectSources.classGradeSources"
                  :value="item.guid"
                  :key="index"
                  >{{ item.className }}</Option
                >
              </Select>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="截至日期" prop="expiryDate">
              <DatePicker
                v-model="formModel.fields.expiryDate"
                placeholder="请输入截至日期"
                type="date"
                :options="expiryDateDatePickerOptions"
                style="width: 100%"
                :editable="false"
                :clearable="false"
                v-bind:disabled="formModel.fields.isMaxExpiryDate"
              ></DatePicker>
            </FormItem>
          </Col>
          <Col span="4">
            <FormItem label=" ">
              <Checkbox v-model="formModel.fields.isMaxExpiryDate"
                >无期限</Checkbox
              >
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="学员剩余课时数" prop="price">
              <Input
                type="number"
                min="0"
                v-model="formModel.fields.surplus"
              ></Input>
            </FormItem>
          </Col>
        </Row>
        <FormItem label="备注" label-position="top">
          <Input
            type="textarea"
            v-model="formModel.fields.memo"
            :rows="4"
            placeholder="备注信息"
          />
        </FormItem>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitCourseHour"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
//import Tables from "_c/tables";
import {
  getCourseHourList,
  createCourseHour,
  loadCourseHour,
  editCourseHour,
} from "@/api/教务中心/coursehour";
import { findCourseSubjectDataSourceByKeyword } from "@/api/教务中心/coursesubject";
import {
  findClassGradeDataSourceByKeyword,
  findClassGradeDataSourceByCourseCode,
} from "@/api/教务中心/classgrade";
import { findTraineesDataSourceByKeyword } from "@/api/教务中心/trainees";

export default {
  name: "coursehour_page",
  components: {
    DzTable,
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "创建学员课时",
        mode: "create",
        selection: [],
        editModeInputIsDisabled: false, //控制课程代码输入框是否可以编辑
        inputPrint: 0,
        isMaxExpiryDate: true,
        fields: {
          guid: "00000000-0000-0000-0000-000000000000",
          surplus: 0,
          expiryDate: new Date().toLocaleDateString(),//dateFormat("yyyy-MM-dd",new Date()),
          traineesGuid: "",
          courseCode: "",
          classGradeGuid: "",
          memo: "",
        },
        rules: {
          traineesGuid: [
            {
              type: "string",
              required: true,
              message: "请选择学员信息",
              min: 1,
            },
          ],
          courseCode: [
            {
              type: "string",
              required: true,
              message: "请选择课程科目",
              min: 1,
            },
          ],
        },
      },
      stores: {
        coursehour: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            courseCode: "",
            classGradeGuid: "",
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn",
              },
            ],
          },
          sources: {
            classGradeSources: {
              loading: false,
              data: [],
            },
            courseSubjectSources: {
              loading: false,
              data: [],
            },
            formCourseHourSelectSources: {
              courseSubjectSources: {
                loading: false,
                data: [],
              },
              classGradeSources: [],
              traineesSources: {
                loading: false,
                data: [],
              },
            },
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "课程名称", key: "courseName", width: 220 },
            { title: "剩余课时数", key: "surplus", width: 95, sortable: true },
            { title: "学员姓名", key: "fullName", width: 95, sortable: true },
            { title: "联系电话", key: "telephone", width: 95, sortable: true },
            { title: "班级名称", width: 220, key: "className" },
            {
              title: "截至日期",
              width: 150,
              ellipsis: true,
              tooltip: true,
              key: "expiryDate",
              sortable: true,
            },
            {
              title: "修改时间",
              width: 150,
              ellipsis: true,
              tooltip: true,
              key: "modifiedOn",
              sortable: true,
            },
            {
              title: "创建时间",
              width: 150,
              ellipsis: true,
              tooltip: true,
              key: "createdOn",
              sortable: true,
            },
            {
              title: "操作日志",
              width: 250,
              ellipsis: true,
              tooltip: true,
              key: "operationLog",
            },
            {
              title: "操作",
              fixed: "right",
              align: "center",
              width: 80,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      expiryDateDatePickerOptions: {
        shortcuts: [
          {
            text: "今天",
            value() {
              return new Date();
            },
          },
          {
            text: "7天后",
            value() {
              const date = new Date();
              date.setTime(date.getTime() + 3600 * 1000 * 24 * 7);
              return date;
            },
          },
          {
            text: "30天后",
            value() {
              const date = new Date();
              date.setTime(date.getTime() + 3600 * 1000 * 24 * 30);
              return date;
            },
          },
          {
            text: "365天后",
            value() {
              const date = new Date();
              date.setTime(date.getTime() + 3600 * 1000 * 24 * 365);
              return date;
            },
          },
        ],
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static",
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建学员课时";
      }
      if (this.formModel.mode === "edit") {
        return "编辑学员课时";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.guid);
    },
  },
  methods: {
    loadCourseHourList() {
      getCourseHourList(this.stores.coursehour.query).then((res) => {
        this.stores.coursehour.data = res.data.data;
        this.stores.coursehour.query.totalCount = res.data.totalCount;
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
      //console.log(row.guid)
      this.handleSwitchFormModeToEdit();
      this.handleResetFormCourseHour();
      this.doLoadCourseHour(row.guid);
      //this.handleLoadformCourseHourClassGradeDataSource();
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      //this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadCourseHourList();
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormCourseHour();
    },
    async handleSubmitCourseHour() {
      let valid = await this.validateCourseHourForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateCourseHour();
        }
        if (this.formModel.mode === "edit") {
          this.doEditCourseHour();
        }
      }
    },
    handleResetFormCourseHour() {
      this.$refs["formCourseHour"].resetFields();
      //this.formModel.inputPrint = 0;
    },
    doCreateCourseHour() {
      //this.formModel.fields.guid = null;
      createCourseHour(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseHourList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditCourseHour() {
      editCourseHour(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseHourList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    async validateCourseHourForm() {
      let _valid = false;
      await this.$refs["formCourseHour"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadCourseHour(guid) {
      loadCourseHour({ guid: guid }).then((res) => {
        this.formModel.fields = res.data.data;
        this.handleLoadformCourseHourClassGradeDataSource();
      });
    },
    handleSearchCourseHour() {
      this.loadCourseHourList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.coursehour.query.sort.direction = column.order;
      this.stores.coursehour.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.coursehour.query.currentPage = page;
      this.loadCourseHourList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.coursehour.query.pageSize = pageSize;
      this.loadCourseHourList();
    },
    handleLoadCourseSubjectDataSource(keyword) {
      this.stores.coursehour.sources.courseSubjectSources.loading = true;
      let query = { keyword: keyword };
      findCourseSubjectDataSourceByKeyword(query).then((res) => {
        this.stores.coursehour.sources.courseSubjectSources.data =
          res.data.data;
        this.stores.coursehour.sources.courseSubjectSources.loading = false;
      });
    },
    handleLoadClassGradeDataSource(keyword) {
      this.stores.coursehour.sources.classGradeSources.loading = true;
      let query = { keyword: keyword };
      findClassGradeDataSourceByKeyword(query).then((res) => {
        this.stores.coursehour.sources.classGradeSources.data = res.data.data;
        this.stores.coursehour.sources.classGradeSources.loading = false;
      });
    },
    handleLoadformCourseHourCourseSubjectDataSource(keyword) {
      if (keyword !== "") {
        this.stores.coursehour.sources.courseSubjectSources.loading = true;
        let query = { keyword: keyword };
        findCourseSubjectDataSourceByKeyword(query).then((res) => {
          this.stores.coursehour.sources.formCourseHourSelectSources.courseSubjectSources.data =
            res.data.data;
          this.stores.coursehour.sources.formCourseHourSelectSources.courseSubjectSources.loading = false;
        });
      } else {
        this.stores.courseschedule.sources.teacherSources.data = [];
      }
    },
    handleLoadformCourseHourClassGradeDataSource() {
      console.log(this.formModel.fields.courseCode);
      if (this.formModel.fields.courseCode !== undefined && this.formModel.fields.courseCode !== "") {
        findClassGradeDataSourceByCourseCode(
          this.formModel.fields.courseCode
        ).then((res) => {
          this.stores.coursehour.sources.formCourseHourSelectSources.classGradeSources =
            res.data.data;
        });
      } else {
        this.stores.coursehour.sources.formCourseHourSelectSources.classGradeSources = [];
      }
    },
    handleSelectCourseCodeOnClear() {
      this.formModel.fields.classGradeGuid = "";
    },
    handleLoadTraineesDataSource(keyword) {
      if (keyword !== "") {
        this.stores.coursehour.sources.formCourseHourSelectSources.traineesSources.loading = true;
        let query = { kw: keyword, cp: 1, ps: 10 };
        findTraineesDataSourceByKeyword(query).then((res) => {
          this.stores.coursehour.sources.formCourseHourSelectSources.traineesSources.data =
            res.data.data;
          this.stores.coursehour.sources.formCourseHourSelectSources.traineesSources.loading = false;
        });
      } else {
        this.stores.courseschedule.sources.traineesSources.data = [];
      }
    },
  },
  mounted() {
    this.loadCourseHourList();
  },
};
</script>

<style>
</style>
