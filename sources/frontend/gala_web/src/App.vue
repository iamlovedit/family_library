<template>
  <n-config-provider :theme="theme" :locale="locale" :data-locale="dataLocale">
    <n-global-style />
    <n-message-provider>
      <div class="w-full box-border min-h-screen flex flex-col justify-between">
        <SiteHeader />
        <router-view />
        <SiteFooter />
      </div>
    </n-message-provider>
  </n-config-provider>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { darkTheme, lightTheme } from "naive-ui";
import { zhCN, dateZhCN } from 'naive-ui'
import { enUS, dateEnUS } from 'naive-ui'
import { useEventListener } from "@vueuse/core";
import SiteFooter from '@components/SiteFooter/Index.vue';
import SiteHeader from '@components/SiteHeader/Index.vue';
import { useAppStore } from '@stores/modules/app'

const appStore = useAppStore()

function setFullHeight(): void {
  const headerHeight = parseInt(getComputedStyle(document.documentElement).getPropertyValue('--header-height'), 10);
  const windowHeight = window.innerHeight;
  document.documentElement.style.setProperty('--full-height', `${windowHeight - headerHeight}px`);
}
useEventListener('resize', setFullHeight);

const theme = computed(() => {
  return appStore.appState.theme === 'dark' ? darkTheme : lightTheme
})
const locale = computed(() => {
  return appStore.appState.language === 'zh-CN' ? zhCN : enUS
});

const dataLocale = computed(() => {
  return appStore.appState.language === 'zh-CN' ? dateZhCN : dateEnUS
})
</script>