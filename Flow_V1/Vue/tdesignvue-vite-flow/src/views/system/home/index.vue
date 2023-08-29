<template>
    <div class="content-top">
        <t-form layout="inline">
            <t-form-item label="年份" name="name">
                <t-select v-model="form.year" :onChange="payTypeSelectChange">
                    <t-option v-for="item in yearList" :key="item" :value="item">{{ item }}</t-option>
                </t-select>
            </t-form-item>
            <t-form-item label="月份" name="password">
                <t-select v-model="form.month" :onChange="payTypeSelectChange">
                    <t-option v-for="item in monthList" :key="item" :value="item">{{ item }}</t-option>
                </t-select>
            </t-form-item>
            <t-form-item label="平台" name="password">
                <t-select v-model="form.type" :onChange="payTypeSelectChange">
                    <t-option value="微信">微信</t-option>
                    <t-option value="支付宝">支付宝</t-option>
                </t-select>
            </t-form-item>
        </t-form>
    </div>
    <div class="content">
        <t-space break-line>
            <t-card>
                <chart  chartId="chart1"  :chartUrl="$api.chart.GetTypeChart"  
                chartTitle="消费类型<支出>"   :param="form" ref="chartData" />
            </t-card>
            <t-card>
                <chart  chartId="chart2" :chartUrl="$api.chart.GetInChart"
                 chartTitle="收入信息" :param="form" ref="chart2Data" />
            </t-card>
            <t-card>
                <payType :param="form" ref="payTypeData" />
            </t-card>
        </t-space>
    </div>
</template>
<script setup lang="jsx">
import $api from "@/api/index";
import { reactive, ref, onMounted } from "vue";
import chart from "./components/chart.vue";
import payType from "./components/payType.vue";
let chartData = ref(null);
let chart2Data = ref(null);
let payTypeData = ref(null);
const payTypeSelectChange = () => {
    console.log("payTypeSelectChange.chartData", chartData.value);
    chartData.value.getChart1Data($api.chart.GetTypeChart);
    chart2Data.value.getChart1Data($api.chart.GetInChart);
    payTypeData.value.getPayType();
};
let form = ref({
    year: new Date().getFullYear(),
    month: new Date().getMonth(),
    type: "微信",
});

let yearList = ref([]);
let monthList = ref([]);
const getYear = () => {
    var date = new Date();
    var year = date.getFullYear();
    for (let index = 0; index < 3; index++) {
        yearList.value.push(year - index);
    }
};
getYear();
const getMonth = () => {
    var date = new Date();
    var month = date.getMonth();
    if (month == 12) {
        for (let index = 0; index < 12; index++) {
            monthList.value.push(month - index);
        }
    } else {
        for (let index = 0; index <= month; index++) {
            let val = month - index ;
            monthList.value.push(val);
        }
    }
};
getMonth();
</script>
