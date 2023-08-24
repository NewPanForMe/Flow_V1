<template>
    <t-table
        row-key="index"
        :data="list.data"
        :columns="columns"
        bordered
        hover
        table-layout="fixed"
        size="small"
        :pagination="list.pagination"
        cell-empty-content="-"
        resizable
        @row-click="handleRowClick"
        :loading="loading"
    >
    </t-table>
</template>
<script setup>
import $instance from "@/utils/http";
import { ref, watchEffect, onMounted, watchPostEffect, watch } from "vue";
let list = ref({
    pagination: { defaultCurrent: 1, defaultPageSize: 5, total: 0 },
    data: [],
});

const loading = ref(false);
const paramReload = ref({});
const emit = defineEmits(["rowClick"]);

const handleRowClick = (e) => {
    emit("rowClick", e);
};

var prop = defineProps({
    columns: { type: Array, default: () => [] },
    listUrl: String,
    param: {},
});

watch(
    () => prop.param,
    (newVal, oldVal) => {
        console.log("watch.newVal", newVal);
        console.log("watch.oldVal", oldVal);
        if (newVal !== null) {
            paramReload.value = newVal;
            getTableList();
        }
        console.log("watch.paramReload", paramReload.value);
    }
);
const getTableList = () => {
    loading.value = !loading.value;
    console.log("getTableList.paramReload", paramReload.value);
    $instance.get(prop.listUrl, { params: paramReload.value }).then((resp) => {
        loading.value = !loading.value;
        list = ref({
            pagination: { defaultCurrent: 1, defaultPageSize: 5, total: 0 },
            data: [],
        });
        list.value.data = resp.result.data;
        list.value.pagination = resp.result.pagination;
    });
};
//暴露属性
defineExpose({
    getTableList,
});
</script>
