import axios from 'axios'
import { Toast } from 'vant'
import { getToken } from '@/utils/auth'

class HttpRequest {
  constructor(baseUrl = baseURL, defaultPrefix= defaultPrefix) {
    //console.log(process)
    //console.log(defaultPrefix)
    this.baseUrl = baseUrl
    this.defaultPrefix = defaultPrefix
    this.queue = {}
  }
  getInsideConfig() {
    const config = {
      baseURL: this.baseUrl,
      headers: {
        "Authorization": "Bearer " + getToken()
      }
    }
    return config
  }
  destroy(url) {
    delete this.queue[url]
    if (!Object.keys(this.queue).length) {
      // Spin.hide()
    }
  }

  showError(error, errorInfo) {
    let message = "接口服务错误,请稍候再试.";

    let statusCode = -1;
    if (error.response && error.response.status) {
      statusCode = error.response.status;
    }
    switch (statusCode) {
      case 401:
        message = "接口服务错误,原因:未授权的访问";
        break;
      case 500:
        message = "接口服务错误,原因:[" + error.response.statusText + "]";
        break;
      case -1:
        message = "网络出错,请检查你的网络或者服务是否可用";
        break;
    }
    Toast.fail({
      message: message,
      position: 'top',
      duration: 3 * 1000
    })
  }

  interceptors(instance, url) {
    // 请求拦截
    instance.interceptors.request.use(config => {
      // 添加全局的loading...
      if (!Object.keys(this.queue).length) {
        // Spin.show() // 不建议开启，因为界面不友好
      }
      this.queue[url] = true
      return config
    }, error => {
      console.log(error, 'err') // for debug
      return Promise.reject(error)
    })
    // 响应拦截
    instance.interceptors.response.use(res => {
      this.destroy(url)
      const {
        data,
        status
      } = res
      return {
        data,
        status
      }
    }, error => {
      this.destroy(url)
      if (error.config.hideError == null || error.config.hideError == false) {
        this.showError(error);
        console.log(error, 'err')
      }
      return Promise.reject(error)
    })
  }
  request(options) {
    const instance = axios.create()
    let withPrefix = true
    if (options.withPrefix !== undefined && options.withPrefix == false) {
      withPrefix = false
    }
    let url = options.url
    if (options.prefix !== undefined && options.prefix.length > 0) {
      url = options.prefix + options.url
    }
    else if (withPrefix) {
      url = this.defaultPrefix + options.url
    }
    options.url = url
    options = Object.assign(this.getInsideConfig(), options)
    //console.log(options)
    this.interceptors(instance, options.url)
    return instance(options)
  }

}
export default HttpRequest
