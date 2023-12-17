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
                        v-model:value="activeKey" />
                </n-space>
            </template>
            <template #avatar>
                <site-icon />
            </template>
            <template #extra>
                <n-space :algin="'center'">
                    <n-button circle @click="switchTheme">
                        <n-icon :component="themeIcon" />
                    </n-button>
                    <n-dropdown :options="languageOptions" placement="bottom-start">
                        <n-button circle>
                            <n-icon :component="LanguageIcon" />
                        </n-button>
                    </n-dropdown>
                </n-space>
            </template>
        </n-page-header>
    </n-layout-header>
</template>

<script setup lang="ts">
import type { MenuOption } from 'naive-ui';
import { h, Component, computed, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { NIcon } from 'naive-ui'
import SiteIcon from '@components/icons/SiteIcon.vue'
import {
    BookOutline as BookIcon,
    HomeOutline as HomeIcon,
    FileTrayFullSharp as FileIcon,
    Language as LanguageIcon
} from '@vicons/ionicons5'
import { DarkModeRound, LightModeRound } from '@vicons/material';
import { useAppStore } from '@/stores/modules/app';
import { t } from '@/locales'



function renderIcon(icon: Component) {
    return () => h(NIcon, null, { default: () => h(icon) })
}
const route = useRoute();
const router = useRouter();
const appStore = useAppStore();
const activeKey = ref<string>(route.name as string);
const menuOptions: MenuOption[] = [
    {
        label: t('header.home'),
        key: 'home',
        // icon: renderIcon(HomeIcon)
    },
    {
        label: t('header.packages'),
        key: 'packages',
        // icon: renderIcon(BookIcon),
    },
    {
        label: t('header.family'),
        key: 'families',
        // icon: renderIcon(FileIcon)
    }
]


const languageOptions = [
    {
        label: 'English',
        key: 'en-US',
        props: {
            onClick: () => {
                appStore.updateLanguage('en-US')
            }
        },
    },
    {
        label: '简体中文',
        key: 'zh-CN',
        props: {
            onClick: () => {
                appStore.updateLanguage('zh-CN')
            },
        },
    }
];

const themeIcon = computed(() => {
    return appStore.appState.theme == 'dark' ? DarkModeRound : LightModeRound;
});

function handleUpdateValue(key: string) {
    router.push({
        name: key
    })
}

function switchTheme() {
    appStore.switchTheme();
}

</script>