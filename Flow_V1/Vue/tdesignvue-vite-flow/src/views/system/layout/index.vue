<template>
    <div class="layout">
        <t-layout>
            <t-aside>
                <t_side_dide />
            </t-aside>
            <t-layout>
                <t-header>
                    <t_head />
                </t-header>
                <t-content>
                    <t_content_content />
                </t-content>
                <t-footer>
                    <t_foot />
                </t-footer>
            </t-layout>
        </t-layout>
    </div>
</template>
<script>
import t_foot from './component/footer.vue'
import t_head from './component/header.vue'
import t_side_dide from './component/side.vue'
import t_content_content from './component/content.vue'
export default { 
    components:{t_foot,t_head,t_content_content,t_side_dide},
    watch:{
        $router(to,from){
            console.log(to)
            console.log(from)
        }
    }
};
</script>
