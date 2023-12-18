<template>
  <n-config-provider :theme="theme" :locale="language" :data-locale="dataLocale" :theme-overrides="themeOverrides">
    <n-global-style />
    <naive-provider>
      <router-view />
    </naive-provider>
  </n-config-provider>
</template>

<script setup lang="ts">

import { useEventListener } from "@vueuse/core";
import NaiveProvider from '@components/NaiveProvider/Index.vue';
import { useTheme } from '@/hooks/useTheme'
import { useLanguage } from '@/hooks/useLanguage';

const { theme, themeOverrides } = useTheme()
const { language, dataLocale } = useLanguage()

function setFullHeight(): void {
  const headerHeight = parseInt(getComputedStyle(document.documentElement).getPropertyValue('--header-height'), 10);
  const windowHeight = window.innerHeight;
  document.documentElement.style.setProperty('--full-height', `${windowHeight - headerHeight}px`);
}
useEventListener('resize', setFullHeight);

</script>