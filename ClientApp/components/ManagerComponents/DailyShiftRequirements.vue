<template>
      <div class="Daily">
          <span class="header"><h4>{{day}}</h4></span>
          <hr />
          <ul class="shiftList">
              <li v-for="shiftCode in getShiftCodes" style="text-align:left">
                  <label>
                      <shift :ShiftCode="shiftCode.statusId"
                             :shiftDescription="shiftCode.description"
                             v-on:AddShiftCode="addShift($event)"
                             @RemoveShiftCode="removeShift($event)"></shift>
                  </label>
              </li>
          </ul>
      </div>
</template>

<script>
  import {mapActions, mapGetters} from 'vuex'
  import shift from './ShiftSelection'

  export default {
        name: 'daily-shift-requirements',
    components: {
      shift
    },
    methods: {
      ...mapActions([
            'fetchShiftCodes',
            'addShiftToDay',
            'removeShiftFromDay'
      ]),
      addShift(event) {
          let shifttoAdd = { day: this.day, shift: event }
          this.addShiftToDay(shifttoAdd)
      },
      removeShift (event) {
          let shiftToRemove = { day: this.day, shiftCode: event }
          this.removeShiftFromDay(shiftToRemove)
      },
      clicked () {
        this.submitDailyShiftRequirements(this.shiftCodes)
      }
    },
    computed: {
      ...mapGetters([
            'getShiftCodes',
            //'getUnSubShiftReq'
      ])
    },
    data () {
      return {
        shiftCodes: [],
        selected: false
      }
    },
    props:['day'],
    created () {
      this.fetchShiftCodes(this.state)
    }

  }
</script>

<style scoped>
    .Daily {
        border: 1px solid black;
        border-radius: 5px;
        padding: 5px 10px;
        margin:1%;
        width: 12%;
        box-shadow: 10px 10px 5px #999999;
        float:left;
    }
    .header{
        margin-left:30%;
        width:0 auto;
    }
    #sumbit{
        margin-left:30%;
        width: 0px auto;
    }
    .shiftList{
        list-style:none;
        text-align:left;
        padding-left:2px;
    }
</style>
