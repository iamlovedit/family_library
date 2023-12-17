import { RouteRecordRaw } from 'vue-router';

const errorRouters: RouteRecordRaw[] = [
    {
        path: '/404',
        name: '404',
        component: () => import('@/views/Errors/404/Index.vue')
    },
    {
        path: '/500',
        name: '500',
        component: () => import('@/views/Errors/500/Index.vue')
    },
    {
        path: '/403',
        name: '403',
        component: () => import('@/views/Errors/403/Index.vue')
    }
]

export default errorRouters;