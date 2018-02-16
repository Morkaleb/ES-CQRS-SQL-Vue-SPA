<template>
    <div class="navbar navbar-inverse navbar-fixed-top gi-sm">
        <div class="container">
            <div class="navbar-header ">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a href="/wsbis" class="pull-left">
                    <img src="../Assets/Logo_Green_lq.jpg" style="width: 100px; height: 60px; padding: .3em .5em .3em .5em" alt="logo" />
                </a>
                <a href="/wsbis" class="pull-left">
                    <img src="../Assets/logo2_lq.png" style="width: 100px; height: 60px;  padding: 1em .5em 1em .5em" alt="logo" />
                </a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav"></ul>
                <section class="section" aria-expanded="true">
                    <div class="dropdown">
                        <span>Supply Hub</span><span class="caret"></span>
                            <div class="dropdown-content">
                                <p>Home (NEW)</p>
                                <hr />
                                <p>Products</p>
                                <p>SKUs</p>
                                <p>Contracts</p>
                                <p>Bids</p>
                                <hr/>
                                <p>Mapper</p>
                                <p>Admin</p>
                          </div>
                        </div>
                </section>
                <section class="section">
                    <div class="dropdown">
                        <span>Menu Hub</span><span class="caret"></span>
                        <div class="dropdown-content">
                            <p>Products</p>
                            <p>Recipes</p>
                            <p>Menus</p>
                            <p>Menu Items</p>
                        </div>
                    </div>
                </section>
                <section class="section">
                    <div class="dropdown">
                        <span>Ops Hub</span><span class="caret"></span>
                        <div class="dropdown-content">
                            <p>My Pillars</p>
                            <hr/>
                            <p>OOO Grid</p>
                            <p>OOO Pivot Data</p>
                            <p>Mgmt Change</p>
                            <p><router-link to="{path: '/schedule/?locationId=' + getUser.locationId}">Schedule</router-link> </p>
                        </div>
                    </div>
                </section>
                <section class="section">
                    <div class="dropdown">
                        <span>Finanace Hub</span><span class="caret"></span>
                        <div class="dropdown-content">
                            <p>Purchase Orders</p>
                        </div>
                    </div>
                </section>
                <section class="section">
                    <div class="dropdown">
                        <span>Admin Hub</span><span class="caret"></span>
                        <div class="dropdown-content">
                            <p>Mgmt Change</p>
                            <p>Mgmt Schedule</p>
                            <p>Measure Units</p>
                            <p>Ops Review</p>
                        </div>
                    </div>
                </section>
                <section class="section">
                    <div class="dropdown">
                        <span>Inventory Hub</span><span class="caret"></span>
                        <div class="dropdown-content">
                            <p>Home</p>
                        </div>
                    </div>
                </section>
                <ul class="nav navbar-nav navbar-right">
                    <li style="width: 100px; height: 60px">
                        <a href="/wsbis" class="pull-right" style="padding:0; ">
                            <img src="../Assets/SmallLogo.png" style="width: 100px; height: 60px; padding: .6em 1em .6em 1em" alt="hub_logo" />
                        </a>
                    </li>
                </ul>

            </div>
        </div>
    </div>

</template>

<script>
    import { routes } from '../routes'
    import {mapGetters, mapActions} from 'vuex'
 
    export default {
        data() {
            return {
                routes,
                collapsed: true
            }
        },
        methods: {
            toggleCollapsed: function (event) {
                this.collapsed = !this.collapsed;
            }
        },
        computed: {
            ...mapGetters([
             'getUser'
            ])
        }
    }
</script>

<style>
    .slide-enter-active, .slide-leave-active {
        transition: max-height .35s
    }

    .slide-enter, .slide-leave-to {
        max-height: 0px;
    }

    .slide-enter-to, .slide-leave {
        max-height: 20em;
    }

    .logo1 {
        margin-top: 3px;
        height: 50px;
    }
    .dropdown {
        color:white;
        position: relative;
        display: inline-block;
        margin-left: 2%;
        margin-top:2%;
        float:left;
        font-size:smaller;
        height:100%;
    }

    .dropdown-content {
        display: none;
        position: absolute;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);      
        z-index: 1;
        color:black;
        margin-top:24%;
    }
    .dropdown:hover {
        background-color: #15a589;
    }
    .dropdown:hover .dropdown-content {
        display: block;
    }
</style>
