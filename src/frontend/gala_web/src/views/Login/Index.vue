<template>
    <site-content>
        <div class="flex flex-1  flex-col justify-center items-center w-2/5 mx-auto h-full">
            <n-card embedded class="p-6 m-6 rounded-lg">
                <n-form ref="formRef" class="space-y-2" :wrapper-col="{ span: 16 }" :model="formValue" :show-label="false"
                    label-placement="left" label-width="auto" :rules="loginRules" require-mark-placement="right-hanging">
                    <n-form-item path="username" required>
                        <n-input type="text" v-model:value="formValue.username"
                            :placeholder="t('login.username_placeholder')" autocomplete="off" :input-props="{
                                autoComplete: 'username',
                            }">
                            <template #prefix>
                                <n-icon>
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
                                <n-icon>
                                    <LockFilled />
                                </n-icon>
                            </template>
                        </n-input>
                    </n-form-item>
                </n-form>
            </n-card>
            <div class="flex justify-end mt-3 mb-5  items-center w-full">
                <n-checkbox>
                    {{ t('login.remeber') }}
                </n-checkbox>
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
import { FormInst, FormRules, useMessage } from 'naive-ui'
import { t } from '@/locales';

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

function handleLoginClick(e: MouseEvent) {
    e.preventDefault()
    const messageReactive = message.loading(t('login.logining'));
    formRef.value?.validate((errors?: Array<FormValidationError>) => {
        if (!errors) {
            loading.value = true;
        } else {
            message.error("验证失败");
        }
        messageReactive.destroy();
    }).then(async () => {

    })
}
</script>
