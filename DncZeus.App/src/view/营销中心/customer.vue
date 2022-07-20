<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.customer.query.totalCount"
        :pageSize="stores.customer.query.pageSize"
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
                      v-model="stores.customer.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchCustomer()"
                    >
                    </Input>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
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
            :data="stores.customer.data"
            :columns="stores.customer.columns"
            @on-select="handleSelect"
            @on-selection-change="handleSelectionChange"
            @on-refresh="handleRefresh"
            :row-class-name="rowClsRender"
            @on-page-change="handlePageChanged"
            @on-page-size-change="handlePageSizeChanged"
            @on-sort-change="handleSortChange"
          >
          <template slot-scope="{row,index}" slot="sex">
            <span>{{renderSex(row.wxSex)}}</span>
          </template>
        </Table>
      </dz-table>
    </Card>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getCustomerList,
} from "@/api/营销中心/customer";
export default {
  name: "customer_page",
  components: {
    DzTable
  },
  data() {
    return {
      stores: {
        customer: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            sort: [
              {
                direct: "DESC",
                field: "CreatedOn"
              }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "微信昵称", key: "wxNickname", width: 150, sortable: true },
            { title: "微信OpenID", key: "wxOpenid", width: 150},
            { title: "用户性别", width: 150, key: "wxSex", slot: "sex" },
            { title: "最后登录", width: 250, ellipsis: true, tooltip: true, key: "lastLogin",sortable: true },
            { title: "已绑定学员数量", width: 150, key: "bindTraineesCount"},
            { title: "创建时间", ellipsis: true, tooltip: true, key: "createdOn",sortable: true },
            //{ title: "操作", align: "center", width: 150, className: "table-command-column",slot:"action" }
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
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.guid);
    }
  },
  methods: {
    loadCustomerList() {
      getCustomerList(this.stores.customer.query).then(res => {
        this.stores.customer.data = res.data.data;
        this.stores.customer.query.totalCount = res.data.totalCount;
      });
    },
    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      //this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadCustomerList();
    },
    handleSearchCustomer() {
      this.loadCustomerList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.customer.query.sort.direction = column.order;
      this.stores.customer.query.sort.field = column.key;
      //this.loadPostList();
    },
    handlePageChanged(page) {
      this.stores.customer.query.currentPage = page;
      this.loadCustomerList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.customer.query.pageSize = pageSize;
      this.loadCustomerList();
    },
    renderSex(sex){
      var customerSexText = "未设置";
      switch (sex) {
        case 0:
          customerSexText = "未设置";
          break;
        case 1:
          customerSexText = "男";
          break;
        case 2:
          customerSexText = "女";
          break;
      }
      return customerSexText;
    },
  },
  mounted() {
    this.loadCustomerList();
  }
};
</script>

<style>
</style>
