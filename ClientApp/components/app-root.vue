<template>
    <div id="app" class="container" style="width:100%">
        <div style="width:100%">
            <nav-menu params="route: route"></nav-menu>
        <div class="main">
            <router-view></router-view>
        </div>
    </div>
 </div>

</template>

<script>
    import Vue from 'vue'
    import HomePage from './home-page'
    import NavMenu from './nav-menu'
    import MonthlyCalendar from './calendar/monthly-calendar'
    import calendar from 'vue2-simple-calendar'
    import Buefy from 'buefy'
    import dropdown from './layout/dropdown'
    import Vuex from 'vuex'
    import shift from './calendar/shift'
    
    Vue.component('home-page', HomePage);
    Vue.component('nav-menu', NavMenu);
    Vue.component('monthly-calendar', MonthlyCalendar);
    Vue.component('dropdown', dropdown)
    Vue.component('shift', shift)
    Vue.component('')
    Vue.use(Buefy)
    Vue.use(Vuex)
    export default {

        data() {
            return {
        }
    }
}
</script>

<style>
    .main{
        margin:6% 0 0 0;
        padding:0;
    }
</style>
