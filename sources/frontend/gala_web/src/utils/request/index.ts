import axios from 'axios'

const service = axios.create({
    baseURL: '',
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

        return Promise.reject(error);
    }
)

service.interceptors.response.use(

)