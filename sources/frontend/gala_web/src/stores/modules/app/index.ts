import { defineStore } from 'pinia'
import { getLocalSetting, setLocalSetting, type AppState, type Language, type Theme } from './helper';
import { ref } from 'vue';

const appSetting: AppState = getLocalSetting();

export const useAppStore = defineStore('app', () => {
    const appState = ref<AppState>(appSetting);

    function switchTheme() {
        appState.value.theme = appState.value.theme === 'light' ? 'dark' : 'light';
        saveState();
    }

    function setLanguage(langue: Language) {
        appState.value.language = langue;
        saveState();
    }

    function saveState(): void {
        setLocalSetting(appState.value);
    }

    return {
        appState,
        switchTheme,
        setLanguage
    }
})