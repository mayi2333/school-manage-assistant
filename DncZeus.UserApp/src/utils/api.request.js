import HttpRequest from '@/utils/axios'
import settings from '@/settings'
const baseUrl = process.env.NODE_ENV === 'development' ? settings.baseUrl.dev : settings.baseUrl.pro
const defaultPrefix = settings.baseUrl.defaultPrefix;
const axios = new HttpRequest(baseUrl,defaultPrefix)
export default axios
