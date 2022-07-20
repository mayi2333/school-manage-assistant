<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.operationlog.query.totalCount"
        :pageSize="stores.operationlog.query.pageSize"
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
                      v-model="stores.operationlog.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchOperationLog()"
                    >
                    </Input>
                  </FormItem>
                  <FormItem>
                    <span>操作时间:</span>
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      placeholder="请输入操作日期"
                      type="daterange"
                      :options="OperationTimeDatePickerOptions"
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
          :data="stores.operationlog.data"
          :columns="stores.operationlog.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
        </Table>
      </dz-table>
    </Card>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import { getOperationLogList } from "@/api/系统管理/operationlog";
export default {
  name: "operationlog_page",
  components: {
    DzTable,
  },
  data() {
    return {
      stores: {
        operationlog: {
          //datepickervalue: [new Date(), new Date()],
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            startTime: new Date().toLocaleDateString(),
            endTime: new Date().toLocaleDateString(),
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "OperationTime",
              },
            ],
          },
          sources: {},
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "操作时间",
              key: "operationTime",
              width: 120,
              sortable: true,
            },
            {
              title: "操作人",
              key: "operationByUserName",
              width: 85,
            },
            { title: "模块名称", key: "moudleName", width: 100 },
            { title: "模块方法", key: "methodName", width: 100 },
            {
              title: "控制器名称",
              width: 120,
              key: "controllerName",
            },
            {
              title: "操作名称",
              width: 120,
              key: "actionName",
            },
            { title: "描述", width: 120, ellipsis: true, tooltip: true, key: "descriptor" },
            {
              title: "访问参数",
              ellipsis: true,
              tooltip: true,
              key: "parameter",
            },
            //{ title: "操作", align: "center",width: 150, className: "table-command-column", slot: "action" },
          ],
          data: [],
        },
      },
      OperationTimeDatePickerOptions: {
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
    loadOperationLogList() {
      getOperationLogList(this.stores.operationlog.query).then((res) => {
        this.stores.operationlog.data = res.data.data;
        this.stores.operationlog.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      //this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadOperationLogList();
    },
    handleSearchOperationLog() {
      this.loadOperationLogList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.operationlog.query.sort.direction = column.order;
      this.stores.operationlog.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.operationlog.query.currentPage = page;
      this.loadOperationLogList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.operationlog.query.pageSize = pageSize;
      this.loadOperationLogList();
    },
    handleDatePickerOnChange(date){
      this.stores.operationlog.query.startTime = date[0];
      this.stores.operationlog.query.endTime = date[1];
      this.loadOperationLogList();
    },
  },
  mounted() {
    this.loadOperationLogList();
  },
};
</script>
<style>
</style>
