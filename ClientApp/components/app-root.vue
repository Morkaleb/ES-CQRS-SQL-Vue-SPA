<template>
    <div id="app" style="width:100%">
        <div style="width:100%">
            <nav class="navbar" style="background-color:#015351;">
                <div class="navbar-brand">
                    <a class="navbar-item">
                        <img src="../Assets/Logo_Green_lq.jpg" style="width:100px; height:140px;">
                        <img src="../Assets/logo2_lq.png" style="width:100px; height:140px;" />
                        <div class="navbar-burger" @click="makeBurger" style="color:white;" data-target="navMenu"  v-bind:class="{ 'is-active': activator }">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    </a>
                </div>
                <div class="navbar-menu" id="navMenu" v-bind:class="{ 'is-active': activator }">
                    <div class="navbar-start">
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Supply Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in SupplyHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Menu Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in MenuHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Ops Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in OpsHub">
                                    <router-link :to="{ path: link.link}">{{link.title}}</router-link>
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Finance Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in FinanceHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Admin Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in AdminHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Inventory Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in InventoryHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-dropdown is-boxed">
                        </div>
                    </div>
                    
                    <div class="navbar-end">
                        <button class="btn btn-default" style="color:white; border-color:#015351; background-color: #015351; font-size:xx-small;" v-on:click="logout"> Sign Out {{ getUser.firstName }} {{getUser.lastName}} </button>
                        <img src="../Assets/SmallLogo.png" style="width: 100px; height: 60px; padding: .6em 1em .6em 1em" alt="hub_logo" />
                    </div>
                </div>
            </nav>            
        <div class="main">
            <router-view></router-view>
        </div>
    </div>
 </div>
</template>

<script>
    import { routes } from '../routes'
    import Vue from 'vue'
    import Vuex from 'vuex'

    import HomePage from './home-page'
    import MonthlyCalendar from './Calendar/MonthlyCalendar'
    import calendar from 'vue-fullcalendar'
    import dropdown from './layout/dropdown'
    import shift from './Calendar/TheShift'
    import day from './Calendar/OneDay'
    import scheduleConsequences from './Calendar/ScheduleConsequences'
    import shiftModal from './Calendar/ShiftModal'
    import weeklyCalendar from './Calendar/WeeklyCalendar'
    import dailyShiftRequirements from './ManagerComponents/DailyShiftRequirements'
    import gmPage from './ManagerComponents/GmPage'
    import shiftRequirementsModal from './ManagerComponents/ShiftRequirementsModal'
    import shiftRequirementsWeek from './ManagerComponents/ShiftRequirementsWeek'
    import shiftSelection from './ManagerComponents/ShiftSelection'
    import approveModal from './Scheduling/ApproveModal'
    import dsapproveModal from './Scheduling/DisapproveModal'
    import gMApproveSchedule from './Scheduling/GMApproveSchedule'
    import PayRollApproval from './Scheduling/PayrollApproval'
    import ReviewScheduleChange from './Scheduling/ReviewScheduleChange'
    import schedulingPages from './schedulingPages'
    import datePicker from 'vuejs-datepicker'

    import { mapGetters, mapActions } from 'vuex'
    

    Vue.component('home-page', HomePage);
   // Vue.component('nav-menu', NavMenu);
    Vue.component('monthly-calendar', MonthlyCalendar);
    Vue.component('dropdown', dropdown);
    Vue.component('shift', shift);
    Vue.component('day', day);
    Vue.component('scheduleConsequences', scheduleConsequences);
    Vue.component('shiftModal', shiftModal);
    Vue.component('dailyShiftRequirements', dailyShiftRequirements);
    Vue.component('gmPage', gmPage);
    Vue.component('shiftRequirementsModal', shiftRequirementsModal);
    Vue.component('shiftRequirementsWeek', shiftRequirementsWeek);
    Vue.component('shiftSelection', shiftSelection);
    Vue.component('approveModal', approveModal);
    Vue.component('dsapproveModal', dsapproveModal);
    Vue.component('gMApproveSchedule', gMApproveSchedule);
    Vue.component('PayRollApproval', PayRollApproval);
    Vue.component('ReviewScheduleChange', ReviewScheduleChange);
    Vue.component('weeklyCalendar', weeklyCalendar);
    Vue.component('calendar', calendar);
    Vue.component('schedulingPages', schedulingPages);
    Vue.component('datePicker', datePicker);
    
    Vue.use(Vuex)

    export default {
        data() {
            return {
                routes,
                collapsed: true, msg: '',
                activator: false,
                showNav:false,
                SupplyHub: [
                    { title: 'Home (NEW)', link: "" },
                    { title: 'Products', link: "" },
                    { title: 'SKUs', link: "" },
                    { title: 'Contracts', link: "" },
                    { title: 'Bids', link: "" },
                    { title: 'Mapper', link: "" },
                    { title: 'Admin', link: "" },
                ],
                MenuHub: [
                    { title: 'Products', link: "" },
                    { title: 'Recipes', link: "" },
                    { title: 'Menus', link: "" },
                    { title: 'Menus Items', link: "" }
                ],
                OpsHub: [
                    { title: 'My Pillars', link: "" },
                    { title: 'OOO Grid', link: "" },
                    { title: 'OOO Pivot Data', link: "" },
                    { title: 'Mgmt Change', link: "" },
                    { title: 'Mgmt Scheduling', link: "/ops/schedule" }

                ],
                FinanceHub: [
                    { title: 'PurchaseOrders', link: "" },
                ],
                AdminHub: [
                    { title: 'Mgmt Change', link: "" },
                    { title: 'Alerts', link: "" },
                    { title: 'Measure Units', link: "" },
                    { title: 'Ops Review', link: "" },
                ],
                InventoryHub: [
                    { title: 'Home', link: "" }
                ]
        }
        },
        computed: {
            ...mapGetters([
                'getUser'])
        },
        methods: {
            ...mapActions([
                'fetchLoggedInUser'
            ]),
            makeBurger() {
                this.activator = !this.activator
                return this.activator
            },
            logout() {
                document.cookie = ""
                window.location.href = 'https://wsbis.whitespotonline.com:4443/signIn'
            }
        },
        created() {
            this.fetchLoggedInUser()
        }
}
</script>

<style>
    body {
        background-color: #e9eaed !important;
    }
    .main {
        margin: 0 0 0 0;
        padding:0;
        position:relative;
        background-color: #e9eaed;
        height:100%;
    }
    .app {
        background-color: #e9eaed !important;
    }
    .navbar-link  {
        color:white !important;
    }
    .navbar-link :hover {
        color:black !important;
    }
    .navbar{
        margin:0;
        border-radius:0;
        font-size:13px;
        padding:0;
    }
    .navbar-dropdown{
        padding:0;
        margin:0;
    }
   
</style>
