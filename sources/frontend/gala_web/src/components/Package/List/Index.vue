<template>
    <div class="w-full box-border h-full flex flex-col flex-nowrap gap-5 ">
        <div class="flex-1">
            <n-list hoverable clickable>
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
        </div>
        <n-pagination v-model:page="page" :page-count="pageCount" :on-update:page=handleUpdatePage />
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const page = ref<number>();
const pageCount = ref<number>();

function handleUpdatePage(value: number) {
    console.log(value)
}
</script>
