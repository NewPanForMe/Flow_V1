import home from '../home/index'
const router = [
    {
        path: "/index",
        name: "index",
        meta:{title:"框架"},
        component: () => import('@/views/system/layout/index.vue'),
        children:[
           ...home,
        ]
    },
];
export default router;
