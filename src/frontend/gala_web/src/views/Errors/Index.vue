<template>
    <site-content>
        <div class="h-full flex flex-nowrap flex-1 justify-center items-center">
            <n-result :status="status" :title="title" :description="description">
                <template #footer>
                    <n-button @click="$router.go(-1)">
                        返回
                    </n-button>
                </template>
                <template #icon>
                    <slot name="icon" />
                </template>
            </n-result>
        </div>
    </site-content>
</template>

<script setup lang="ts">
import SiteContent from '@components/Layout/SiteContent/Index.vue';
interface Props {
    status: string,
    title?: string,
    description?: string
}
defineProps<Props>()
</script>
