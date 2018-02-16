<template>
    <div id="app" class="container-fluid">
        <div class="row">
                <nav-menu params="route: route"></nav-menu>
            </div>
            <div class="col-sm-12">
                <router-view></router-view>
            </div>
        </div>

    </div>

</template>

<script>
    import Vue from 'vue'
    import CounterExample from './counter-example'
    import FetchData from './fetch-data'
    import HomePage from './home-page'
    import NavMenu from './nav-menu'
    import MonthlyCalendar from './calendar/monthly-calendar'
    import calendar from 'vue2-simple-calendar'
    import Buefy from 'buefy'

    Vue.component('counter-example', CounterExample);
    Vue.component('fetch-data', FetchData);
    Vue.component('home-page', HomePage);
    Vue.component('nav-menu', NavMenu);
    Vue.component('monthly-calendar', MonthlyCalendar);
    Vue.component('')
    Vue.use(Buefy)
    export default {

        data() {
            return {
        }
    }
}
</script>

<style>
</style>
