<template>
    <site-content>
        <div class="h-full flex flex-nowrap flex-1 justify-center items-center">
            <n-form ref="formRef" label-placement="left" label-width="auto" :rules="loginRules"
                require-mark-placement="right-hanging">
                <n-form-item path="username" :label="t('login.username')" required>
                    <n-input type="text" v-model:value="formValue.username" :placeholder="t('login.username_placeholder')"
                        autoComplete="new-password" />
                </n-form-item>
                <n-form-item path="password" :label="t('login.password')" required>
                    <n-input type="password" show-password-on="mousedown" v-model:value="formValue.password"
                        :placeholder="t('login.password_placeholder')" autoComplete="new-password" />
                </n-form-item>
                <n-form-item>
                    <n-button :enabled="loading" type="primary" @click="handleLoginClick">

                    </n-button>
                </n-form-item>
            </n-form>
        </div>
    </site-content>
</template>

<script setup lang="ts">
import SiteContent from '@components/Layout/SiteContent/Index.vue';
import { ref } from 'vue'
import { FormInst, FormItemRule, useMessage } from 'naive-ui'
import { t } from '@/locales';

const formRef = ref<FormInst | null>(null)
const loading = ref<boolean>(false)
const message = useMessage()
const loginRules = {
    username: { required: true, message: t('login.username_placeholder'), trigger: 'blur' },
    password: { required: true, message: t('login.password_placeholder'), trigger: 'blur' },
};
const formValue = ref({
    username: '',
    password: '',
});

function handleLoginClick(e: MouseEvent) {
    e.preventDefault()
    const messageReactive = message.loading(t('login.logging'));
    formRef.value?.validate((errors) => {
        if (!errors) {
            console.log('验证通过')
        } else {
            message.error('');
        }
        messageReactive.destroy();
    })
}
</script>
