<template>
  <div id="wrapper" class="container">
    <shift-modal v-if="showModal">
      <h3 slot="header" class="modal-card-title">Set The Shift</h3>
      <div slot="body">
        <div>Shift Date {{ this.shiftDate }}</div>
        <div><span>Manager Name</span>
          <select id = 'managerName'
                  class="form-control"
                  v-model="selectedManager">
            <option v-for="manager in getManagers">{{ manager.Name }}</option>
          </select>
        </div>
        <div><span>Shift</span>
          <select id = 'ShiftCode'
                  class="form-control"
                  v-model="newShiftCode">
            <option v-for="code in getShiftCodes"> {{ code.description }} </option>
          </select>
        </div>
      </div>
      <div slot="footer">
        <button @click="submit()"> Submit </button>
      <button @click="closeModal()">Close</button>
      </div>
    </shift-modal>
    <shift-modal v-if="showChangeModal">
      <h3 slot="header" class="modal-card-title">Change The Shift</h3>
      <div slot="body">
        <div>Shift Date {{ this.dayToChange }}</div>
        <div><span>Manager Name</span>
          <select id = 'managerName1'
                  class="form-control"
                  v-model="selectedManager">
            <option v-for="manager in getManagers">{{ manager.Name }}</option>
          </select>
        </div>
        <div>
          <div>
            Reason For Change
          </div>
          <div>
            <input type="text" v-model="reasonForChange">
          </div>
        </div>
      </div>
      <div slot="footer">
        <button @click="submitChange()"> Submit </button>
        <button @click="closeModal()">Close</button>
      </div>
    </shift-modal>
    <div class="title">
      <h1>Schedule for Store 104 Manager shifts</h1>
    </div>
    <calendar
      :events="getSchedule"
      :ShowLimit="4"
      @dayClicked="addShift($event)"
      @eventClicked="changeShift($event)"
    ></calendar>
  </div>
</template>

<script>
  import calendar from 'vue2-simple-calendar'
  import ShiftModel from './ShiftModal.vue'
  import moment from 'moment'
  import { mapActions, mapGetters } from 'vuex'

  export default {
    name: 'monthly-calendar',
    data () {
      return {
        showModal: false,
        manager: '',
        shiftCode: '',
        shiftDate: '',
        shiftEOW: '',
        Date: this.shiftDate,
        newName: '',
        newShiftCode: '',
        reasonForChange: '',
        selectedManager: '',
        dayToChange: '',
        shiftCodeToChangeFrom: '',
        showChangeModal: false,
        managerChangeFrom: '',
        concerns: [{}],
        params: ''
      }
    },
    components: {
      calendar: calendar,
      shiftModal: ShiftModel
    },
    methods: {
      ...mapActions([
        'fetchSchedule',
        'fetchManagers',
        'fetchShiftCodes',
        'submitNewShift',
        'submitShiftChange'
      ]),
      addShift (event) {
        let dateArray = event.date.toString().split(' ')
        this.shiftDate = dateArray[1] + '-' + dateArray[2] + '-' + dateArray[3]
        this.showModal = true
      },
      changeShift (event) {
        this.shiftCode = event.class
        this.dayToChange = event.start
        this.showChangeModal = true
        this.managerChangeFrom = event.title.split(' ')[0] + ' ' + event.title.split(' ')[1]
      },
      closeModal () {
        this.showModal = false
        this.showChangeModal = false
      },
      submit () {
        let shiftDay = moment(this.shiftDate, 'MMM-DD-YYYY').format('MM-DD-YYYY')
        let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
        let shiftCodeIndex = this.getShiftCodes.findIndex(x => x.description === this.newShiftCode)
        let aNewShift = {
          LocationId: this.params,
          ShiftCode: this.getShiftCodes[shiftCodeIndex].code,
          ManagerId: this.getManagers[managerIndex].Id,
          Day: shiftDay
        }
        this.submitNewShift(aNewShift)
        this.emptyFields()
        this.closeModal()
        setTimeout(() => { this.submissionCompletion() }, 300)
      },
      submitChange () {
        let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
        let GMIndex = this.getManagers.findIndex(m => m.Role === 3)
        let shiftDayIndex = this.getSchedule.findIndex(d => d.start === this.dayToChange)
        let shiftChange = {
          RequestId: '',
          Id: this.getManagers[managerIndex].Id,
          ManagerId: this.getManagers[managerIndex].Id,
          ManagerToName: this.getManagers[managerIndex].Name,
          ManagerFromName: this.managerChangeFrom,
          EOW: this.getSchedule[shiftDayIndex].EOW,
          ShiftCode: this.shiftCode,
          Reason: this.reasonForChange,
          shiftDate: this.dayToChange,
          ManagerEmailAddress: this.getManagers[managerIndex].EmailAddress,
          GMEmailAddress: this.getManagers[GMIndex].EmailAddress,
          LocationId: 104
        }
        this.submitShiftChange(shiftChange)
        this.closeModal()
        this.emptyFields()
        setTimeout(() => { this.submissionCompletion() }, 300)
      },
      emptyFields () {
        this.selectedManager = ''
        this.newShiftCode = ''
        this.reasonForChange = ''
      },
      submissionCompletion () {
        this.$toast.open('Shift change saved')
        this.fetchSchedule(this.state)
      }
    },
    computed: {
      ...mapGetters([
        'getSchedule',
        'getManagers',
        'getShiftCodes'
      ]),
      regetSchedule () {
        return this.fetchSchedule(this.state)
      }
    },
    created () {
      this.params = this.$route.query.locationId
      console.log(this.params)
      this.fetchSchedule(this.params)
      this.fetchManagers(this.state, this.params)
      this.fetchShiftCodes(this.state)
    }

  }
</script>

<style>

</style>
