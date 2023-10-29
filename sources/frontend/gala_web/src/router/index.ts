import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('@/views/HomeView.vue')
  },
  {
    path: '/packages',
    name: 'packages',
    component: () => import('@/views/Package/Index.vue')
  },
  {
    path: '/families',
    name: 'families',
    component: () => import('@/views/Family/Index.vue')
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes
})

export default router
