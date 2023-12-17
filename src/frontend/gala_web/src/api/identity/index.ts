import service from "@/utils/request";

export type LoginData = {
    username: string
    password: string
}

function login(data: LoginData) {
    return service.post('/identity/v1/auth/login', data,)
}



export {
    login
}
