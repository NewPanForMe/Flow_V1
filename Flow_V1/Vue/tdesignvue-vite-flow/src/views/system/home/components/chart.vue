<template>
    <div :id="prop.chartId" style="height: 400px; width: 480px"></div>
</template>
<script setup>
import * as echarts from "echarts";
import $instance from "@/utils/http";

import { reactive, ref, onMounted } from "vue";
let chart1Data = ref([]);
const initChart1 = (id) => {
    let newPromise = new Promise((resolve) => {
        resolve();
    });
    //然后异步执行echarts的初始化函数
    newPromise.then(() => {
        //	此dom为echarts图标展示dom
        let chart = echarts.init(document.getElementById(id));
        var option = {
            title: {
                text: prop.chartTitle,
            },
            tooltip: {
                trigger: "item",
            },
            legend: {
                orient: "horizontal",
                type: "scroll",
                top: "7%", //与上方的距离 可百分比% 可像素px
            },

            series: [
                {
                    type: "pie",
                    radius: "50%",
                    data: chart1Data.value,
                    label: {
                        formatter: " {b|{b}：}{c}  {per|{d}%}  ",
                        backgroundColor: "#F6F8FC",
                        borderColor: "#8C8D8E",
                        borderWidth: 1,
                        borderRadius: 4,
                        rich: {
                            a: {
                                color: "#6E7079",
                                lineHeight: 22,
                                align: "center",
                            },
                            hr: {
                                borderColor: "#8C8D8E",
                                width: "100%",
                                borderWidth: 1,
                                height: 0,
                            },
                            b: {
                                color: "#4C5058",
                                fontSize: 14,
                                fontWeight: "bold",
                                lineHeight: 33,
                            },
                     
                        },
                    },
                    emphasis: {
                        itemStyle: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: "rgba(0, 0, 0, 0.5)",
                        },
                    },
                },
            ],
        };
        chart.setOption(option);
    });
};
const getChart1Data = (url) => {
    console.log("getChart1Data.prop", prop);
    $instance.get(url, { params: prop.param }).then((resp) => {
        console.log(resp);
        if (resp.success) {
            chart1Data.value = resp.result.data;
            initChart1(prop.chartId);
        }
    });
};
getChart1Data(prop.chartUrl);
//暴露属性
defineExpose({
    getChart1Data,
});
var prop = defineProps({
    param: {},
    chartUrl: "",
    chartTitle: "",
    chartId: "",
});
</script>
