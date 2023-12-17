import 'vue-router'

declare type Role = 'admin' | 'user'

declare module 'vue-router' {
    interface RouteMate {
        requiresAuth: boolean;
        roles?: Role[]
        ignoreCache: boolean;
    }
}
