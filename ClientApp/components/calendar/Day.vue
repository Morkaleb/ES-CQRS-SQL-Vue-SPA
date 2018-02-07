<template>
  <section class="card">
    <div class="card-content">
      <strong v-if="this.Day.date">{{ this.getDay(this.Day.date) }} {{ this.Day.date}}</strong>
      <div v-for="shift in this.Day.shifts"><hr/>
        <div>Shift: {{shift.shiftType}}</div>
        <div>Manager: {{shift.managerName}}</div>
      </div>
    </div>
  </section>
</template>

<script>
   import moment from 'moment'

   export default {
     name: 'day',
     props: ['thisDay', 'Day'],
     methods: {
       getDay (date) {
         return moment(date, 'MM-DD-YYYY').format('dddd')
       }
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
</style>
