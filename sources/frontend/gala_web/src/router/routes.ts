import { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: 'home',
        component: () => import('@/views/HomeView.vue')
    },
    {
        path: '/packages',
        name: 'packages',
        component: () => import('@/views/Package/Index.vue'),
        children: [
            {
                path: '',
                name: 'packages',
                component: () => import('@/views/Package/Browser/Index.vue')
            }
        ]
    },
    {
        path: '/families',
        name: 'families',
        component: () => import('@/views/Family/Index.vue'),
        children: [
            {
                path: '',
                component: () => import('@/views/Family/Browser/Index.vue')
            }
        ]
    }
]
export default routes;