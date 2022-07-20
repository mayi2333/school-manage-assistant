<template>
  <div class="face-scan">
    <video ref="faceVideo" id="video" preload autoplay loop muted></video>
    <canvas
      ref="faceCanvas"
      :width="videoWidth"
      :height="videoHeight"
      :style="canvasIsShow ? '' : 'position: fixed; left: 100%'"
    ></canvas>
  </div>
</template>
<script>
// 引入trackingjs所需文件
//import tracking from "tracking/build/tracking-min.js";
//import "tracking/build/data/face-min.js";
//require("tracking/build/data/mouth-min.js");
//require("tracking/examples/assets/stats.min.js");

export default {
  name: "faceScan",
  props: {
    videoWidth: {
      type: Number,
      default: 300,
    },
    videoHeight: {
      type: Number,
      default: 300,
    },
    canvasIsShow: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      videoEle: null,
      canvasEle: null,
      trackerTask: null,
      isReady: true,
      //isRepeat: false,
      constraints: {
        video: {
          deviceId: {
            exact: "",
          },
          width: this.videoWidth,
          height: this.videoHeight,
        },
        audio: false,
      },
      cameraArray: [],
      cameraIndex: 0,
      openCameraLock: false,
      closeCameraLock: false,
    };
  },
  created() {
    //查找所有可用摄像头
    navigator.mediaDevices
      .enumerateDevices()
      .then((devices) => {
        //console.log(devices);
        this.cameraArray = [];
        devices.forEach((device) => {
          if (device.kind == "videoinput") {
            this.cameraArray.push(device.deviceId);
          }
        });
        if (this.cameraArray.length > 0) {
          this.constraints.video.deviceId.exact = this.cameraArray[0]; //设置默认摄像头
        }
      })
      .catch(function (err) {
        console.log(err.name + ": " + err.message);
      });
  },
  methods: {
    openCamera() {
      if (this.trackerTask != null || this.openCameraLock) {
        return;
      }
      this.openCameraLock = true;
      this.closeCamera();
      //this.closeCamera();
      console.log("打开摄像头");
      if (this.cameraArray.length <= 0) {
        this.$Message.warning("没有可用的摄像头");
      }
      this.isReady = true;
      //var video = document.getElementById("video");
      //var canvas = document.getElementById("canvas");
      this.videoEle = this.$refs.faceVideo;
      this.canvasEle = this.$refs.faceCanvas;

      var context = this.canvasEle.getContext("2d");

      var tracker = new tracking.ObjectTracker("face"); // 引入第三方 库

      tracker.setInitialScale(1);
      tracker.setStepSize(2);
      tracker.setEdgesDensity(0.1);

      // 启动摄像头初始化
      this.trackerTask = tracking.track("#video", tracker, { camera: true });
      //-------  指定 canvas 的宽高 ，auto 自动播放
      //console.log(this.trackerTask);
      let promise = navigator.mediaDevices.getUserMedia(this.constraints); // h5 新的API
      //console.log(promise);
      //console.log(this.videoEle);
      promise
        .then((MediaStream) => {
          this.videoEle.srcObject = MediaStream;
          this.videoEle.play();
        })
        .catch((PermissionDeniedError) => {
          console.log(PermissionDeniedError);
        });
      this.tracker_fun(tracker, video, context, this.canvasEle); //open 摄像头，执行tracker_fun( 传入参数 )
      this.openCameraLock = false;
      console.log("摄像头开启完毕");
    },

    //切换摄像头
    switchCamera() {
      if (
        this.cameraArray.length > 0 &&
        !this.closeCameraLock &&
        !this.openCameraLock
      ) {
        if (
          this.constraints.video.deviceId.exact ===
          this.cameraArray[this.cameraIndex]
        ) {
          this.cameraIndex++;
          if (this.cameraIndex >= this.cameraArray.length) {
            this.cameraIndex = 0;
          }
        }
        this.constraints.video.deviceId.exact = this.cameraArray[
          this.cameraIndex
        ];
        //console.log(this.constraints.video.deviceId.exact);
        this.closeCamera();
        this.openCamera();
        return true;
      }
    },
    closeCamera() {
      if (this.videoEle != null && this.videoEle.srcObject != null) {
        console.log("关闭摄像头");
        this.closeCameraLock = true;
        let stream = this.videoEle.srcObject;
        let tracks = stream.getTracks();
        tracks.forEach((track) => {
          track.stop();
        });
        this.trackerTask.stop();
        this.videoEle = null;
        this.trackerTask = null;
        this.closeCameraLock = false;
        console.log("摄像头已关闭");
      }
      // 停止侦测
      //if (this.trackerTask != null) {
      //  console.log("关闭摄像头");
      //  this.$emit("faceupload");
      //  this.videoEle.stop();
      //  this.trackerTask.pause();
      // 关闭摄像头
      //  window.tracking.closeCamera();
      //  window.stream.getTracks().forEach((track) => track.stop()); // 停止侦测
      //  trackerTask = null;
      //}
    },
    tracker_fun(tracker, video, context, canvas) {
      // 每秒 检测人脸 结果
      tracker.on("track", (event) => {
        console.log(this.isReady);
        //console.log(this.first);
        // 视频流
        // context.clearRect(0, 0, canvas.width, canvas.height);
        if (this.isReady && event.data.length > 0) {
          this.drawImage(event.data, video, context, canvas);
          //video.load();
          // if  --- > else  检测到人脸 success() =>{}
        }
      });
    },
    drawImage(data, video, context, canvas) {
      console.log("开始画图");
      data.forEach((rect) => {
        console.log(rect);
        if (rect.x) {
          //this.isRepeat = false;
          this.isReady = false;
          video.pause(); // success  将暂停 video
          //console.log(rect);
          //console.log(video);
          context.drawImage(video, 0, 0, video.videoWidth, video.videoHeight); // 绘制到 canvas
          //context.font = "11px Helvetica";
          //context.fillText("", 100, 40);
          //context.strokeStyle = "#a64ceb";
          //context.strokeRect(rect.x, rect.y, rect.width, rect.height);
          var data_url = canvas.toDataURL("image/png", 1); //base64 request
          video.play();
          //console.log(data_url);
          console.log("faceupload");
          this.$emit("faceupload", data_url);
          return;
        }
      });
    },
    //拍照准备状态重置
    isReadyReset() {
      this.isReady = true;
    },
    //重复拍照时候会检测是否镜头前没有人，没有人则拍照状态重置为准备好了
    //isRepeatReset() {
    //  this.isRepeat = true;
    //},
    //清空画布
    clearCanvas() {
      var context = this.canvasEle.getContext("2d");
      var w = this.canvasEle.width;
      var h = this.canvasEle.height;
      context.clearRect(0, 0, w, h);
    },
  },
  destroyed() {
    this.closeCamera();
  },
};
require("tracking/build/tracking-min.js");
require("tracking/build/data/face-min.js");
</script>