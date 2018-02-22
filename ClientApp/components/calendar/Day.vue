<template>
  <section class="day">
    <div >
      <strong v-if="this.Day.date">{{ this.getDay(this.Day.date) }} {{ this.Day.date}}</strong>
      <div class="card" v-for="shift in this.Day.shifts">
        <div style="padding:2px 2px;">Shift: {{shift.shiftType}}</div>
        <div style="padding:2px 2px;">Manager: {{shift.managerName}}</div>
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
