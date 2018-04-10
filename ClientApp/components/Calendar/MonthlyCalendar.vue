<template>
    <div id="wrapper" class="flex-container">
        <shift-modal v-if="showModal">
            <h3 slot="header" class="modal-card-title">Set The Shift</h3>
            <div slot="body">
                <div>Shift Date {{ this.shiftDate }}</div>
                <div>
                    <span>Manager Name</span>
                    <select id='managerName'
                            class="form-control"
                            v-model="selectedManager">
                        <option v-for="manager in getManagers">{{ manager.Name }}</option>
                    </select>
                </div>
                <span v-if="selectedManager =='Manager In Charge'">If Manager in Charge</span>
                <input v-if="selectedManager =='Manager In Charge'" v-model="managerInCharge" placeholder="Manager in Charge name"/>
                <div>
                    <span>Shift</span>
                    <select id='ShiftCode'
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
                <div>Shift Date {{ this.dayToChange }}  Shift: {{this.shiftCode}}</div>
                <div>
                    <span>Manager Name</span>
                    <select id='managerName1'
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
        <shift-modal v-if="printModal">
            <h3 slot="header" class="=modal-card-title">Which period would you like to print?</h3>
            <div slot="body">
                <p>Select a start and end date</p>
                <div style="width:15%; float:left;" >
                    <b-field label="">
                        <b-datepicker placeholder="Type or select a date..."
                                      icon="calendar-today"
                                      :readonly="false"
                                      v-model="PrintStartDate"
                                      :first-day-of-week=1>
                        </b-datepicker>
                    </b-field>
                    <b-field label="">
                        <b-datepicker placeholder="Type or select a date..."
                                      icon="calendar-today"
                                      :readonly="false"
                                      v-model="PrintEndDate"
                                      :first-day-of-week=1>
                        </b-datepicker>
                    </b-field>
                </div>
            </div>
            <div slot="footer">
                <button @click="printSchedule()"> Submit </button>
                <button @click="closeModal()">Close</button>
            </div>
        </shift-modal>
        <div class="title">
            <h1 style="text-align:center">Schedule for Store {{getUser.locationId}} Manager shifts</h1>
            <button v-if="this.getUser.role === 3"
                    style="margin-left:80%;
                    font-size:x-small;
                    border-radius:5px;
                    border:1px solid black;"
                    :onclick="gotoShift">
                ><router-link to="/dailyShiftRequirements">Change Daily shift requirements</router-link>
            </button>
            <button style="margin-left:80%;
                    font-size:x-small;
                    border-radius:5px;
                    border:1px solid black;"
                    :onclick="gotoShift">
                ><router-link :to=this.weeklyUrl>switch to weekly</router-link>
            </button>
            <button style="margin-left:80%;
                    font-size:x-small;
                    border-radius:5px;
                    border:1px solid black;"
                    v-on:click="showPrintModal">
                Download the monthly schedule
            </button>
        </div>    
        <calendar :firstDay="1"
                  :events='getSchedule'
                  local="en"
                  @dayClick="addShift($event)"
                  @eventClick="changeShift($event)">
            >
        </calendar>
    </div>
</template>

