import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import TDesign from 'tdesign-vue-next';
import router from './router/router'
import { MessagePlugin } from "tdesign-vue-next";
import instance from './utils/http'
import cookies from './utils/cookies'
import api from './api/index'
import moment from 'moment'



// 引入组件库的少量全局样式变量
import 'tdesign-vue-next/es/style/index.css';
var app=createApp(App)
app.use(TDesign)
app.use(router)
app.use(moment)

app.provide('$message', MessagePlugin)
app.provide('$instance', instance)
app.provide('$cookies', cookies)
app.provide('$api', api)

app.config.globalProperties.$message = MessagePlugin;
app.config.globalProperties.$instance = instance;
app.config.globalProperties.$cookies = cookies;
app.config.globalProperties.$api = api;
app.mount('#app')
