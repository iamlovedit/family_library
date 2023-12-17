import { RouteRecordRaw } from 'vue-router';
const authRoutes: RouteRecordRaw[] = [
    {
        path: '/login',
        name: 'login',
        component: () => import('@/views/Login/Index.vue')
    },
    {
        path: '/register',
        name: 'register',
        component: () => import('@/views/Register/Index.vue')
    }
]

export default authRoutes;