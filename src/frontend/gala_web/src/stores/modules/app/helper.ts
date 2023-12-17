import { ss } from "@/utils/storage";

const LOCAL_NAME = 'appSetting'

export type Language = 'zh-CN' | 'en-US'

export type Theme = 'light' | 'dark' | 'auto'

export interface AppState {
    theme: Theme
    language: string
}

export function defaultSetting(): AppState {
    return { theme: 'light', language: 'en-US' }
}

export function getLocalSetting(): AppState {
    const localSetting: AppState | undefined = ss.get(LOCAL_NAME)
    return { ...defaultSetting(), ...localSetting }
}

export function setLocalSetting(setting: AppState): void {
    ss.set(LOCAL_NAME, setting)
}