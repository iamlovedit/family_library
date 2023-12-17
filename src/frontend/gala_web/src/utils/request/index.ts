import axios from 'axios'
import { useRouter } from 'vue-router'
import { useMessage } from 'naive-ui'

export type HttpResponse<T> = {
    statusCode: number;
    message: string;
    response: T,
    succeed: boolean
}

const router = useRouter();
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
    config => {
        if (config.method == 'post') {
            config.data = JSON.stringify(config.data)
        }
        return config;
    },
    error => {
        message.error(error);
        return Promise.reject(error);
    }
)

service.interceptors.response.use(
    response => {
        if (response.status === 200) {
            return Promise.resolve(response);
        } else {
            return Promise.reject(response);
        }
    },
    error => {
        if (error.response.status) {
            switch (error.response.status) {
                case 401:
                    router.replace({
                        path: '/login',
                        query: {
                            redirect: router.currentRoute.value.fullPath
                        }
                    });
                    break;
                case 403:
                    message.info("token expired", {
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
                    message.info("404", {
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
                    message.info(error.response.data.message, {
                        duration: 1500
                    })
            }
            return Promise.reject(error.response);
        }
    }
)

export default service