<template>
    <t-space direction="vertical">
        <!-- allow-upload-duplicate-file: false  不允许上传名称相同的文件 -->
        <!-- autoUpload: false  是否在选择文件后自动发起请求上传文件 -->
        <t-upload ref="uploadRef1" method="post" v-model="files1" :action="action" :headers="header" :multiple="multiple"
            :auto-upload="autoUpload" :response="response" :size-limit="{ size: 300, unit: 'MB', message: '图片大小不超过300MB' }"
            :allow-upload-duplicate-file="false" @select-change="handleSelectChange" @fail="handleFail"
            @success="handleSuccess" @validate="onValidate" :before-upload="beforeUpload" :theme="theme"
            showUploadProgress :accept="accept"
            :codeStrings="codeStrings" />
        <table :style="style"   >
            <thead>
                <tr>
                    <td>文件名称</td>
                    <td>操作</td>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in fileList">
                    <td>
                        <a :href="url(item.location)" target="_blank"> {{ item.fullName }}</a>
                    </td>
                    <td>
                        <t-popconfirm content="确认删除吗" @confirm="deleteFile(item.code)">
                            <t-button variant="text" theme="danger" ghost>删除</t-button>
                        </t-popconfirm>
                    </td>
                </tr>
            </tbody>
        </table>
    </t-space>
</template>
<script setup lang="jsx">
import { ref, watch } from "vue";
import $api from "@/api/index";
import { MessagePlugin } from "tdesign-vue-next";
import $cookies from "@/utils/cookies";
import $instance from "@/utils/http";
const uploadRef1 = ref();
const files1 = ref([]);
const multiple = ref(false);
const autoUpload = ref(true);
const emit = defineEmits(["Resp"]);
let fileList = ref([]);
let action = ref($api.file.FileUpload);
let header = ref({});
let response = ref(null);
const prop = defineProps({
    codeStrings: { type: Array, default: () => [] },
    theme: ref("single-input"),
    tableDisplay:ref(false),
    accept:ref("")
});
const handleFail = ({ file }) => {
    MessagePlugin.error(`文件 ${file.name} 上传失败`);
    return;
};


let style=ref("")
const changeTableStyle=()=>{
    if(prop.tableDisplay){
        style.value="min-width: 498px; max-width: 960px; text-align: center;"
    }
    else{
        style.value="min-width: 498px; max-width: 960px; text-align: center;display:none"
    }
}
changeTableStyle();
const handleSelectChange = (files) => {
    console.log("onSelectChange", files);
};

const handleSuccess = (params) => {
    let res = params.response;
    emit("Resp", res);
    if (res.success) {
        MessagePlugin.success("上传成功");
        return;
    } else {
        MessagePlugin.warning(res.result);
        return;
    }
};

const onValidate = (params) => {
    const { files, type } = params;
    console.log("onValidate", type, files);
    const messageMap = {
        FILE_OVER_SIZE_LIMIT: "文件大小超出限制，已自动过滤",
        FILES_OVER_LENGTH_LIMIT: "文件数量超出限制，仅上传未超出数量的文件",
        FILTER_FILE_SAME_NAME: "不允许上传同名文件",
        BEFORE_ALL_FILES_UPLOAD: "beforeAllFilesUpload 方法拦截了文件",
        CUSTOM_BEFORE_UPLOAD: "beforeUpload 方法拦截了文件",
    };
    // you can also set Upload.tips and Upload.status to show warning message.
    messageMap[type] && MessagePlugin.warning(messageMap[type]);
};

const beforeUpload = (file) => {
    var token = $cookies.getToken();
    header.value = {
        Authorization: "Bearer " + token.tokenName,
        JwtVersion: token.jwtVersion,
        RefreshToken: token.refreshToken,
    };
};

const dealFileCode = () => {
    $instance.post($api.file.GetHasUploadList, { codes: prop.codeStrings.join() }).then((resp) => {
        fileList.value = resp.result.data;
    });
};
dealFileCode();
const deleteFile = (code) => {
    $instance.post($api.file.Delete, { code: code }).then((resp) => {
        if (resp.success) {
            MessagePlugin.success("成功");
            dealFileCode();
        }
    });
}

const url = (location) => {
    return window.config.baseFileUrl + location;
}



</script>
