<template>
  <section class="card ">
      <div class="card-content container">
          <strong>Things To Consider</strong><hr />
          <div class="ConsiderationCard">
              <strong style="margin-left:15%; margin-bottom:5px;">Shift break down for the week</strong>
             <p v-for="manager in getManagerDays" v-if="manager.name != 'Cancel Shift'">
                     {{manager.name}}
                     <span v-if="manager.shifts !=5" style="color:red">has {{manager.shifts}} shifts</span>
                     <span v-if="manager.shifts === 5">has 5 shifts</span>
                     <span v-if="manager.daysToOwe != 0"> Will be owed {{manager.daysToOwe}} shift</span>
              </p>
          </div>
          <div class="ConsiderationCard"></div>
          <div class="ConsiderationCard">
          <div v-for="history in getVacationHistory">
              <p>{{history.managerName}} has {{history.statHolidaysOwed + history.vacationOwed}} days owed</p>
              </div>
          </div>
      </div>
  </section>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'

    export default {
        name: 'schedule-consequences',
        props: ['consequences'],
        data() {
            return {
                managerDays: [],
                schedule: []
            }
        },
        methods: {
            ...mapActions([
                'fetchVacationHistory'
            ])
        },
        computed: {
            ...mapGetters([
                'getWeek',
                'getManagerDays',
                'getUser',
                'getVacationHistory'
            ])
        },
        created() {
            this.fetchVacationHistory(this.$store.state.loggedInUser.locationId)
        }
    }
</script>

<style scoped>
  .card {
    border-radius: 5px;
    border: 1px solid darkgray;
    height: 500px;
    width:80%
  }
  strong {
    text-align: right;
  }
    .ConsiderationCard {
        border-radius: 5px;
        padding: 2px;
        margin-left: 5px;
        float: left;
        text-align: left;
        border: 1px solid black;
        height:80%;
        width:28%;
    }
</style>
