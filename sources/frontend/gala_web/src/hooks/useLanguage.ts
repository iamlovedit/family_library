import { computed } from "vue"
import { enUS, zhCN } from "naive-ui"
import { useAppStore } from "@/stores/modules/app"
import { setLocale } from '@/locales'

export function useLanguage() {
    const appStore = useAppStore()
    const language = computed(() => {
        if (appStore.appState.language === 'en-US') {
            setLocale('en-US')
            return enUS
        } else {
            setLocale('zh-CN')
            return zhCN
        }
    });

    return {
        language
    }
}