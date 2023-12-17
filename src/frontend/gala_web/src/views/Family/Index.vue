<template>
    <site-content>
        <div class="w-3/5 my-40px mx-auto box-border flex flex-col flex-1 flex-nowrap">
            <router-view />
        </div>
    </site-content>
</template>
<script setup lang="ts">
import SiteContent from '@components/Layout/SiteContent/Index.vue';
</script>
