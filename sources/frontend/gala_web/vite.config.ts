import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import UnoCSS from 'unocss/vite'

export default defineConfig({
  plugins: [
    vue(),
    UnoCSS()
  ],
  resolve: {
    alias: [
      { find: '@', replacement: fileURLToPath(new URL('src/', import.meta.url)) },
      { find: '@assets', replacement: fileURLToPath(new URL('src/assets', import.meta.url)) },
      { find: '@stores', replacement: fileURLToPath(new URL('src/stores', import.meta.url)) },
      { find: '@models', replacement: fileURLToPath(new URL('src/models', import.meta.url)) },
      { find: '@components', replacement: fileURLToPath(new URL('src/components', import.meta.url)) },
      { find: '@styles', replacement: fileURLToPath(new URL('src/styles', import.meta.url)) },
      { find: '@utils', replacement: fileURLToPath(new URL('src/utils', import.meta.url)) },
      { find: '@services', replacement: fileURLToPath(new URL('/src/services', import.meta.url)) },
    ]
  },
  css: {
    preprocessorOptions: {
      scss: {
        javascriptEnabled: true,
        additionalData: '@import "@styles/variable.scss";'
      }
    }
  },
  define: {
    'import.meta.env.PACKAGE_VERSION': JSON.stringify(process.env.npm_package_version)
  }
})
