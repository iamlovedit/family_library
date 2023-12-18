import type { GlobalThemeOverrides } from "naive-ui"
import { computed, watch } from 'vue'
import { darkTheme, useOsTheme, lightTheme } from 'naive-ui'
import { useAppStore } from "@/stores/modules/app"

export function useTheme() {
    const appStore = useAppStore()
    const osTheme = useOsTheme()

    const isDark = computed(() => {
        return appStore.appState.theme === 'auto' ? osTheme.value === 'dark' : appStore.appState.theme === 'dark'
    })

    const theme = computed(() => {
        return isDark.value ? darkTheme : lightTheme
    })

    const themeOverrides = computed<GlobalThemeOverrides>(() => {
        if (isDark.value) {
            return {
                common: {},
            }
        }
        return {}
    })

    watch(() => isDark.value,
        (dark) => {
            if (dark) {
                document.documentElement.classList.add('dark')
            }
            else {
                document.documentElement.classList.remove('dark')
            }
        },
        { immediate: true },
    )
    return {
        theme,
        themeOverrides
    }
}
