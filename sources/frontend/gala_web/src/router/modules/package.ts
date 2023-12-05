import { RouteRecordRaw } from 'vue-router';

const packageRoutes: RouteRecordRaw[] = [
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
]
export default packageRoutes