<template>
    <n-layout-header bordered style="height: var(--header-height)" class="px-3 flex flex-col justify-center flex-nowrap">
        <n-page-header>
            <template #title>
                <n-space :algin="'center'">
                    <div>
                        <a href="/" style="text-decoration: none; color: inherit" class="inline-block">
                            gala
                        </a>
                    </div>
                    <n-menu :options="menuOptions" mode="horizontal" @update:value="handleUpdateValue"
                        :default-value="defaultMenuItem" />
                </n-space>
            </template>
            <template #avatar>
                <site-icon />
            </template>
            <template #extra>
                <n-space>
                    <n-button circle @click="switchTheme">
                        <n-icon :component="themeIcon" />
                    </n-button>
                    <n-dropdown :options="languageOptions" placement="bottom-start">
                        <n-button circle>
                            <n-icon :component="Language" />
                        </n-button>
                    </n-dropdown>
                </n-space>
            </template>
        </n-page-header>
    </n-layout-header>
</template>

<script setup lang="ts">
import type { MenuOption } from 'naive-ui';
import { h, Component, ref, onMounted, watch, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { NIcon, useMessage } from 'naive-ui'
import SiteIcon from '@components/icons/SiteIcon.vue'
import {
    BookOutline as BookIcon,
    HomeOutline as HomeIcon,
    FileTrayFullSharp as FileIcon,
    Language
} from '@vicons/ionicons5'
import { DarkModeRound, LightModeRound } from '@vicons/material';
import { useAppStore } from '@/stores/modules/app';


function renderIcon(icon: Component) {
    return () => h(NIcon, null, { default: () => h(icon) })
}
const defaultMenuItem = ref<string | null>('packages');
const message = useMessage()
const route = useRoute();
const router = useRouter();
const appStore = useAppStore();

const menuOptions: MenuOption[] = [
    {
        label: '首页',
        key: 'home',
        icon: renderIcon(HomeIcon)
    },
    {
        label: '节点包',
        key: 'packages',
        icon: renderIcon(BookIcon),
    },
    {
        label: '族库',
        key: 'families',
        icon: renderIcon(FileIcon)
    }
]


const languageOptions = [
    {
        label: 'English',
        key: 'en-US',
        props: {
            onClick: () => {
                appStore.setLanguage('en-US');
            }
        },
    },
    {
        label: '简体中文',
        key: 'zh-CN',
        props: {
            onClick: () => {
                appStore.setLanguage('zh-CN');
            },
        },
    }
];
// onMounted(() => {
//     defaultMenuItem.value = route.name as string;
// })

// watch(route, () => {
//     defaultMenuItem.value = route.name as string;
// })

const themeIcon = computed(() => {
    return appStore.appState.theme == 'dark' ? DarkModeRound : LightModeRound;
});

function handleUpdateValue(key: string) {
    router.push({
        name: key,
        params: {
            lang: 'zh-CN'
        }
    })
}

function switchTheme() {
    appStore.switchTheme();
}

</script>