<template>
    <div class="content-top"></div>
    <div class="content">
        <baseTable ref="table" :columns="columns" :listUrl="listUrl" @row-click="handleRowClick" @refreshTable="refresh" />
    </div>
</template>
<script setup lang="jsx">
import $api from "@/api/index";
import { reactive, ref, onMounted } from "vue";
import baseTable from "@/components/table/baseTable.vue";
import moment from "moment";
const listUrl = $api.file.GetFileList;
const columns = [
    { colKey: "userName", title: "上传人", align: "center" },
    {
        colKey: "location",
        title: "查看链接",
        align: "center",
        cell: (h, { row }) => {
            return <a href={window.config.baseFileUrl+row.location} target="_blank">{row.fullName }</a>;
        },
    },
    {
        colKey: "createDate",
        title: "上传时间",
        align: "center",
        cell: (h, { row }) => {
            return moment(row.createDate).format("yyyy/MM/DD HH:mm:ss");
        },
    },
];



let table = ref(null);
onMounted(() => {
    loadTable();
});
const loadTable = () => {
    table.value.getTableList();
};
const handleRowClick = (e) => {
    console.log(e);
};
</script>
