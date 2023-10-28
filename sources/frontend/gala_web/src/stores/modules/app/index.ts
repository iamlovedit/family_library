import { defineStore } from 'pinia'
import { getLocalSetting, setLocalSetting, type AppState, type Language, type Theme } from './helper';
import { ref } from 'vue';

const appSetting: AppState = getLocalSetting();

export const useAppStore = defineStore('app', () => {
    const appState = ref<AppState>(appSetting);

    function setTheme(theme: Theme) {
        appState.value.theme = theme;
        saveState();
    }

    function setLangue(langue: Language) {
        appState.value.language = langue;
        saveState();
    }

    function saveState(): void {
        setLocalSetting(appState.value);
    }

    return {
        appState,
        setTheme,
        setLangue
    }
})