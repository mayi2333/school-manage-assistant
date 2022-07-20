<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.teacherattence.query.totalCount"
        :pageSize="stores.teacherattence.query.pageSize"
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
                      v-model="stores.teacherattence.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchTeacherAttence()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.teacherattence.query.isAttend"
                        @on-change="handleSearchTeacherAttence"
                        placeholder="考勤状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.teacherattence.sources.isAttendSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.teacherattence.query.isSubstitute"
                        @on-change="handleSearchTeacherAttence"
                        placeholder="考勤状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.teacherattence.sources.isSubstituteSources"
                          :value="item.value"
                          :key="item.value"
                        >{{item.text}}</Option>
                      </Select>
                    </Input>
                  </FormItem>
                  <FormItem>
                    <span>创建时间:</span>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      placeholder="请输入上课日期"
                      type="daterange"
                      :options="DatePickerOptions"
                      style="width: 100%"
                      :editable="false"
                      :clearable="false"
                      @on-change="handleDatePickerOnChange"
                    ></DatePicker>
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
          :data="stores.teacherattence.data"
          :columns="stores.teacherattence.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
        <template slot-scope="{row,index}" slot="action">
            <Tooltip placement="top" content="设置代课教师" :delay="1000" :transfer="true">
              <Button v-can="'set_substitute_teacher'" type="primary" size="small" shape="circle" icon="md-create" @click="handleSetSubstituteTeacher(row)"></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      title="设置代课教师"
      v-model="formSetSubstituteTeacher.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <Form
      :model="formSetSubstituteTeacher.fields"
        :rules="formSetSubstituteTeacher.rules"
        label-position="top"
      >
        <FormItem label="授课教师" prop="teacherGuid">
              <el-select
                v-model="formSetSubstituteTeacher.fields.teacherGuid"
                clearable
                filterable
                remote
                placeholder="请输入关键字搜索教师..."
                :remote-method="handleLoadTeacherDataSource"
                :loading="stores.teacherattence.sources.teacherSources.loading"
                no-data-text="无匹配数据"
                style="display: block"
              >
                <el-option
                  v-for="item in stores.teacherattence.sources.teacherSources
                    .data"
                  :key="item.guid"
                  :value="item.guid"
                  :label="item.fullName"
                >
                  <span style="float: left">{{ item.fullName }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{ item.telephone }}</span>
                </el-option>
              </el-select>
            </FormItem>
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 15px">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitSetSubstituteTeacher"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formSetSubstituteTeacher.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getTeacherAttenceList,
  setSubstituteTeacher,
} from "@/api/教务中心/teacherattence";
import { findTeacherDataSourceByKeyword } from "@/api/教务中心/teacher";
export default {
  name: "teacherattence_page",
  components: {
    DzTable,
  },
  data() {
    return {
      formSetSubstituteTeacher: {
        opened: false,
        fields: {
          attenctGuid: "",
          teacherGuid: "",
        },
        rules: {
          teacherGuid: [
            { type: "string", required: true, message: "请选择教师信息" },
          ],
        },
      },
      stores: {
        teacherattence: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isAttend: -1,
            isSubstitute: -1,
            startTime: new Date().toLocaleDateString(),
            endTime: new Date().toLocaleDateString(),
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn",
              },
            ],
          },
          sources: {
            isAttendSources:[
              { value: -1, text: "全部" },
              { value: 0, text: "缺勤" },
              { value: 1, text: "出勤" }
            ],
            isSubstituteSources:[
              { value: -1, text: "全部" },
              { value: 0, text: "不是代课" },
              { value: 1, text: "是代课" }
            ],
            teacherSources: {
              loading: false,
              data: [],
            },
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "签到时间", key: "attenceTime", width: 150, sortable: true },
            { title: "教师姓名", key: "teacherName", width: 120 },
            { title: "科目名称", key: "courseName", width: 150 },
            { title: "出勤", key: "isAttend", width:80,
              render: (h, params) => {
                let isAttend = params.row.isAttend;
                let statusColor = "success";
                let statusText = "未知";
                let tooltipText = "";
                switch (isAttend) {
                  case 0:
                    statusText = "否";
                    statusColor = "default";
                    tooltipText = "代课老师姓名:" + params.row.substituteName;
                    break;
                    case 1:
                    statusText = "是";
                    statusColor = "primary";
                    tooltipText = "是";
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
            { title: "代课", key: "isSubstitute", width: 80,
              render: (h, params) => {
                let isSubstitute = params.row.isSubstitute;
                let statusColor = "success";
                let statusText = "未知";
                let tooltipText = "";
                switch (isSubstitute) {
                  case 0:
                    statusText = "否";
                    statusColor = "default";
                    tooltipText = "否"
                    break;
                    case 1:
                    statusText = "是";
                    statusColor = "primary";
                    tooltipText = "被代课老师姓名:" + params.row.substituteName;
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
            { title: "创建时间", key: "createdOn", width: 150, sortable: true },
            //{ title: "被代课/代课教师", key: "substituteName", width: 80 },
            { title: "最近修改时间", width:150, key: "modifiedOn", ellipsis: true, tooltip: true, sortable: true },
            { title: "修改操作人", key: "modifiedByUserName", width: 80 },
            { title: " ",},
            { title: "操作", align: "center", width: 150, className: "table-command-column", slot: "action" },
          ],
          data: [],
        },
      },
      DatePickerOptions: {
        shortcuts: [
          {
            text: "今天",
            value() {
              const end = new Date();
              const start = new Date();
              return [start, end];
            },
          },
          {
            text: "近一周",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              return [start, end];
            },
          },
          {
            text: "近一个月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              return [start, end];
            },
          },
          {
            text: "近三个月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              return [start, end];
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
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.guid);
    },
  },
  methods: {
    loadTeacherAttenceList() {
      getTeacherAttenceList(this.stores.teacherattence.query).then((res) => {
        this.stores.teacherattence.data = res.data.data;
        this.stores.teacherattence.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      //this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadTeacherAttenceList();
    },
    doLoadTeacherAttence(guid) {
      loadTeacherAttence({ guid: guid }).then((res) => {
        this.formModel.fields = res.data.data;
      });
    },
    handleSearchTeacherAttence() {
      this.loadTeacherAttenceList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.teacherattence.query.sort.direction = column.order;
      this.stores.teacherattence.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.teacherattence.query.currentPage = page;
      this.loadTeacherAttenceList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.teacherattence.query.pageSize = pageSize;
      this.loadTeacherAttenceList();
    },
    handleDatePickerOnChange(date){
      this.stores.teacherattence.query.startTime = date[0];
      this.stores.teacherattence.query.endTime = date[1];
      this.loadTeacherAttenceList();
    },
    handleLoadTeacherDataSource(keyword) {
      if (keyword !== "") {
        this.stores.teacherattence.sources.teacherSources.loading = true;
        let query = { kw: keyword, cp: 1, ps: 10 };
        findTeacherDataSourceByKeyword(query).then((res) => {
          this.stores.teacherattence.sources.teacherSources.data =
            res.data.data;
          this.stores.teacherattence.sources.teacherSources.loading = false;
        });
      } else {
        this.stores.teacherattence.sources.teacherSources.data = [];
      }
    },
    handleSetSubstituteTeacher(row){
      this.formSetSubstituteTeacher.fields.attenctGuid = row.guid;
      this.formSetSubstituteTeacher.opened = true;
    },
    handleSubmitSetSubstituteTeacher(){
      setSubstituteTeacher(this.formSetSubstituteTeacher.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formSetSubstituteTeacher.opened = false;
          this.formSetSubstituteTeacher.fields.attenctGuid = "";
          this.formSetSubstituteTeacher.fields.teacherGuid = "";
          this.loadTeacherAttenceList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
  },
  mounted() {
    this.loadTeacherAttenceList();
  },
};
</script>
<style>
</style>
