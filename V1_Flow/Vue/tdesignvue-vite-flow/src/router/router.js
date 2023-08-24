import { createRouter, createWebHistory } from "vue-router";
import login from './login/index'
import layout from './layout/index'
const routers = [
  ...login,
  ...layout,
]

const router = createRouter({
  // 4. 内部提供了 history 模式的实现。为了简单起见，我们在这里使用 hash 模式。
  history: createWebHistory(),
  routes: routers,
})
router.beforeEach((to, from, next) => {
  next()
})
export default router