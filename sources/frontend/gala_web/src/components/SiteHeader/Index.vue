<template>
    <n-layout-header bordered style="height: var(--header-height)" class="px-3 flex flex-row justify-center flex-nowrap">
        <div>
            1
        </div>
        <div>
            2
        </div>
    </n-layout-header>
</template>

<script setup lang="ts">
import type { MenuOption } from 'naive-ui';
import { defineComponent, h, Component } from 'vue'
import { RouterLink } from 'vue-router'
import { NIcon, useMessage } from 'naive-ui'

function renderIcon(icon: Component) {
    return () => h(NIcon, null, { default: () => h(icon) })
}

const menuOptions: MenuOption[] = [

]

</script>