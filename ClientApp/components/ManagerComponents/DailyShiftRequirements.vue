<template>
  <div class="container">
    <ul>
      <li v-for="shiftCode in getShiftCodes">
        <label>
          <shift :ShiftCode="shiftCode.code"
                 :shiftDescription="shiftCode.description"
                 @AddShiftCode="addShift($event)"
                 @RemoveShiftCode="removeShift($event)"
          ></shift>
        </label>
      </li>
    </ul>
    <button @click="clicked()">submit</button>
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
        'submitDailyShiftRequirements'
      ]),
      addShift (event) {
        this.shiftCodes.push(event)
      },
      removeShift (event) {
        var index = this.shiftCodes.findIndex(c => c === event)
        this.shiftCodes.splice(index, 1)
      },
      clicked () {
        this.submitDailyShiftRequirements(this.shiftCodes)
      }
    },
    computed: {
      ...mapGetters([
        'getShiftCodes'
      ])
    },
    data () {
      return {
        shiftCodes: [],
        selected: false
      }
    },
    created () {
      this.fetchShiftCodes(this.state)
    }

  }
</script>

<style scoped>

</style>
