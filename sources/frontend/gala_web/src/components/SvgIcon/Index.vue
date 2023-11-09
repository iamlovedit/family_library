<template>
    <Icon :icon="props.icon" v-bind="bindAttrs" />
</template>

<script setup lang="ts">
import { computed, useAttrs } from 'vue'
import { Icon, IconifyIcon } from '@iconify/vue'

interface Props {
    icon: string | IconifyIcon
}
const props = defineProps<Props>();
const attrs = useAttrs()

const bindAttrs = computed<{ class: string; style: string }>(() => ({
    class: (attrs.class as string) || '',
    style: (attrs.style as string) || '',
}))
</script>