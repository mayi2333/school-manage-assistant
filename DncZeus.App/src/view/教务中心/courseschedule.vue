<template>
  <div>
    <Card>
      <div class="dnc-table-wrap">
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
                      v-model="stores.courseschedule.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchCourseSchedule()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.courseschedule.query.isDeleted"
                        @on-change="handleSearchCourseSchedule"
                        placeholder="删除状态"
                        style="width: 80px"
                      >
                        <Option
                          v-for="item in stores.courseschedule.sources
                            .isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.courseschedule.query.isEnabled"
                        @on-change="handleSearchCourseSchedule"
                        placeholder="开启状态"
                        style="width: 80px"
                      >
                        <Option
                          v-for="item in stores.courseschedule.sources
                            .isEnabledSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                      <Select
                        slot="prepend"
                        v-model="stores.courseschedule.query.dayOfWeek"
                        @on-change="handleSearchCourseSchedule"
                        placeholder="星期数"
                        style="width: 80px"
                      >
                        <Option
                          v-for="item in stores.courseschedule.sources
                            .dayOfWeekSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                    </Input>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.courseschedule.query.teacherGuid"
                      clearable
                      filterable
                      remote
                      :remote-method="handleLoadTeacherDataSource"
                      :loading="
                        stores.courseschedule.sources.teacherSources.loading
                      "
                      placeholder="请选择教师..."
                      @on-change="handleSearchCourseSchedule"
                    >
                      <Option
                        v-for="(item, index) in stores.courseschedule.sources
                          .teacherSources.data"
                        :value="item.guid"
                        :key="index"
                        >{{
                          item.fullName + " " + item.telephone
                        }}
                      </Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.courseschedule.query.courseCode"
                      clearable
                      filterable
                      remote
                      :remote-method="handleLoadCourseSubjectDataSource"
                      :loading="
                        stores.courseschedule.sources.courseSubjectSources
                          .loading
                      "
                      placeholder="请选择课程科目..."
                      @on-change="handleSearchCourseSchedule"
                    >
                      <Option
                        v-for="(item, index) in stores.courseschedule.sources
                          .courseSubjectSources.data"
                        :value="item.code"
                        :key="index"
                        >{{ item.courseName + " " + item.code }}</Option
                      >
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Select
                      v-model="stores.courseschedule.query.classGradeGuid"
                      clearable
                      filterable
                      remote
                      :remote-method="handleLoadClassGradeDataSource"
                      :loading="
                        stores.courseschedule.sources.classGradeSources.loading
                      "
                      placeholder="请选择班级..."
                      @on-change="handleSearchCourseSchedule"
                    >
                      <Option
                        v-for="(item, index) in stores.courseschedule.sources
                          .classGradeSources.data"
                        :value="item.guid"
                        :key="index"
                        >{{ item.className }}</Option
                      >
                    </Select>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      placeholder="请选择上课日期"
                      type="daterange"
                      :options="formDatePickerOptions"
                      style="width: 100%"
                      :editable="false"
                      :clearable="true"
                      @on-change="handleQueryDatePickerOnChange"
                    ></DatePicker>
                  </FormItem>
                  <FormItem>
                    <TimePicker
                      format="HH:mm"
                      placeholder="请选择上课时间"
                      type="timerange"
                      style="width: 100%"
                      :editable="false"
                      :clearable="true"
                      @on-change="handleQueryTimePickerOnChange"
                    ></TimePicker>
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
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'courseschedule_create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增排课"
                  >新增排课</Button
                >
              </Col>
            </Row>
          </section>
        </div>
        <Tabs
          v-model="stores.courseschedule.mode"
          @on-click="handleTabsOnClick"
        >
          <TabPane label="网格视图" icon="ios-grid" name="grid-view">
            <Card>
              <FullCalendar ref="fullCalendar" :options="calendarOptions" />
              <Slider 
                v-model="sliderOptions.value"
                :step="10"
                show-tip="never"
                @on-change="handleSliderOnChange"
                :marks="sliderOptions.marks"></Slider>
            </Card>
          </TabPane>
          <TabPane label="列表视图" icon="md-list-box" name="list-view">
            <Card>
              <div>
                <Table
                  slot="table"
                  ref="tables"
                  :border="false"
                  size="small"
                  :highlight-row="true"
                  :data="stores.courseschedule.data"
                  :columns="stores.courseschedule.columns"
                  @on-select="handleSelect"
                  @on-selection-change="handleSelectionChange"
                  @on-refresh="handleRefresh"
                  :row-class-name="rowClsRender"
                  @on-page-change="handlePageChanged"
                  @on-page-size-change="handlePageSizeChanged"
                  @on-sort-change="handleSortChange"
                >
                  <template slot-scope="{ row, index }" slot="action">
                    <Poptip
                      confirm
                      :transfer="true"
                      title="确定要删除吗?"
                      @on-ok="handleDelete(row)"
                    >
                      <Tooltip
                        placement="top"
                        content="删除"
                        :delay="1000"
                        :transfer="true"
                      >
                        <Button
                          type="error"
                          size="small"
                          shape="circle"
                          icon="md-trash"
                        ></Button>
                      </Tooltip>
                    </Poptip>
                    <Tooltip
                      placement="top"
                      content="编辑"
                      :delay="1000"
                      :transfer="true"
                    >
                      <Button
                        v-can="'courseschedule_edit'"
                        type="primary"
                        size="small"
                        shape="circle"
                        icon="md-create"
                        @click="handleEdit(row)"
                      ></Button>
                    </Tooltip>
                    <Tooltip
                      placement="top"
                      content="分配课表"
                      :delay="1000"
                      :transfer="true"
                    >
                      <Button
                        v-can="'courseschedule_assign'"
                        type="success"
                        size="small"
                        shape="circle"
                        icon="md-attach"
                        @click="handleAssignCourseSchedule(row)"
                      ></Button>
                    </Tooltip>
                  </template>
                </Table>
                <div style="margin-top: 15px">
                  <Page
                    :total="stores.courseschedule.query.totalCount"
                    :page-size="stores.courseschedule.query.pageSize"
                    size="small"
                    show-elevator
                    show-sizer
                    show-total
                    :page-size-opts="pageSizeOpts"
                    @on-change="handlePageChanged"
                    @on-page-size-change="handlePageSizeChanged"
                  ></Page>
                </div>
              </div>
            </Card>
          </TabPane>
        </Tabs>
      </div>
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
        ref="formCourseSchedule"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="上课日期">
              <DatePicker
                v-model="formModel.formDatePickerValue"
                placeholder="请输入上课日期"
                type="daterange"
                :options="formDatePickerOptions"
                style="width: 100%"
                :editable="false"
                :clearable="false"
                @on-change="handleFormDatePickerOnChange"
              ></DatePicker>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="上课时间">
              <TimePicker
                v-model="formModel.formTimePickerValue"
                format="HH:mm"
                placeholder="请输入上课时间"
                type="timerange"
                style="width: 100%"
                :editable="false"
                :clearable="false"
                @on-change="handleFormTimePickerOnChange"
              ></TimePicker>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <RadioGroup v-model="formModel.fields.dayOfWeek" type="button">
              <Radio :label="1">星期一</Radio>
              <Radio :label="2">星期二</Radio>
              <Radio :label="3">星期三</Radio>
              <Radio :label="4">星期四</Radio>
              <Radio :label="5">星期五</Radio>
              <Radio :label="6">星期六</Radio>
              <Radio :label="0">星期日</Radio>
              <Radio :label="7">特约课</Radio>
            </RadioGroup>
          </Col>
          <Col span="6">
            <FormItem label="按年循环">
              <i-switch
                size="large"
                v-model="formModel.fields.loopOfYear"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
          <Col span="6">
            <FormItem label="是否开启">
              <i-switch
                size="large"
                v-model="formModel.fields.isEnabled"
                :true-value="1"
                :false-value="0"
              >
                <span slot="open">开启</span>
                <span slot="close">关闭</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="授课教师" prop="teacherGuid">
              <el-select
                v-model="formModel.fields.teacherGuid"
                clearable
                filterable
                remote
                placeholder="请输入关键字搜索教师..."
                :remote-method="handleLoadTeacherDataSource"
                :loading="stores.courseschedule.sources.teacherSources.loading"
                no-data-text="无匹配数据"
                style="display: block"
              >
                <el-option
                  v-for="item in stores.courseschedule.sources.teacherSources
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
          </Col>
          <Col span="12">
            <FormItem label="课程科目" prop="courseCode">
              <el-select
                v-model="formModel.fields.courseCode"
                clearable
                filterable
                remote
                :remote-method="handleLoadCourseSubjectDataSource"
                :loading="
                  stores.courseschedule.sources.courseSubjectSources.loading
                "
                no-data-text="无匹配数据"
                placeholder="请输入关键字搜索课程科目..."
                v-bind:disabled="formModel.editModeInputIsDisabled"
                @change="handleLoadformClassGradeDataSource"
                @clear="handleSelectCourseCodeOnClear"
                style="display: block"
              >
                <el-option
                  v-for="item in stores.courseschedule.sources
                    .courseSubjectSources.data"
                  :value="item.code"
                  :key="item.code"
                  :label="item.courseName"
                >
                  <span style="float: left">{{ item.courseName }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{ item.code }}</span>
                </el-option>
              </el-select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="12">
            <FormItem label="教室名称" prop="classRoomName">
              <Input v-model="formModel.fields.classRoomName" placeholder="请输入班级名称"/>
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="背景颜色" label-position="top">
              <ColorPicker v-model="formModel.fields.backColor" placeholder="课表背景色"/>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <FormItem label="班级分配" prop="classGradeGuids">
              <Select
                v-model="formModel.fields.classGradeGuids"
                filterable
                not-found-text="请先选定课程科目"
                placeholder="请选择班级..."
                multiple
                v-bind:disabled="formModel.editModeInputIsDisabled"
              >
                <Option
                  v-for="(item, index) in stores.courseschedule.sources
                    .formSelectSources.classGradeSources"
                  :value="item.guid"
                  :key="index"
                  >{{ item.className }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="24">
            <FormItem label="学员分配" prop="courseHourGuids">
              <Select
                v-model="formModel.fields.courseHourGuids"
                filterable
                not-found-text="请先选定课程科目"
                placeholder="请选择学员..."
                multiple
                v-bind:disabled="formModel.editModeInputIsDisabled"
              >
                <Option
                  v-for="(item, index) in stores.courseschedule.sources
                    .formSelectSources.courseHourGuids"
                  :value="item.guid"
                  :key="index"
                  >{{ item.fullName }}</Option
                >
              </Select>
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
          @click="handleSubmitCourseSchedule"
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
    <Drawer
      title="课表分配"
      v-model="formAssignCoursesChedule.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <Form>
        <FormItem>
          <Transfer
            :data="formAssignCoursesChedule.classGrades"
            :target-keys="formAssignCoursesChedule.ownedClassGrades"
            :render-format="renderOwnedClassGrades"
            :titles="['未分配的班级', '已分配的班级']"
            @on-change="handleOwnedClassGradesChanged"
          ></Transfer>
        </FormItem>
        <FormItem>
          <Transfer
            :data="formAssignCoursesChedule.courseHours"
            :target-keys="formAssignCoursesChedule.ownedCourseHours"
            :render-format="renderOwnedCourseHours"
            :titles="['未分配的学员', '已分配的学员']"
            @on-change="handleOwnedCourseHoursChanged"
          ></Transfer>
        </FormItem>
      </Form>
      <div class="demo-drawer-footer" style="margin-top: 15px">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSaveAssignCoursesChedule"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formAssignCoursesChedule.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <el-dialog
      :title="dialogModel.title"
      :visible.sync="dialogModel.opened"
      width="30%"
      :before-close="handleDialogClose"
    >
      <div>
        <div class="text item">
          {{ "课程名称: " + dialogModel.courseName }}
        </div>
        <div class="text item">
          {{
            "上课时间: " + dialogModel.startTime + " — " + dialogModel.endTime
          }}
        </div>
        <div class="text item">
          {{ "上课教师: " + dialogModel.teacherName }}
        </div>
        <div class="text item">
          {{ "上课教室: " + dialogModel.classRoomName }}
        </div>
        <div class="text item">
          {{ "上课班级: " + dialogModel.className }}
        </div>
        <div class="text item">
          {{ "上课学员: " + dialogModel.traineesName }}
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button v-can="'courseschedule_assign'" type="primary" @click="handleDialogAssignButtonOnclick"
          >课表分配</el-button
        >
        <el-button v-can="'courseschedule_edit'" type="primary" @click="handleDialogEditButtonOnclick"
          >课表编辑</el-button
        >
        <el-button @click="handleDialogClose">关 闭</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import FullCalendar from "@fullcalendar/vue";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import {
  getCourseScheduleList,
  createCourseSchedule,
  loadCourseSchedule,
  editCourseSchedule,
  saveAssignCoursesChedule,
  getCourseScheduleGrid,
  getCourseDetail,
} from "@/api/教务中心/courseschedule";
import { findCourseSubjectDataSourceByKeyword } from "@/api/教务中心/coursesubject";
import { findTeacherDataSourceByKeyword } from "@/api/教务中心/teacher";
import {
  findClassGradeDataSourceByCourseCodeAndIsSpecial,
  loadClassGradeListByCoursesCheduleGuid,
  findClassGradeDataSourceByKeyword,
} from "@/api/教务中心/classgrade";
import {
  isSpecialOfFindCourseHourDataSourceByCourseCode,
  loadCourseHourListByCoursesCheduleGuid,
} from "@/api/教务中心/coursehour";
import { dateFormat } from "@/libs/tools";

export default {
  name: "courseschedule_page",
  components: {
    FullCalendar, // 使 <FullCalendar> 标记可用
  },
  data() {
    return {
      formModel: {
        opened: false,
        title: "新增排课",
        mode: "create",
        selection: [],
        editModeInputIsDisabled: false, //控制课程代码输入框是否可以编辑
        formDatePickerValue: [new Date(), new Date()],
        formTimePickerValue: ["0:00", "0:00"],
        fields: {
          guid: "00000000-0000-0000-0000-000000000000",
          startDate: new Date().toLocaleDateString(),
          endDate: new Date().toLocaleDateString(),
          loopOfYear: 0,
          startTime: "0:00",
          endTime: "0:00",
          dayOfWeek: 7,
          isEnabled: 1,
          teacherGuid: "",
          courseCode: "",
          classRoomName:"",
          backColor:"",
          classGradeGuids: [],
          courseHourGuids: [],
          memo: "",
        },
        rules: {
          teacherGuid: [
            { type: "string", required: true, message: "请选择教师信息" },
          ],
          courseCode: [
            { type: "string", required: true, message: "请选择课程科目" },
          ],
        },
      },
      formAssignCoursesChedule: {
        courseScheduleGuid: "",
        opened: false,
        ownedClassGrades: [],
        ownedCourseHours: [],
        inited: false,
        classGrades: [],
        courseHours: [],
      },
      dialogModel: {
        opened: false,
        title: "",
        startTime: "",
        endTime: "",
        courseName: "",
        courseCode: "",
        teacherName: "",
        classRoomName:"",
        className: "",
        traineesName: "",
        courseScheduleGuid: "",
      },
      stores: {
        courseschedule: {
          mode: "grid-view",
          query: {
            totalCount: 0,
            pageSize: 10,
            currentPage: 1,
            isDeleted: 0,
            isEnabled: 1,
            dayOfWeek: -1,
            courseCode: "",
            teacherGuid: "",
            classGradeGuid: "",
            startTime: "",
            endTime: "",
            startDate: "",
            endDate: "",
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn",
              },
            ],
          },
          sources: {
            dayOfWeekSources: [
              { value: -1, text: "全部" },
              { value: 1, text: "星期一" },
              { value: 2, text: "星期二" },
              { value: 3, text: "星期三" },
              { value: 4, text: "星期四" },
              { value: 5, text: "星期五" },
              { value: 6, text: "星期六" },
              { value: 0, text: "星期日" },
              { value: 7, text: "特约课" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
            ],
            isEnabledSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "关闭" },
              { value: 1, text: "开启" },
            ],
            courseSubjectSources: {
              loading: false,
              data: [],
            },
            teacherSources: {
              loading: false,
              data: [],
            },
            classGradeSources: {
              loading: false,
              data: [],
            },
            formSelectSources: {
              classGradeSources: [],
              courseHourGuids: [],
            },
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "上课时间", key: "displayTime", width: 330 },
            {
              title: "课程名称",
              key: "courseName",
              width: 150,
              ellipsis: true,
              tooltip: true,
            },
            { title: "上课教师", key: "teacherName", width: 85 },
            { title: "教室名称", key: "classRoomName", width: 100, ellipsis: true, tooltip: true },
            {
              title: "是否开启",
              key: "isEnabled",
              width: 65,
              render: (h, params) => {
                let isEnabled = params.row.isEnabled;
                let statusColor = "success";
                let statusText = "未知";
                let tooltipText = "";
                switch (isEnabled) {
                  case 0:
                    statusText = "关闭";
                    statusColor = "default";
                    tooltipText = "关闭";
                    break;
                  case 1:
                    statusText = "开启";
                    statusColor = "primary";
                    tooltipText = "开启";
                    break;
                }
                return h(
                  "Tooltip",
                  {
                    props: {
                      placement: "top",
                      transfer: true,
                      delay: 500,
                    },
                  },
                  [
                    //这个中括号表示是Tooltip标签的子标签
                    h(
                      "Tag",
                      {
                        props: {
                          //type: "dot",
                          color: statusColor,
                        },
                      },
                      statusText
                    ), //表格列显示文字
                    h(
                      "p",
                      {
                        slot: "content",
                        style: {
                          whiteSpace: "normal",
                        },
                      },
                      tooltipText //整个的信息即气泡内文字
                    ),
                  ]
                );
              },
            },
            { title: "创建时间", key: "createdOn", width: 120, sortable: true },
            {
              title: "备注",
              key: "memo",
              width: 90,
              ellipsis: true,
              tooltip: true,
            },
            { title: " " },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
          gridData: {
            gridItem: [],
            totalCount: 0,
            weekDay: 0,
            initialDate: new Date(),
          },
        },
      },
      pageSizeOpts: [5, 10, 20, 30, 40, 50],
      formDatePickerOptions: {
        shortcuts: [
          {
            text: "今天",
            value() {
              const end = new Date();
              const start = new Date();
              return [start, end];
            },
          },
        ],
      },
      calendarOptions: {
        // 引入的插件，比如fullcalendar/daygrid，fullcalendar/timegrid引入后才可显示月，周，日
        plugins: [dayGridPlugin, interactionPlugin, timeGridPlugin],
        initialView: "timeGridDay", // 默认为那个视图（月：dayGridMonth，周：timeGridWeek，日：timeGridDay）
        firstDay: 0, //this.stores.courseschedule.gridData.weekDay, // 设置一周中显示的第一天是哪天，周日是0，周一是1，类推
        locale: "zh-cn", // 切换语言，当前为中文
        //eventColor: '#3BB2E3', // 全部日历日程背景色
        //themeSystem: 'standard', // 主题色(本地测试未能生效)
        //defaultDate: new Date(), //初始化日期时间
        //timeGridEventMinHeight: '20', // 设置事件的最小高度
        aspectRatio: 3, // 设置日历单元格宽度与高度的比例。
        slotDuration:"00:30:00",//时间间隔
        // displayEventTime: false, // 是否显示时间
        allDaySlot: false, // 周，日视图时，all-day 不显示
        //eventLimit: true, // 设置月日程，与all-day slot的最大显示数量，超过的通过弹窗显示
        headerToolbar: {
          // 日历头部按钮位置
          left: "prev today next",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay",
        },
        buttonText: {
          today: "今天",
          month: "月",
          week: "周",
          day: "日",
        },
        //slotMinutes:5, //找不到配置
        slotLabelFormat: {
          hour: "2-digit",
          minute: "2-digit",
          meridiem: false,
          hour12: false, // 设置时间为24小时
        },
        events: [],
        editable: false, // 是否可以进行（拖动、缩放）修改
        eventStartEditable: false, // Event日程开始时间可以改变，默认true，如果是false其实就是指日程块不能随意拖动，只能上下拉伸改变他的endTime
        eventDurationEditable: false, // Event日程的开始结束时间距离是否可以改变，默认true，如果是false则表示开始结束时间范围不能拉伸，只能拖拽
        selectable: false, // 是否可以选中日历格
        weekends: true,
        navLinks: true, // 天链接
        slotEventOverlap: true, // 相同时间段的多个日程视觉上是否允许重叠，默认true允许
        //eventLimitNum: { // 事件显示数量限制(本地测试未能生效)
        //  dayGrid: {
        //    eventLimit: 5
        //  },
        //  timeGrid: {
        //    eventLimit: 2 // adjust to 6 only for timeGridWeek/timeGridDay
        //  }
        //},
        // 事件
        eventClick: this.handleCalendarEventClick, // 点击日历日程事件
        //eventMouseEnter: this.handleCalendarEventMouseEnter,//鼠标悬停事件
        //eventDblClick: this.handleCalendarEventDblClick, // 双击日历日程事件 (这部分是在源码中添加的)
        //eventClickDelete: this.eventClickDelete, // 点击删除标签事件 (这部分是在源码中添加的)
        //eventDrop: this.eventDrop, // 拖动日历日程事件
        //eventResize: this.eventResize, // 修改日历日程大小事件
        //select: this.handleDateSelect, // 选中日历格事件
        //eventDidMount: this.eventDidMount, // 安装提示事件
        // loading: this.loadingEvent, // 视图数据加载中、加载完成触发（用于配合显示/隐藏加载指示器。）
        // selectAllow: this.selectAllow, //编程控制用户可以选择的地方，返回true则表示可选择，false表示不可选择
        //eventMouseEnter: this.eventMouseEnter, // 鼠标滑过
        //allowContextMenu: false,
        //selectMirror: true,
        //selectMinDistance: 0, // 选中日历格的最小距离
        //dayMaxEvents: true,
        //selectHelper: true,
      },
      sliderOptions:{
        value: 0,
        marks: {
                    0: '默认',
                    10: '10分钟',
                    20: '',
                    30: '半小时',
                    40: '',
                    50: '',
                    60: '一个小时',
                    70: '',
                    80: '',
                    90: '一个半小时',
                    100: '',
                }
      },
      //currentEvents: [],
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
        return "新增排课";
      }
      if (this.formModel.mode === "edit") {
        return "编辑排课";
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
    loadCourseScheduleListOrGrid() {
      if (this.stores.courseschedule.mode === "list-view") {
        getCourseScheduleList(this.stores.courseschedule.query).then((res) => {
          this.stores.courseschedule.data = res.data.data;
          this.stores.courseschedule.query.totalCount = res.data.totalCount;
        });
      } else if (this.stores.courseschedule.mode === "grid-view") {
        getCourseScheduleGrid(this.stores.courseschedule.query).then((res) => {
          this.stores.courseschedule.gridData.gridItem = res.data.data.gridItem;
          this.stores.courseschedule.gridData.weekDay = res.data.data.weekDay;
          this.stores.courseschedule.gridData.initialDate =
            res.data.data.initialDate;
          this.stores.courseschedule.gridData.totalCount = res.data.totalCount;
          this.handleCalendarEventRefetch();
        });
      } else {
        this.$Message.warning("参数错误");
      }
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadCourseScheduleListOrGrid();
    },
    handleSearchCourseSchedule() {
      this.loadCourseScheduleListOrGrid();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.courseschedule.query.sort.direction = column.order;
      this.stores.courseschedule.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.courseschedule.query.currentPage = page;
      this.loadCourseScheduleListOrGrid();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.courseschedule.query.pageSize = pageSize;
      this.loadCourseScheduleListOrGrid();
    },
    doLoadCourseSchedule(guid) {
      loadCourseSchedule({ guid: guid }).then((res) => {
        this.formModel.fields = res.data.data;
        //this.$refs.formteacherGuidSelect.setQuery(res.data.data.teacherGuid); //res.data.data.teacherGuid
        //window.console.log(this.formModel.fields);
        //this.$refs.formteacherGuidSelect.setQuery(null)
        this.formModel.formTimePickerValue = [
          res.data.data.startTime,
          res.data.data.endTime,
        ];
        this.formModel.formDatePickerValue = [
          res.data.data.startDate,
          res.data.data.endDate,
        ];
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
    async handleEdit(row) {
      this.handleSwitchFormModeToEdit();
      await this.handleResetFormCourseSchedule();
      this.doLoadCourseSchedule(row.guid);
    },
    handleShowCreateWindow() {
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormCourseSchedule();
    },
    async handleSubmitCourseSchedule() {
      let valid = await this.validateCourseScheduleForm();
      //window.console.log("4" + valid);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateCourseSchedule();
        }
        if (this.formModel.mode === "edit") {
          this.doEditCourseSchedule();
        }
      }
    },
    handleResetFormCourseSchedule() {
      //window.console.log(this.$refs["formCourseSchedule"]);
      this.$refs["formCourseSchedule"].resetFields();
    },
    doCreateCourseSchedule() {
      //this.formModel.fields.guid = null;
      createCourseSchedule(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseScheduleListOrGrid();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditCourseSchedule() {
      editCourseSchedule(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadCourseScheduleListOrGrid();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    async validateCourseScheduleForm() {
      //window.console.log(this.$refs["formCourseSchedule"].validate());
      let _valid = false;
      await this.$refs["formCourseSchedule"].validate((valid) => {
        //window.console.log("1" + valid);
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
          //this.$Message.success("表单已完善");
          //window.console.log("2" + _valid);
        }
      });
      //window.console.log("3" + _valid);
      return _valid;
    },
    handleDelete(row) {
      this.doDelete(row.code);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteCourseSchedule(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCourseScheduleListOrGrid();
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
      recoverCourseSchedule(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadCourseScheduleListOrGrid();
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
        content: "<p>确定要执行当前 [" + command + "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    doBatchCommand(command) {
      if (command == "删除") {
        this.doDelete(this.selectedRowsId.join(","));
      } else if (command == "恢复") {
        this.doRecover(this.selectedRowsId.join(","));
      }
      this.$Modal.remove();
    },
    handleFormDatePickerOnChange(date) {
      this.formModel.fields.startDate = date[0];
      this.formModel.fields.endDate = date[1];
    },
    handleQueryDatePickerOnChange(date) {
      this.stores.courseschedule.query.startDate = date[0];
      this.stores.courseschedule.query.endDate = date[1];
      this.handleSearchCourseSchedule();
    },
    handleFormTimePickerOnChange(date) {
      this.formModel.fields.startTime = date[0];
      this.formModel.fields.endTime = date[1];
    },
    handleQueryTimePickerOnChange(date) {
      this.stores.courseschedule.query.startTime = date[0];
      this.stores.courseschedule.query.endTime = date[1];
      this.handleSearchCourseSchedule();
    },
    handleLoadCourseSubjectDataSource(keyword) {
      if (keyword !== "") {
        this.stores.courseschedule.sources.courseSubjectSources.loading = true;
        let query = { keyword: keyword };
        findCourseSubjectDataSourceByKeyword(query).then((res) => {
          this.stores.courseschedule.sources.courseSubjectSources.data =
            res.data.data;
          this.stores.courseschedule.sources.courseSubjectSources.loading = false;
        });
      } else {
        this.stores.courseschedule.sources.teacherSources.data = [];
      }
    },
    handleLoadTeacherDataSource(keyword) {
      if (keyword !== "") {
        this.stores.courseschedule.sources.teacherSources.loading = true;
        let query = { kw: keyword, cp: 1, ps: 10 };
        findTeacherDataSourceByKeyword(query).then((res) => {
          this.stores.courseschedule.sources.teacherSources.data =
            res.data.data;
          this.stores.courseschedule.sources.teacherSources.loading = false;
        });
      } else {
        this.stores.courseschedule.sources.teacherSources.data = [];
      }
    },
    handleLoadformClassGradeDataSource() {
      if (this.formModel.mode === "create") {
        if (
          this.formModel.fields.courseCode !== undefined &&
          this.formModel.fields.courseCode !== ""
        ) {
          let query = { code: this.formModel.fields.courseCode, isspecial: 0 };
          findClassGradeDataSourceByCourseCodeAndIsSpecial(query).then(
            (res) => {
              this.stores.courseschedule.sources.formSelectSources.classGradeSources =
                res.data.data;
            }
          );
          isSpecialOfFindCourseHourDataSourceByCourseCode(
            this.formModel.fields.courseCode
          ).then((res) => {
            this.stores.courseschedule.sources.formSelectSources.courseHourGuids =
              res.data.data;
          });
        } else {
          this.stores.courseschedule.sources.formSelectSources.classGradeSources = [];
          this.stores.courseschedule.sources.formSelectSources.courseHourGuids = [];
        }
      }
    },
    //加载班级查询条件搜索班级
    handleLoadClassGradeDataSource(keyword) {
      this.stores.courseschedule.sources.classGradeSources.loading = true;
      let query = { keyword: keyword };
      findClassGradeDataSourceByKeyword(query).then((res) => {
        this.stores.courseschedule.sources.classGradeSources.data =
          res.data.data;
        this.stores.courseschedule.sources.classGradeSources.loading = false;
      });
    },
    handleSelectCourseCodeOnClear() {
      this.formModel.fields.classGradeGuids = [];
      this.formModel.fields.courseHourGuids = [];
    },
    //分配课程表
    handleAssignCourseSchedule(row) {
      this.formAssignCoursesChedule.opened = true;
      this.formAssignCoursesChedule.courseScheduleGuid = row.guid;
      this.loadAssignCoursesCheduleOfClassGrades(row.guid, row.courseCode);
      this.loadAssignCoursesCheduleOfCourseHours(row.guid, row.courseCode);
    },
    //分配课程表的班级数据列表加载
    loadAssignCoursesCheduleOfClassGrades(guid, code) {
      this.formAssignCoursesChedule.classGrades = [];
      this.formAssignCoursesChedule.ownedClassGrades = [];
      let query = { guid: guid, code: code };
      loadClassGradeListByCoursesCheduleGuid(query).then((res) => {
        var result = res.data.data;
        this.formAssignCoursesChedule.classGrades = result.classGrades;
        this.formAssignCoursesChedule.ownedClassGrades =
          result.assignedClassGrades;
      });
    },
    //分配课程表的学员课时数据列表加载
    loadAssignCoursesCheduleOfCourseHours(guid, code) {
      this.formAssignCoursesChedule.courseHours = [];
      this.formAssignCoursesChedule.ownedCourseHours = [];
      let query = { guid: guid, code: code };
      loadCourseHourListByCoursesCheduleGuid(query).then((res) => {
        var result = res.data.data;
        this.formAssignCoursesChedule.courseHours = result.courseHours;
        this.formAssignCoursesChedule.ownedCourseHours =
          result.assignedCourseHours;
      });
    },
    handleOwnedClassGradesChanged(newTargetKeys, direction, moveKeys) {
      this.formAssignCoursesChedule.ownedClassGrades = newTargetKeys;
    },
    handleOwnedCourseHoursChanged(newTargetKeys, direction, moveKeys) {
      this.formAssignCoursesChedule.ownedCourseHours = newTargetKeys;
    },
    renderOwnedCourseHours(item) {
      return item.label;
    },
    renderOwnedClassGrades(item) {
      return item.label;
    },
    handleSaveAssignCoursesChedule() {
      var data = {
        courseScheduleGuid: this.formAssignCoursesChedule.courseScheduleGuid,
        assignedClassGrades: this.formAssignCoursesChedule.ownedClassGrades,
        assignedCourseHours: this.formAssignCoursesChedule.ownedCourseHours,
      };
      saveAssignCoursesChedule(data).then((res) => {
        this.formAssignCoursesChedule.opened = false;
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleTabsOnClick() {
      this.loadCourseScheduleListOrGrid();
    },
    handleCalendarEventClick(info) {
      //this.$Message.success("单击了日历");
      //window.console.log(info);
      //window.console.log(this.dialogModel.startTime);
      this.dialogModel.title = info.event.title;
      this.dialogModel.startTime = dateFormat(
        "YYYY-mm-dd HH:MM",
        info.event.start
      );
      //window.console.log(this.dialogModel.startTime);
      this.dialogModel.endTime = dateFormat("YYYY-mm-dd HH:MM", info.event.end);
      this.dialogModel.courseCode = info.event.extendedProps.courseCode;
      this.dialogModel.courseScheduleGuid = info.event.extendedProps.guid;
      this.dialogModel.courseName = info.event.title;
      getCourseDetail(info.event.extendedProps.guid).then((res) => {
        if (res.data.code === 200) {
          this.dialogModel.className = res.data.data.className;
          this.dialogModel.traineesName = res.data.data.traineesName;
          this.dialogModel.teacherName = res.data.data.teacherName;
          this.dialogModel.classRoomName = res.data.data.classRoomName;
        } else {
          this.handleDialogClose();
          this.$Message.warning(res.data.message);
        }
      });
      this.dialogModel.opened = true;
    },
    async handleCalendarEventRefetch() {
      this.calendarOptions.firstDay = this.stores.courseschedule.gridData.weekDay;
      //this.calendarOptions.defaultDate= this.stores.courseschedule.gridData.initialDate;
      this.$refs.fullCalendar
        .getApi()
        .gotoDate(this.stores.courseschedule.gridData.initialDate);
      this.calendarOptions.events = this.stores.courseschedule.gridData.gridItem;
      //VueComponent.handleCalendarEventRefetch();
      //this.calendarOptions.weekends= !this.calendarOptions.weekends;
      //this.$refs.fullCalendar.getApi().render();
      //this.$Message.success("刷新了日历");
      //this.calendarOptions.slotDuration = "1:00:00";
      //window.console.log(this.$refs.fullCalendar.getApi());
      //window.console.log(this.calendarOptions);
    },
    handleDialogEditButtonOnclick() {
      //this.$Message.success("编辑课表");
      let params = { guid: this.dialogModel.courseScheduleGuid };
      this.handleDialogClose();
      this.handleEdit(params);
    },
    handleDialogAssignButtonOnclick() {
      //this.$Message.success("分配课表");
      let params = {
        guid: this.dialogModel.courseScheduleGuid,
        courseCode: this.dialogModel.courseCode,
      };
      this.handleDialogClose();
      this.handleAssignCourseSchedule(params);
    },
    async handleDialogClose() {
      this.dialogModel.opened = false;
      this.dialogModel.title = "";
      this.dialogModel.courseName = "";
      this.dialogModel.courseCode = "";
      this.dialogModel.courseScheduleGuid = "";
      this.dialogModel.startTime = "";
      this.dialogModel.endTime = "";
      this.dialogModel.teacherName = "";
      this.dialogModel.className = "";
      this.dialogModel.traineesName = "";
      //window.console.log(this.dialogModel);
    },
    handleSliderOnChange(val){
      //window.console.log(val);
      switch(val){
        case 10:
          this.calendarOptions.slotDuration = "00:10:00";
          break;
        case 20:
          this.calendarOptions.slotDuration = "00:20:00";
          break;
        case 30:
          this.calendarOptions.slotDuration = "00:30:00";
          break;
        case 40:
          this.calendarOptions.slotDuration = "00:40:00";
          break;
        case 50:
          this.calendarOptions.slotDuration = "00:50:00";
          break;
        case 60:
          this.calendarOptions.slotDuration = "1:00:00";
          break;
        case 70:
          this.calendarOptions.slotDuration = "1:10:00";
          break;
        case 80:
          this.calendarOptions.slotDuration = "1:20:00";
          break;
        case 90:
          this.calendarOptions.slotDuration = "1:30:00";
          break;
        default:
          this.calendarOptions.slotDuration = "00:30:00";
      }
    }
  },
  mounted() {
    this.loadCourseScheduleListOrGrid();
  },
};
</script>
<style>
.table-command-column button {
  margin-right: 2px;
}
.text {
  font-size: 14px;
}

.item {
  padding: 5px 0;
}
</style>
