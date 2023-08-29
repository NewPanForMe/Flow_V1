<template>
    <div class="header">
        <div class="header-left">
            <div class="header-left-logout">
                <t-space>
                    <t-dropdown trigger="click" @click="clickHandler" :options="options">
                    <t-button> {{ $cookies.getToken().userName }}</t-button>
                </t-dropdown>
                <t-button shape="circle" theme="primary" variant="outline" @click="onClick">
                    <template #icon><t-icon name="logout" /></template>
                </t-button>
        </t-space>
            
            </div>
        </div>
    </div>
    <t-dialog v-model:visible="visible" :confirm-on-enter="true" :on-confirm="onConfirmAnother">
        <h2><t-icon name="info-circle" style="color: red" /> 确定要退出吗</h2>
    </t-dialog>
</template>
<script setup>
import { ref } from "vue";
import $cookies from "@/utils/cookies";
import $router from "@/router/router";
import { MessagePlugin } from 'tdesign-vue-next';
const visible = ref(false);
const onClick = (context) => {
    visible.value = true;
};
const onConfirmAnother = (context) => {
    visible.value = false;
    $cookies.removeToken();
    $cookies.removeRefreshToken();
    $router.push({ name: "login", replace: true });
};

const options = [
    { content: "个人信息", value: "/user_info" },
    { content: "绑定手机号", value: "/user_bind_phone" },
    { content: "修改密码", value: "/user_password" },
];
const clickHandler = (data) => {
    $router.push({path:data.value})
};
</script>
