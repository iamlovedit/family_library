<template>
    <site-content>
        <div class="flex flex-1 flex-col justify-center items-center w-1/3 mx-auto h-full">
            <n-card embedded class="p-1 m-6 rounded-lg">
                <n-form ref="formRef" class="space-y-2" :wrapper-col="{ span: 16 }" :model="formValue" :show-label="false"
                    label-placement="left" label-width="auto" :rules="loginRules" require-mark-placement="right-hanging">
                    <n-form-item path="username" required>
                        <n-input type="text" v-model:value="formValue.username" clearable
                            :placeholder="t('login.username_placeholder')" autocomplete="off" :input-props="{
                                autoComplete: 'username',
                            }">
                            <template #prefix>
                                <n-icon :size="22">
                                    <PersonFilled />
                                </n-icon>
                            </template>
                        </n-input>
                    </n-form-item>
                    <n-form-item path="password" :label="t('login.password')" required>
                        <n-input type="password" show-password-on="click" v-model:value="formValue.password"
                            :placeholder="t('login.password_placeholder')" autoComplete="new-password" :input-props="{
                                autoComplete: 'current-password',
                            }">
                            <template #prefix>
                                <n-icon :size="22">
                                    <LockFilled />
                                </n-icon>
                            </template>
                        </n-input>
                    </n-form-item>
                </n-form>
            </n-card>
            <div class="flex justify-center mt-3 mb-5  items-center w-full gap-3">
                <n-button class="flex-1" @click="handleRegisterClick">
                    {{ t('login.register') }}
                </n-button>
                <n-button class="flex-1" :enabled="loading" type="primary" @click="handleLoginClick">
                    {{ t('header.login') }}
                </n-button>
            </div>
        </div>

    </site-content>
</template>

<script setup lang="ts">
import SiteContent from '@components/Layout/SiteContent/Index.vue';
import { FormValidationError } from 'naive-ui/es/form';
import { LockFilled, PersonFilled } from '@vicons/material'
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { FormInst, FormRules, useMessage } from 'naive-ui'
import { t } from '@/locales';
import { useAuthStore } from '@/stores/modules/auth';
import { login } from "@/api/identity";

const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()
const formRef = ref<FormInst | null>(null)
const loading = ref<boolean>(false)
const message = useMessage()
const loginRules: FormRules = {
    username: { required: true, message: t('login.username_placeholder'), trigger: 'blur' },
    password: { required: true, message: t('login.password_placeholder'), trigger: 'blur' },
};
const formValue = ref({
    username: '',
    password: '',
});

function handleRegisterClick(e: MouseEvent) {
    e.preventDefault()
    router.push({
        name: 'register',
        query: {
            redirect: route.query.redirect
        }
    })
}

function handleLoginClick(e: MouseEvent) {
    e.preventDefault()
    const messageReactive = message.loading(t('login.logining'));
    formRef.value?.validate((errors?: Array<FormValidationError>) => {
        if (!errors) {
            loading.value = true;
        }
    }).then(async () => {
        try {
            const res = await login(formValue.value);
            if (res) {
                const httpResponse = res.data;
                if (httpResponse.succeed) {
                    authStore.updateToken(httpResponse.response)
                    if (route.query.redirect) {
                        router.push(route.query.redirect as string)
                    } else {
                        router.push('/')
                    }
                }
                else {
                    message.error(t('error.password'));
                }
            }
        } catch (error: any) {
            message.error(t('error.server'));
        } finally {
            loading.value = false;
            messageReactive.destroy();
        }
    })
}
</script>
