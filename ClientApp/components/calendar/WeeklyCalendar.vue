<template>
  <div class="container">
    <day class="dayBox" :Day="this.monday"></day>
    <day class="dayBox" :Day="this.tuesday"></day>
    <day class="dayBox" :Day="this.wednesday"></day>
    <day class="dayBox" :Day="this.thursday"></day>
    <day class="dayBox" :Day="this.friday"></day>
    <day class="dayBox" :Day="this.saturday"></day>
    <day class="dayBox" :Day="this.sunday"></day>
    <ScheduleConsequences :consequences="this.getManagers" class="dayBox"></ScheduleConsequences>
  </div>
</template>

<script>
  import { mapActions, mapGetters } from 'vuex'
  import moment from 'moment'
  import Day from './Day'
  import ScheduleConsequences from './ScheduleConsequences'

  export default {
    name: 'weekly-calendar',
    components: {Day, ScheduleConsequences},
    props: ['eow', 'locationId'],
    methods: {
      ...mapActions([
        'fetchWeek',
        'fetchManagers'
      ]),
      distributeWeek (week) {
        for (var day in week) {
          var dotw = moment(week[day].date, 'MM-DD-YYYY').format('ddd')
          if (dotw === 'Mon') {
            this.monday = week[day]
          }
          if (dotw === 'Tue') {
            this.tuesday = week[day]
          }
          if (dotw === 'Wed') {
            this.wednesday = week[day]
          }
          if (dotw === 'Thu') {
            this.thursday = week[day]
          }
          if (dotw === 'Fri') {
            this.friday = week[day]
          }
          if (dotw === 'Sat') {
            this.saturday = week[day]
          }
          if (dotw === 'Sun') {
            this.sunday = week[day]
          }
        }
      }
    },
    data () {
      return {
        monday: {},
        tuesday: {},
        wednesday: {},
        thursday: {},
        friday: {},
        saturday: {},
        sunday: {},
        week: []
      }
    },
    computed: {
      ...mapGetters([
        'getWeek',
        'getManagers'
      ])
    },
    created () {
      var param = {
        eow: this.eow,
        locationId: this.locationId
      }
      this.fetchManagers()
      this.fetchWeek(param)
        .then(() => {
          this.distributeWeek(this.getWeek)
        })
    }
  }
</script>

<style scoped>
  .dayBox{
    width:11%;
    background: lightgray;
    float:left;
    margin: 5px ;
    height: 100px;
  }
</style>
