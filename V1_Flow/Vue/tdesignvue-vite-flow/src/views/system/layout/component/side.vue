<template>
    <t-menu    :collapsed="collapsed">
        <template v-for="item in menu">
            <!-- <t-menu-item value="home" to="/home">
                <template #icon>
                    <t-icon name="desktop" />
                </template>
                首页
            </t-menu-item> -->
            <template v-if="item.child.length == 0">
                <t-menu-item value="{{ item.value }}" :to="item.path" :content="item.meta.title" >
                    <template #icon>
                        <t-icon :name="item.icon" />
                    </template>
                </t-menu-item>
            </template>
            <template v-if="item.child.length > 0">
                <t-submenu :value="item.value" :title="item.meta.title">
                    <template #icon>
                        <t-icon :name="item.icon" />
                    </template>
                    <template v-for="itemChild in item.child">
                        <t-menu-item :value="itemChild.value" :to="itemChild.path">
                            <template #icon>
                                <t-icon :name="itemChild.icon" />
                            </template>
                            <span>{{ itemChild.meta.title }}</span>
                        </t-menu-item>
                    </template>
                </t-submenu>
            </template>
        </template>
        <template #operations>
            <t-button class="t-demo-collapse-btn" variant="text" shape="square" @click="changeCollapsed">
                <template #icon><t-icon name="view-list" /></template>
            </t-button>
        </template>
    </t-menu>
</template>
<script setup>
import { ref, defineEmits } from "vue";
import $instance from "@/utils/http";
import $api from "@/api/index";
const menu = ref([]);
const GetMenu = () => {
    $instance.get($api.menu.GetMenu).then((resp) => {
        menu.value = resp.result.menuNode;
    });
};
GetMenu();
const collapsed = ref(false);

const changeCollapsed = () => {
    collapsed.value = !collapsed.value;
};
</script>
