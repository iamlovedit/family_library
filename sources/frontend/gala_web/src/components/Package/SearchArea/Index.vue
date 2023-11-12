<template>
    <div class="w-full box-border h-full flex flex-col flex-nowrap gap-5 ">
        <n-input-group>
            <n-input :v-model:value="searchValue" type="text" :placeholder="t('package.placeholder')" clearable />
            <n-button type="primary" bordered keyboard strong @click="handleSearch" :loading="loading">
                搜索
            </n-button>
        </n-input-group>
        <n-radio-group v-model:value="orderValue" :on-update:value="handleUpdateValue" :loading="loading">
            <n-radio-button v-for="order in orders" :key="order.value" :value="order.value" :label="order.label" />
        </n-radio-group>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { t } from '@/locales';

const route = useRoute()
const { orderBy, keyword } = route.query;

const router = useRouter()
const searchValue = ref<string>(keyword as string)
const orderValue = ref(orderBy)
const loading = ref<boolean>(false)

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

function handleUpdateValue(value: string) {
    orderValue.value = value;
    router.push({
        name: 'package-search',
        query: {
            keyword: searchValue.value,
            orderBy: value
        }
    })
}

function handleSearch() {
    router.push({
        name: 'package-search',
        query: {
            keyword: searchValue.value,
            orderBy: orderValue.value
        }
    })
}
</script>
