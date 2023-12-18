import naive from 'naive-ui'
import { createApp } from 'vue'
import { setupAssets, setupScrollbarStyle } from '@/plugins'
import App from '@/App.vue'
import { setupStore } from '@/stores'
import { setupI18n } from '@/locales'
import { setupRouter } from '@/router'

async function bootstrap() {
    const app = createApp(App)

    setupAssets();

    setupScrollbarStyle();

    setupStore(app);

    setupI18n(app)

    await setupRouter(app)

    app.use(naive)

    app.mount('#app')
}

bootstrap();
