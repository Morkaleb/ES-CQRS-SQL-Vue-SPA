<template>
    <div id="app" style="width:100%">
        <div style="width:100%">
            <nav class="navbar" style="background-color:black">
                <div class="navbar-brand">
                    <a class="navbar-item">
                    
                        <div class="navbar-burger" @click="makeBurger" style="color:white;" data-target="navMenu"  v-bind:class="{ 'is-active': activator }">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    </a>
                </div>
                <div class="navbar-menu" id="navMenu" v-bind:class="{ 'is-active': activator }">
                    <div class="navbar-start">                        
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

    import homepage from './homePage'

    import { mapGetters, mapActions } from 'vuex'
    

    Vue.component('home-page', HomePage);
 
    Vue.component('datePicker', datePicker);
    
    Vue.use(Vuex)

    export default {
        data() {
            return {
                routes,
                collapsed: true, msg: '',
                activator: false              
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
                window.location.href = 'localhost:5000'
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
