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
          <div class="ConsiderationCard">
              <strong style="margin-left:14%; margin-bottom:5px;">Make sure that these shifts are covered:</strong>
              <div v-for="day in Object.keys(getWeeklyRequirements)" v-if="day != 'id'" > {{day}}:
                  <span v-for="shift in getWeeklyRequirements[day]">{{shift}}, </span>
              </div>
          </div>
          <div class="ConsiderationCard">
              <strong style="margin-left:0; margin-bottom:5px;">The following managers have accrued this number of days:</strong>
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
    'fetchVacationHistory',
    'fetchRequiredShifts',
    'fetchLoggedInUser'
    ])
    },
    computed: {
    ...mapGetters([
    'getWeek',
    'getManagerDays',
    'getUser',
    'getVacationHistory',
    'getWeeklyRequirements'
    ])
    },
    created() {
          
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
