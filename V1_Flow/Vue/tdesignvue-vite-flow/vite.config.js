import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import {resolve} from 'path'
export default defineConfig({
  plugins: [vue(),vueJsx()],
  resolve:{
    alias:{
      "@":resolve(__dirname,"./src")
    }
  },  
  server: {
    //方式二:设置多个代理
    proxy: {
      "/FlowV1Service": {
        target: "http://localhost:20000",
        changeOrigin: true, //必须要开启跨域
        rewrite: (path) => path.replace(/\/FlowV1Service/, "FlowV1Service"), // 路径重写
      },
    },
    fs: {
      // Allow serving files from one level up to the project root
      allow: ['..'],
    },
    port:5174
  },
})
