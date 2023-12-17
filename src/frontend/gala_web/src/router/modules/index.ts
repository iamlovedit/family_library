import { RouteRecordRaw } from 'vue-router';
import packageRoutes from './package';
import familyRoutes from './family';
import authRoutes from './auth';
import errorRouters from './errors';

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: 'home',
        component: () => import('@/views/HomeView.vue')
    },
    {
        path: '/:pathMatch(.*)*',
        name: 'notFound',
        redirect: '/404',
    },
    ...packageRoutes,
    ...familyRoutes,
    ...authRoutes,
    ...errorRouters,
]
export default routes;