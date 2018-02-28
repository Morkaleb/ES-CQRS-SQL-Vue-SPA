<template>
  <section class="day" v-on:click="$emit('dayClicked', Day.date)">
    <div >
      <strong v-if="this.Day.date">{{ this.getDay(this.Day.date) }} {{ this.Day.date}}</strong>
      <div v-for="theshift in this.Day.shifts" >
          <shift :date="Day.date" :shiftCode="theshift.shiftCode" :shiftType="theshift.shiftType" :managerName="theshift.managerName" :managerFromId="theshift.managerId" v-on:eventClicked="eventClicked($event)"></shift>
      </div>
        
    </div>
  </section>
</template>

<script>
    import moment from 'moment'
    import shift from './Shift'

   export default {
     name: 'day',
     props: ['thisDay', 'Day'],
     methods: {
       getDay (date) {
         return moment(date, 'MM-DD-YYYY').format('dddd')
         },
       eventClicked(event) {
           event.shiftDate = this.Day.date
           this.$emit('eventClicked', event)
       },
       components: { shift:shift }
     }
   }
</script>

<style scoped>
  .card {
      margin:6px 2px;
      border:1px solid black;
      border-radius:5px;
      font-size:small;
      padding:2px;
  }
  strong {
    text-align: right;
    font-size:medium;
  }  
</style>
