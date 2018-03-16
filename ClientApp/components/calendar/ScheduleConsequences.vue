<template>
  <section class="card ">
      <div class="card-content container">
          <strong>Things To Consider</strong><hr />
          <div class="ConsiderationCard" style="margin-left:1.5vw;">
              <strong style="margin-left:18%; margin-bottom:5px;">Shift break down for the week</strong>
              <div v-for="manager in getManagerDays" v-if="manager.name != 'Cancel Shift'" >
                  <p style="margin:1% 10%">
                      - {{manager.name}}
                      <span v-if="manager.shifts !== 5" style="color:red">has {{manager.shifts}} shifts</span>
                      <span v-if="manager.shifts === 5">has 5 shifts</span>
                      <!--<span v-if="manager.daysToOwe !== 0"> Will be owed {{manager.daysToOwe}} shift</span>-->
                  </p>
                  <hr style="margin:1px;" />
              </div>
          </div>
          <div class="ConsiderationCard">
              <strong style="margin-left:14%; margin-bottom:5px;">Make sure that these shifts are covered:</strong>
              <div v-for="day in Object.keys(getWeeklyRequirements)" v-if="day != 'id'"  >
                  <p style="margin:1% 10%">
                      {{day}}:
                      <span v-for="shift in getWeeklyRequirements[day]">{{shift}}, </span>
                  </p>
              <hr style="margin:1px" />
              </div>
          </div>
          <div class="ConsiderationCard">
              <strong style="margin-left:0; margin-bottom:5px;">The following managers have accrued this number of days:</strong>
              <div v-for="history in getVacationHistory">
                  <p>
                      {{history.managerFirstName}} {{history.managerLastName}} has {{Math.round(history.statHolidaysOwed + history.vacationOwed)}} days owed
                  </p>
                  <p>At years end they will be owed {{Math.round(getDays(history.vacationRate) + history.vacationOwed)}} days</p>

                  <hr style="margin:1px" />
              </div>
          </div>
      </div>
  </section>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'
    import moment from 'moment'

    export default {
        name: 'schedule-consequences',
        data() {
            return {
                managerDays: [],
                schedule: [],
                consequences: []
            }
        },
        methods: {
            ...mapActions([
                'fetchVacationHistory',
                'fetchRequiredShifts',
                'fetchLoggedInUser',
                'fetchManagers',
                'fetchWeek'
            ]),
            getDays(rate) {
                var days = ((365 - moment().format("DDDD")) * rate)/1.4
                return days
            }
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
            var param = {
                eow: this.eow,
                locationId: this.$store.state.loggedInUser.locationId
            }
            this.fetchVacationHistory(this.$store.state.loggedInUser.locationId)
                .then(() => {
                    this.fetchRequiredShifts(this.$store.state.loggedInUser.locationId)
                    this.fetchManagers(this.$store.state.loggedInUser.locationId)
                        .then(() => { this.fetchWeek(param) })
                }) 
        }
     }
</script>

<style scoped>
  .card {
    border-radius: 5px;
    border: 1px solid darkgray;
    height: 500px;
  }
  strong {
    text-align: right;
  }
    .ConsiderationCard {
        border-radius: 5px;
        padding: 2px;
        margin-left: 1vw;
        float: left;
        text-align: left;
        border: 1px solid black;
        height:35vh;
        width:16vw;
    }
</style>