<script>
    import moment from 'moment'
  import { mapActions, mapGetters } from 'vuex'
  import Toasted from 'vue-toasted';
    
    export default {
        name: 'monthly-calendar',
    data () {
         return {
            showModal: false,
            printModal:false,
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
            params: '',
            events: [{}],
            weeklyUrl: "",
            PrintEndDate: new Date(),
            PrintStartDate: new Date(),
            managerInCharge:""
         }
    },
    computed: {
        ...mapGetters([
            'getSchedule',
            'getManagers',
            'getShiftCodes',
            'getUser'
        ]),
        regetSchedule() {
            return this.fetchSchedule(this.state)
        }
    },
    methods: {
      ...mapActions([
            'fetchSchedule',
            'fetchManagers',
            'fetchShiftCodes',
            'submitNewShift',
            'submitShiftChange',
            'fetchLoggedInUser',
            'exportMonth'
        ]),
      getEow() {
            let daysuntileow = 7 - moment().format('d');
            let eowstring = moment().add(daysuntileow, 'day').format('MM-DD-YYYY')
            return eowstring
        },
      addShift(event) {
          this.shiftDate = moment(event.toString()).format("MM-DD-YYYY")
          this.showModal = true
      },
      changeShift(event) {
            this.shiftCode = event.YOUR_DATA.class
            this.dayToChange = event.start
            this.showChangeModal = true
            this.managerChangeFrom = event.title.split(' ')[0] + ' ' + event.title.split(' ')[1]
            this.managerFromId = event.YOUR_DATA.ManagerId
      },
      checkWindowSize() {
          let size = window.innerWidth;
          if(size < 800) window.location.href = this.weeklyUrl
      },
      showPrintModal() {
          this.printModal = true;
      },
      closeModal () {
          this.showModal = false
          this.showChangeModal = false
          this.printModal = false
      },
      printSchedule() {
          let payload = {
              start: this.PrintStartDate.toString(),
              end: this.PrintEndDate.toString(),
              locationId:this.$store.state.loggedInUser.locationId
          }
          this.exportMonth(payload)
          this.closeModal();
      },
      submit() {
            console.log(this.selectedManager)
            let shiftDay = moment(this.shiftDate, 'MMM-DD-YYYY').format('MM-DD-YYYY')
            let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
            let shiftCodeIndex = this.getShiftCodes.findIndex(x => x.description === this.newShiftCode)
            let aNewShift = {
              LocationId: this.$store.state.loggedInUser.locationId,
              ShiftCode: this.getShiftCodes[shiftCodeIndex].statusId,
              ManagerId: this.getManagers[managerIndex].Id,
              Day: this.shiftDate,
              ShiftStatus: this.getShiftCodes[shiftCodeIndex].shiftStatus
            }
            if (this.selectedManager === "Manager In Charge") {
                aNewShift.ManagerId = this.managerInCharge + " IC"
            }
            this.submitNewShift(aNewShift)
            this.emptyFields()
            this.closeModal()
            this.submissionCompletion()
      },
      submitChange () {
          let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
          console.log(managerIndex)
          let GMIndex = this.getManagers.findIndex(m => m.Role === 3)
          console.log(GMIndex)
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
              LocationId: this.$store.state.loggedInUser.locationId,
              RequestingManagerRole: this.$store.state.loggedInUser.role,
              managerFromId:this.managerFromId
            }
            this.submitShiftChange(shiftChange)
            this.closeModal()
            this.emptyFields()
            this.submissionCompletion()
      },
      emptyFields () {
            this.selectedManager = ''
            this.newShiftCode = ''
            this.reasonForChange = ''
      },
      submissionCompletion() {
            setTimeout(() => {this.fetchSchedule(this.$store.state.loggedInUser.locationId)}, 200)
            this.$toast.open({duration:1000, type:'is-success', message: 'Shift change saved'})
      },
      gotoShift() {
          
      }      
    },    
    created() {
    this.weeklyUrl = "/ops/approveSchedule/?eow=" + this.getEow();
    this.checkWindowSize()
    this.fetchLoggedInUser()
        .then(() => {
            this.fetchSchedule(this.$store.state.loggedInUser.locationId)
                .then(() => {
                    this.fetchManagers(this.$store.state.loggedInUser.locationId)
                    this.fetchShiftCodes(this.state)
                    console.log(this.$store.state)
                })
        })      
    }
  }
</script>

<style>
    .full-calendar-body .dates .dates-events .events-week .events-day {
        min-height:100px !important;
    }
    .event-box .event-item {
        background-color: #015351 !important;
        color:white !important
    }
</style>
