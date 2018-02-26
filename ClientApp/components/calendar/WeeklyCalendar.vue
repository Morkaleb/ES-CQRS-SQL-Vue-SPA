<template>
    <div>
        <div class="container weekly">
            <day class="dayBox" :Day="this.monday"></day>
            <day class="dayBox" :Day="this.tuesday"></day>
            <day class="dayBox" :Day="this.wednesday"></day>
            <day class="dayBox" :Day="this.thursday"></day>
            <day class="dayBox" :Day="this.friday"></day>
            <day class="dayBox" :Day="this.saturday"></day>
            <day class="dayBox" :Day="this.sunday"></day>
        </div>
        <hr />
        <div>
            <ScheduleConsequences :consequences="this.getManagers" class="consequences"></ScheduleConsequences>
        </div>
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
    .dayBox {
        width: 13%;
        background: white;
        float: left;
        margin:0px 8px;
        height: 80vh;
        box-shadow: 10px 10px 5px #999999;
        border: 1px solid black;
        border-radius:5px;
    }
    .weekly{
        width:100%;
        margin:0 0 0 10px 0;
        padding:0px;
    }
    .consequences {
        background: white;
        vertical-align: central;
        text-align:center;
        margin: auto;
        padding: auto;
        float: none;
        clear: both;
        width: 80%;
        height: 60vh !important;
        border: 1px solid black !important;
        border-radius: 5px;
        box-shadow: 10px 10px 5px #999999;
    }
</style>
