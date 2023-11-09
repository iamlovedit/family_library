import { createRouter, createWebHistory } from 'vue-router'
import { App } from 'vue'
import routes from './routes'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
  scrollBehavior() {
    return { left: 0, top: 0 }
  }
})

export async function setupRouter(app: App) {
  app.use(router)
  await router.isReady()
}
export default router
