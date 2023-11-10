<template>
    <site-content>
        <div class="w-3/5 my-40px mx-auto box-border flex flex-col flex-1 flex-nowrap  justify-between gap-5">
            <n-input type="text" :placeholder="t('package.placeholder')" clearable />
            <n-radio-group>
                <n-radio-button v-for="order in orders" :key="order.value" :value="order.value" :label="order.label" />
            </n-radio-group>
            <div class="flex-1">
                <router-view />
            </div>
        </div>
    </site-content>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import SiteContent from '@components/Layout/SiteContent/Index.vue';

import { t } from '@/locales';
const orders = [
    {
        value: 'name',
        label: t('package.order.name')
    },
    {
        value: 'downloads',
        label: t('package.order.downloads')
    },
    {
        value: 'votes',
        label: t('package.order.votes')
    },
    {
        value: 'created',
        label: t('package.order.createdDate')
    }
]


</script>
