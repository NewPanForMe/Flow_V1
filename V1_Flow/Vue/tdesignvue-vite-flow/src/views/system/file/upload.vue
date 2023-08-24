<template>
    <t-space>
        <upload @Resp="resp"  :codeStrings="codeStrings" />
    </t-space>
</template>
<script setup>
import { ref, watch } from "vue";
import upload from "@/components/file/upload.vue";
//:theme="theme"   开启多文件上传
const theme=ref("file-flow")
const resp = (param) => {
    console.log(param);
};

let codeStrings=ref([
   
])


</script>
