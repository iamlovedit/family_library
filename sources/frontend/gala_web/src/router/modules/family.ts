import { RouteRecordRaw } from 'vue-router';

const familyRoutes: RouteRecordRaw[] = [
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
]

export default familyRoutes