import { defineStore } from 'pinia'
import { getLocalSetting, setLocalSetting, type AppState, type Language, type Theme } from './helper';
import { ref } from 'vue';
import { store } from '@/stores/helper'

export const useAppStore = defineStore('app', () => {
    const appState = ref<AppState>(getLocalSetting());
    function updateTheme(theme: Theme) {
        appState.value.theme = theme;
        saveState();
    }

    function updateLanguage(language: Language) {
        if (appState.value.language !== language) {
            appState.value.language = language;
            saveState();
        }
    }

    function saveState() {
        setLocalSetting(appState.value);
    }
    function switchTheme() {
        appState.value.theme = appState.value.theme === 'dark' ? 'light' : 'dark';
        saveState();
    }

    return {
        appState,
        updateTheme,
        updateLanguage,
        switchTheme
    }
})

export function useAppStoreWithOut() {
    return useAppStore(store)
}