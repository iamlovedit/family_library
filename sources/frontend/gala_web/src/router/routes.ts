import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: 'home',
        component: () => import('@/views/HomeView.vue')
    },
    {
        path: '/packages',
        component: () => import('@/views/Package/Index.vue'),
        children: [
            {
                path: '',
                name: 'packages',
                component: () => import('@/views/Package/Browser/Index.vue')
            },
            {
                path: ':id',
                name: 'package-details',
                component: () => import('@/views/Package/Details/Index.vue')
            },
            {
                path: 'search',
                name: 'package-search',
                component: () => import('@/views/Package/Search/Index.vue')
            }
        ]
    },
    {
        path: '/families',
        component: () => import('@/views/Family/Index.vue'),
        children: [
            {
                path: '',
                name: 'families',
                component: () => import('@/views/Family/Browser/Index.vue')
            }
        ]
    },
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