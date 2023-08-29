<template>
    <div>
        <t-space direction="vertical">
     
            <table style="width: 5%; text-align: center; border-style: solid; border-width: 1px; border-radius: 10px">
                <thead>
                    <tr>
                        <td>类型</td>
                        <td>金额</td>
                    </tr>
                </thead>
                <tbody>
                    <template v-for="item in payTypeList">
                        <tr>
                            <td>
                                <t-button theme="default" variant="text">{{ item.type }}</t-button>
                            </td>
                            <td>
                                <t-button theme="default" variant="text">{{ item.money }}</t-button>
                            </td>
                        </tr>
                    </template>
                </tbody>
            </table>
        </t-space>
    </div>
</template>
<script setup>
import $instance from "@/utils/http";
import $api from "@/api/index";
import { reactive, ref, onMounted } from "vue";

let payTypeList = ref([]);

const getPayType = () => {
    $instance.get($api.chart.GetPayTypeChart,{params:prop.param}).then((resp) => {
        payTypeList.value = resp.result.data;
    });
};
getPayType();
//暴露属性
defineExpose({
    getPayType,
});
var prop = defineProps({
    param: {},
});
</script>
