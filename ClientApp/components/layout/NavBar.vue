<template>
  <!--<section class="hero app-navbar is-bold">
    <div class="hero-head">-->
      <header class="navbar is-primary">
        <img src="../../assets/Logo_Green_lq.jpg"/>
        <img src="../../assets/logo2_lq.png"/>
        <div class="container is-primary">
        <div class="navbar-brand">
          <a class="navbar-item is-hidden-desktop" href="https://github.com/ndro/vue-webpack-buefy" target="_blank">
            <b-icon pack="fa" icon="github" type="is-black"></b-icon>
          </a>
          <div class="navbar-burger burger" data-target="navbarDropdown">
          </div>
        </div>
          <div class="navbar-start">
        <div id="SupplyHub" class="navbar-menu">
            <div class="navbar-item has-dropdown is-hoverable">
              <a class="navbar-link" href="#">
                Supply Hub
              </a>
              <div class="navbar-dropdown is-boxed">
                <a class="navbar-item" href="#">
                  Home
                </a>
                <hr class="navbar-divider">
                <a class="navbar-item" href="#">
                  Products
                </a>
                <a class="navbar-item" href="#">
                  SKUs
                </a>
                <a class="navbar-item" href="#">
                  Contracts
                </a>
                <a class="navbar-item" href="#">
                  Bids
                </a>
                <hr class="navbar-divider">
                <a class="navbar-item" href="#">
                  Mapper
                </a>
                <a class="navbar-item" href="#">
                Admin
                </a>
              </div>
            </div>

        </div>
          <div id="MenuHub" class="navbar-menu">
            <div class="navbar-start">
              <div class="navbar-item has-dropdown is-hoverable">
                <a class="navbar-link" href="#">
                  Menu Hub
                </a>
                <div class="navbar-dropdown is-boxed">
                  <a class="navbar-item" href="#">
                    Products
                  </a>
                  <a class="navbar-item" href="#">
                    Recipes
                  </a>
                  <a class="navbar-item" href="#">
                    Menus
                  </a>
                  <a class="navbar-item" href="#">
                    Menu Items
                  </a>
                </div>
              </div>
            </div>
          </div>
            <div id="OpsHub" class="navbar-menu">
              <div class="navbar-start">
                <div class="navbar-item has-dropdown is-hoverable">
                  <a class="navbar-link" href="#">
                    Ops Hub
                  </a>
                  <div class="navbar-dropdown is-boxed">
                    <a class="navbar-item" href="#">
                      My Pillars
                    </a>
                    <hr class="navbar-divider">
                    <a class="navbar-item" href="#">
                      OOO Grid
                    </a>
                    <a class="navbar-item" href="#">
                      OOO Pivot Data
                    </a>
                    <a class="navbar-item" href="#">
                      Mgmt Change
                    </a>
                    <div>
                      <router-link to='/schedule'>Mgmt Scheduling</router-link></div>
                    <div>
                     <router-link to='/gmtools'>Restaurant settings </router-link>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div id="FinanceHub" class="navbar-menu">
            <div class="navbar-start">
              <div class="navbar-item has-dropdown is-hoverable">
                <a class="navbar-link" href="#">
                  Finance Hub
                </a>
                <div class="navbar-dropdown is-boxed">
                  <a class="navbar-item" href="#">
                    Purchase Orders
                  </a>
                </div>
              </div>
            </div>
          </div>
            <div id="AdminHub" class="navbar-menu">
              <div class="navbar-start">
                <div class="navbar-item has-dropdown is-hoverable">
                  <a class="navbar-link" href="#">
                    Admin Hub
                  </a>
                  <div class="navbar-dropdown is-boxed">
                    <a class="navbar-item" href="#">
                      Mgmt Change
                    </a>
                    <a class="navbar-item" href="#">
                      Alerts
                    </a>
                    <a class="navbar-item" href="#">
                      Ops Review
                    </a>
                  </div>
                </div>
              </div>
            </div>
            <div id="Inventory Hub" class="navbar-menu">
              <div class="navbar-start">
                <div class="navbar-item has-dropdown is-hoverable">
                  <a class="navbar-link" href="#">
                    Inventory Hub
                  </a>
                  <div class="navbar-dropdown is-boxed">
                    <a class="navbar-item" href="#">
                      Home
                    </a>
                    <a class="navbar-item" href="#">
                      Old Version
                    </a>
                  </div>
                </div>
              </div>
            </div>
            <div class="navbar-end">
            <!--<div class="navbar-item">-->
             <img src="../../assets/SmallLogo.png"/>
            <!--</div>-->
            <div class="navbar-item">
              <div class="field is-grouped">
                <p class="control">
                  <a class="button is-primary is-outlined" href="#">
                    <span>Button</span>
                  </a>
                </p>
              </div>
            </div>
          </div>
        </div>
        </div>
      </header>
    <!--</div>
  </section>-->
</template>

<script>
export default {
  methods: {
    ChangeActive () {
      this.$emit('set-active-component', 'MonthlyCalender')
    }
  }
}
/* burger navigation */
document.addEventListener('DOMContentLoaded', function () {
  // Get all "navbar-burger" elements
  var $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0)
  // Check if there are any navbar burgers
  if ($navbarBurgers.length > 0) {
    // Add a click event on each of them
    $navbarBurgers.forEach(function ($el) {
      $el.addEventListener('click', function () {
        // Get the target from the "data-target" attribute
        var target = $el.dataset.target
        var $target = document.getElementById(target)
        // Toggle the class on both the "navbar-burger" and the "navbar-menu"
        $el.classList.toggle('is-active')
        $target.classList.toggle('is-active')
      })
    })
  }
})
</script>

<style lang="scss" scoped>
.navbar {
  color: #015351;
  height: 80px;
  padding: 8px 10px;
},
.navbar-menu {
   color: #015351;
}
</style>

