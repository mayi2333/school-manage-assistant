<template>
  <div class="idcard-scan"></div>
</template>
<script>
let idCardVue = {
  name: "IdCardScan",
  props: {},
  data() {
    return {
      idCard: "",
      keyupLastTime: undefined,
    };
  },
  created() {
    //let that = this;
    //// 监听页面的keyup事件
    //document.onkeyup = function (e) {
    //  that.handleKeyUp(e);
    //};
  },
  methods: {
    // 处理keyup事件
    handleKeyUp(e) {
      let gap = 0;
      if (this.keyupLastTime) {
        gap = new Date().getTime() - this.keyupLastTime;
        //console.log(gap);
        if (gap >= 50) {
          gap = 0;
          this.idCard = "";
        }
      }
      this.keyupLastTime = new Date().getTime();
      // 输入法会触发keyup事件，key为Process，跳过即可
      if (e.key != "Process" && gap < 50) {
        if (e.key.trim().length == 1) {
          // 输入单个字母或者数字
          this.idCard += e.key;
        } else if (e.key.trim() == "Enter") {
          //e.preventDefault();
          //e.stopPropagation();
          //e.returnValue = false;
          //e.defaultPrevented = false;
          //console.log(e);
          // 如果卡号大于7位 返回数据
          //console.log(this.idCard.length);
          if (this.idCard.length > 4) {
            let data = {
              card: this.idCard,
            };
            this.$emit("handle", data);
            this.idCard = "";
          }
        }
      }
    },
    bindOnKeyUp() {
      let that = this;
      // 监听页面的keyup事件
      document.onkeyup = function (e) {
        that.handleKeyUp(e);
      };
    },
  },
};
export default { ...idCardVue };
</script>
