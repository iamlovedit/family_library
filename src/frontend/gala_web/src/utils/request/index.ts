import axios from 'axios'
import { useRouter } from 'vue-router'
import { useMessage } from 'naive-ui'
import { useAuthStore } from "@/stores/modules/auth";

export type HttpResponse<T> = {
    statusCode: number;
    message: string;
    response: T,
    succeed: boolean
}

const router = useRouter();
const authStore = useAuthStore();
const message = useMessage();

const service = axios.create({
    baseURL: import.meta.env.VITE_BASE_URL,
    timeout: 5000,
    responseType: 'json',
    headers: {
        "Content-Type": "application/json;charset=utf-8"
    }
})

service.interceptors.request.use(
    function (config) {
        const requireAuth: boolean = (config.headers || {}).requireAuth || false;
        const token = authStore.tokenState?.token;
        if (token && requireAuth) {
            config.headers.Authorization = `Bearer ${token}`
        }
        if (config.method == 'post') {
            config.data = JSON.stringify(config.data)
        }
        return config;
    },
    function (error) {
        message.error(error);
        return Promise.reject(error);
    }
)

service.interceptors.response.use(
    function (response) {
        if (response.status === 200) {
            return Promise.resolve(response);
        } else {
            return Promise.reject(response);
        }
    },
    function (error) {
        const response = error.response;
        const statusCode: number = response.status || 200;
        const errorMessage: string = `${response.data?.error?.message}`
        switch (statusCode) {
            case 401:
                router.replace({
                    path: '/login',
                    query: {
                        redirect: router.currentRoute.value.fullPath
                    }
                });
                break;
            case 403:
                message.info(errorMessage, {
                    duration: 1000
                })
                setTimeout(() => {
                    router.replace({
                        path: '/403',
                        query: {
                            redirect: router.currentRoute.value.fullPath
                        }
                    });
                }, 1000);
                break;
            case 404:
                message.info(errorMessage, {
                    duration: 1500
                })
                setTimeout(() => {
                    router.replace({
                        path: '/404',
                        query: {
                            redirect: router.currentRoute.value.fullPath
                        }
                    });
                }, 1500);
                break;
            default:
                message.info(errorMessage, {
                    duration: 1500
                })
        }
        return Promise.reject(error.response);
    }
)

export default service