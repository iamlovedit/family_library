import { computed } from "vue"
import { enUS, zhCN, dateZhCN, dateEnUS } from "naive-ui"
import { useAppStore } from "@/stores/modules/app"
import { setLocale } from '@/locales'

const appStore = useAppStore()
export function useLanguage() {
    const isEn = computed(() => {
        return appStore.appState.language === 'en-US'
    })
    const language = computed(() => {
        if (isEn.value) {
            setLocale('en-US')
            return enUS
        } else {
            setLocale('zh-CN')
            return zhCN
        }
    });

    const dataLocale = computed(() => {
        return isEn.value ? dateEnUS : dateZhCN;
    })
    return {
        language,
        dataLocale
    }
}