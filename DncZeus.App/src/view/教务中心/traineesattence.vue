<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.traineesattence.query.totalCount"
        :pageSize="stores.traineesattence.query.pageSize"
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
                      v-model="stores.traineesattence.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchTraineesAttence()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.traineesattence.query.isAttend"
                        @on-change="handleSearchTraineesAttence"
                        placeholder="考勤状态"
                        style="width:80px;"
                      >
                        <Option
                          v-for="item in stores.traineesattence.sources.isAttendSources"
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
                      placeholder="请输入考勤日期"
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
          :data="stores.traineesattence.data"
          :columns="stores.traineesattence.columns"
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
              title="确定要扣除吗?"
              @on-ok="handleDeductCourseHour(row)"
              >
              <Tooltip placement="top" content="扣除课时" :delay="1000" :transfer="true">
              <Button v-can="'deduct_coursehour'" type="primary" size="small" shape="circle" icon="md-create"></Button>
            </Tooltip>
            </Poptip>
          </template>
        </Table>
      </dz-table>
    </Card>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getTraineesAttenceList,
  deductCourseHour,
} from "@/api/教务中心/traineesattence";
export default {
  name: "traineesattence_page",
  components: {
    DzTable,
  },
  data() {
    return {
      stores: {
        traineesattence: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            isAttend: -1,
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
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "签到时间", key: "attenceTime", width: 150, sortable: true },
            { title: "学员姓名", key: "traineesName", width: 120 },
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
                    tooltipText = "否";
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
            { title: "创建时间", key: "createdOn", width: 150, sortable: true },
            { title: "最近修改时间", width:150, key: "modifiedOn", ellipsis: true, tooltip: true, sortable: true },
            { title: "修改操作人", key: "modifiedByUserName", width: 100 },
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
    loadTraineesAttenceList() {
      getTraineesAttenceList(this.stores.traineesattence.query).then((res) => {
        this.stores.traineesattence.data = res.data.data;
        this.stores.traineesattence.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      //this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadTraineesAttenceList();
    },
    doLoadTraineesAttence(guid) {
      loadTraineesAttence({ guid: guid }).then((res) => {
        this.formModel.fields = res.data.data;
      });
    },
    handleSearchTraineesAttence() {
      this.loadTraineesAttenceList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.traineesattence.query.sort.direction = column.order;
      this.stores.traineesattence.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.traineesattence.query.currentPage = page;
      this.loadTraineesAttenceList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.traineesattence.query.pageSize = pageSize;
      this.loadTraineesAttenceList();
    },
    handleDatePickerOnChange(date){
      this.stores.traineesattence.query.startTime = date[0];
      this.stores.traineesattence.query.endTime = date[1];
      this.loadTraineesAttenceList();
    },
    handleDeductCourseHour(row){
      deductCourseHour(row.guid).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadTraineesAttenceList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
  },
  mounted() {
    this.loadTraineesAttenceList();
  },
};
</script>
<style>
</style>
