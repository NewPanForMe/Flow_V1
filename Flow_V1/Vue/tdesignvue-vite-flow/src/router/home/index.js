const router = [
    {
        path: "/home2",
        name: "home2",
        meta: { title: "首页" },
        component: () => import('@/views/system/home/index.vue')
    },
 
];
export default router;
