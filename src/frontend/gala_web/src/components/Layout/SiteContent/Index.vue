<template>
    <div class="w-full box-border min-h-screen flex flex-col flex-nowrap justify-between">
        <site-header />
        <div class="flex-1 w-full flex flex-col flex-nowrap h-full">
            <slot />
        </div>
        <site-footer />
    </div>
</template>

<script setup lang="ts">
import SiteFooter from '@components/Layout/SiteFooter/Index.vue';
import SiteHeader from '@components/Layout/SiteHeader/Index.vue';
</script>
