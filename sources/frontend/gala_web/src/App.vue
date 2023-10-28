<template>
  <n-config-provider :theme="darkTheme">
    <n-global-style />
    <div class="w-full box-border min-h-screen flex flex-col justify-between">
      <SiteHeader />
      <router-view />
      <SiteFooter />
    </div>
  </n-config-provider>
</template>

<script setup lang="ts">
import { darkTheme, lightTheme } from "naive-ui";

import SiteFooter from '@components/SiteFooter/Index.vue';
import SiteHeader from '@components/SiteHeader/Index.vue';
</script>