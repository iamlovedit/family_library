<template>
    <site-content>
        <div class="h-full flex flex-nowrap flex-1 justify-center items-center">
            <n-form ref="formRef" label-placement="left" label-width="auto" require-mark-placement="right-hanging">
                <n-form-item path="username" :label="t('login.username')" required>
                    <n-input type="text" :placeholder="t('login.username_placeholder')" />
                </n-form-item>
                <n-form-item path="password" :label="t('login.password')" required>
                    <n-input type="password" show-password-on="mousedown" />
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

const message = useMessage()
</script>
