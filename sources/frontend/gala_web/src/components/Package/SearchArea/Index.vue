<template>
    <div class="w-full box-border h-full flex flex-col flex-nowrap gap-5 justify-between flex-1">
        <n-input-group>
            <n-input v-model:value="searchValue" type="text" :placeholder="t('package.placeholder')" clearable
                :on-clear="handleInputClear" />
            <n-button type="primary" bordered keyboard strong @click="handleSearch" :loading="loading">
                {{ t('common.search') }}
            </n-button>
        </n-input-group>
        <n-radio-group v-model:value="orderValue" :on-update:value="handleUpdateValue" :loading="loading">
            <n-radio-button v-for="order in orders" :key="order.value" :value="order.value" :label="order.label" />
        </n-radio-group>

        <n-list bordered hoverable show-divider class="flex-1">
            <n-list-item>
                <n-thing title="相见恨晚" content-style="margin-top: 10px;">
                    <template #description>
                        <n-space size="small" style="margin-top: 4px">
                            <n-tag :bordered="false" type="info" size="small">
                                暑夜
                            </n-tag>
                            <n-tag :bordered="false" type="info" size="small">
                                晚春
                            </n-tag>
                        </n-space>
                    </template>
                    奋勇呀然后休息呀<br>
                    完成你伟大的人生
                </n-thing>
            </n-list-item>
            <n-list-item>
                <n-thing title="他在时间门外" content-style="margin-top: 10px;">
                    <template #description>
                        <n-space size="small" style="margin-top: 4px">
                            <n-tag :bordered="false" type="info" size="small">
                                环形公路
                            </n-tag>
                            <n-tag :bordered="false" type="info" size="small">
                                潜水艇司机
                            </n-tag>
                        </n-space>
                    </template>
                    最新的打印机<br>
                    复制着彩色傀儡<br>
                    早上好我的罐头先生<br>
                    让他带你去被工厂敲击
                </n-thing>
            </n-list-item>
        </n-list>
        <n-pagination v-model:page="page" :page-count="pageCount" :on-update:page=handleUpdatePage />
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
const orderValue = ref(orderBy || 'default')
const loading = ref<boolean>(false)
const page = ref<number>();
const pageCount = ref<number>();
const orders = [
    {
        value: 'default',
        label: t('package.order.default')
    },
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
    router.push({
        name: 'package-search',
        query: {
            keyword: searchValue.value,
            orderBy: value
        }
    })
}

function handleInputClear() {
    router.push({
        name: 'packages'
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
function handleUpdatePage(value: number) {
    console.log(value)
}
</script>
