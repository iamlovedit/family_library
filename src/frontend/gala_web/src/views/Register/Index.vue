<template>
    <site-content>
        <div class="flex flex-1 flex-col justify-center items-center w-1/3 mx-auto h-full">
            <n-card embedded class="p-1 m-6 rounded-lg">
                <n-form :model="formValue" :rules="registerRule" ref="formRef" class="space-y-2"
                    :wrapper-col="{ span: 16 }">
                    <n-form-item path="username" :label="t('login.username')" required>
                        <n-input />
                    </n-form-item>
                    <n-form-item path="password" :label="t('login.password')" required>
                        <n-input type="password" />
                    </n-form-item>
                    <n-form-item path="password" :label="t('login.password')" required>
                        <n-input type="password" />
                    </n-form-item>
                    <n-form-item path="email" label="Email" required>
                        <n-input />
                    </n-form-item>
                    <n-form-item>
                        <n-button type="primary">提交</n-button>
                    </n-form-item>
                </n-form>
            </n-card>
        </div>
    </site-content>
</template>

<script setup lang="ts">
import SiteContent from '@components/Layout/SiteContent/Index.vue';
import { FormValidationError } from 'naive-ui/es/form';
import { ref } from 'vue'
import { FormInst, FormRules, useMessage } from 'naive-ui'
import { useAuthStore } from '@/stores/modules/auth';
import { useRouter, useRoute } from 'vue-router'
import { t } from '@/locales';

const formRef = ref<FormInst | null>(null)
const loading = ref<boolean>(false)
const message = useMessage()
const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()

const registerRule: FormRules = {
    username: { required: true, message: t('login.username_placeholder'), trigger: 'blur' },
    password: { required: true, message: t('login.password_placeholder'), trigger: 'blur' },
};
const formValue = ref({
    username: '',
    password: '',
    email: '',
});

</script>