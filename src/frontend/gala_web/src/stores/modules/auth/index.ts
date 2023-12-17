import { defineStore } from "pinia";
import { ref } from 'vue'
import { TokenInfo, getTokenInfo, setTokenInfo, removeTokenInfo } from "./helper";
export const useAuthStore = defineStore("auth", () => {
    const tokenState = ref<TokenInfo>(getTokenInfo())
    function updateToken(tokenInfo: TokenInfo) {
        tokenState.value = tokenInfo
        saveState()
    }
    function clearToken() {
        removeTokenInfo();
    }

    function saveState() {
        setTokenInfo(tokenState.value)
    }
    return {
        tokenState,
        updateToken,
        clearToken
    }
});