import { RouteRecordRaw } from 'vue-router';
import packageRoutes from './package';
import familyRoutes from './family';

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: 'home',
        component: () => import('@/views/HomeView.vue')
    },
    ...packageRoutes,
    ...familyRoutes,
    {
        path: '/:pathMatch(.*)*',
        name: 'notFound',
        redirect: '/404',
    },
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
    },
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
export default routes;