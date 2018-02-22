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
            <div>
                <dropdown title="Supply Hub" :items="SupplyHub"></dropdown>
                <dropdown title="Menu Hub" :items="MenuHub"></dropdown>
                <dropdown title="Ops Hub" :items="OpsHub"></dropdown>
                <dropdown title="Finance Hub" :items="FinanceHub"></dropdown>
                <dropdown title="Admin Hub" :items="AdminHub"></dropdown>
                <dropdown title="Inventory Hub" :items="InventoryHub"></dropdown>
            </div>
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
    import { mapGetters, mapActions } from 'vuex'
    import dropdown from './layout/dropdown'

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
                    { title: 'Mgmt Scheduling', link: "http://localhost:8000/schedule/"},
                    {title: "Gm Page", link: "http://localhost:8000/GmPage"}

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
        watch: {
            
        },
        methods: {
            ...mapActions([
                'fetchLoggedInUser'
            ]),
            toggleCollapsed: function (event) {
                this.collapsed = !this.collapsed;
            }
        },
        computed: {
            ...mapGetters([
             'getUser'
            ]),
            locationId: {
                get: function () {
                    return this.location
                },
                set: function (newValue){
                    this.location = newValue
                }
            }
        },
        components: {
            dropdown
        },
        created() {            
            if (!this.$store.state.loggedInUser.locationId) {
                this.fetchLoggedInUser()
                    .then(() => {
                        this.getUser
                    })
            }
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
    }
    .dropdown:hover {
        background-color: #15a589;
    }
    .dropdown:hover .dropdown-content {
        display: block;
    }
</style>
