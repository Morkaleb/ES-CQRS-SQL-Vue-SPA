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
                    <img src="../Assets/Logo_Green_lq.jpg" style="width: 100px; height: 60px; padding: .3em .5em .3em .5em" alt="logo"/>
                </a>
                <a href="/wsbis" class="pull-left">
                    <img src="../Assets/logo2_lq.png" style="width: 100px; height: 60px;  padding: 1em .5em 1em .5em" alt="logo" />
                </a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                </ul>

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

export default {
    data() {
        return {
            routes,
            collapsed : true
        }
    },
    methods: {
        toggleCollapsed: function(event){
            this.collapsed = !this.collapsed;
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

.logo1{
  margin-top:3px;
  height : 50px;
}

</style>
