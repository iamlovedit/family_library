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
import { h, Component, ref, watch, computed } from 'vue'
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


function renderIcon(icon: Component) {
    return () => h(NIcon, null, { default: () => h(icon) })
}
const activeKey = ref<string | null>();
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

watch(() => route.path, () => {
    activeKey.value = route.matched[0].name as string
})

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