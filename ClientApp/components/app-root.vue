<template>
    <div id="app" style="width:100%">
        <div style="width:100%">
            <nav class="navbar" style="background-color:#015351;">
                <div class="navbar-brand">
                    <a class="navbar-item">
                        <img src="../Assets/Logo_Green_lq.jpg" style="width:100px; height:140px;">
                        <img src="../Assets/logo2_lq.png" style="width:100px; height:140px;"/>
                    </a>
                </div>
                <div class="navbar-menu">
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
                                <a class="navbar-item" v-for="link in MenuHub":href=link.link>
                                   {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link">
                                Ops Hub
                            </a>
                            <div class="navbar-dropdown">
                                <a class="navbar-item" v-for="link in OpsHub" :href=link.link>
                                    {{link.title}}
                                </a>
                            </div>
                        </div>
                        <div class="navbar-item has-dropdown is-hoverable">
                            <a class="navbar-link" >
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
                            <a class="navbar-link" >
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
                        <span style="color:white; font-size:xx-small;" v-on:click="logout"> Sign Out {{getUser.managerName}} </span>
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
    import HomePage from './home-page'
    import NavMenu from './nav-menu'
    import MonthlyCalendar from './calendar/monthly-calendar'
    import calendar from 'vue2-simple-calendar'
    import dropdown from './layout/dropdown'
    import Vuex from 'vuex'
    import shift from './calendar/TheShift'

    import { mapGetters, mapActions } from 'vuex'
    

    Vue.component('home-page', HomePage);
    Vue.component('nav-menu', NavMenu);
    Vue.component('monthly-calendar', MonthlyCalendar);
    Vue.component('dropdown', dropdown)
    Vue.component('shift', shift)
    Vue.use(Vuex)

    export default {
        data() {
            return {
                routes,
                collapsed: true,
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
                    { title: 'Mgmt Scheduling', link: "http://192.168.0.37:8000/schedule/" },
                    { title: "Gm Page", link: "http://192.168.0.37:8000/GmPage" }

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
            logout() {
                document.cookie = ""
                window.location.href = 'http://192.168.0.37:8001'
            }
        },
        created() {
            this.fetchLoggedInUser()
        }
}
</script>

<style>
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
        font-size:15px
    }
   
</style>
