import { ss } from '@/utils/storage'
const LOCAL_NAME = 'SECRET_TOKEN'

export interface TokenInfo {
    token: string;
    expiresIn: number;
    refreshToken: string;
    refreshTokenExpiresIn: number;
}
export function getTokenInfo() {
    return ss.get(LOCAL_NAME)
}

export function setTokenInfo(tokenInfo: TokenInfo) {
    return ss.set(LOCAL_NAME, tokenInfo)
}

export function removeTokenInfo() {
    return ss.remove(LOCAL_NAME)
}