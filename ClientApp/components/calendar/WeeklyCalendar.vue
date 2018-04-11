<template>
    <div>
        <shift-modal v-if="showDayClickedModal">
            <h3 slot="header" class="modal-card-title">Set The Shift</h3>
            <div slot="body">
                <div>Shift Date {{ this.workingDate }}</div>
                <div>
                    <span>Manager Name</span>
                    <select id='managerName'
                            class="form-control"
                            v-model="selectedManager">
                        <option v-for="manager in getManagers">{{ manager.Name }}</option>
                    </select>
                </div>
                <span v-if="selectedManager =='Manager In Charge'">If Manager in Charge</span>
                <input v-if="selectedManager =='Manager In Charge'" v-model="managerInCharge" placeholder="Manager in Charge name" />
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
        <div style="vertical-align:middle; margin-bottom:5px; margin-left:45%">
            <button style="border-bottom-left-radius:15px; border-top-left-radius:15px; background-color:black; color:white";><a :href=this.lastWeekString>last week</a></button>
            <button style="border-bottom-right-radius:15px; border-top-right-radius:15px; background-color:black; color:white";><a :href=this.nextweekString>next week</a></button>
        </div>
        <div class="container-fluid weekly">
            <day class="dayBox" :Day="this.monday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.tuesday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.wednesday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.thursday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.friday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.saturday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
            <day class="dayBox" :Day="this.sunday" v-on:dayClicked="dayClicked($event)" v-on:eventClicked="eventClicked($event)"></day>
        </div>
    </div>
</template>

<script>
  import { mapActions, mapGetters } from 'vuex'
  import moment from 'moment' 
  import Toasted from 'vue-toasted';

  export default {
        name: 'weeklyCalendar',
        props: [
            'monday',
            'tuesday',
            'wednesday',
            'thursday',
            'friday',
            'saturday',
            'sunday',
            ],
    methods: {
      ...mapActions([
            'fetchWeek',
            'fetchManagers',
            'fetchShiftCodes',
            'submitNewShift',
            'submitShiftChange'
        ]),
        nextWeek() {
            let eowString = moment(this.eow, "MM-DD-YYYY").add(7, 'day').format('MM-DD-YYYY')
            return eowString
        },
        lastWeek() {
            let eowString = moment(this.eow, "MM-DD-YYYY").subtract(7, 'day').format('MM-DD-YYYY')
            return eowString;
        },
        addShift(event) {
            let dateArray = event.toString().split(' ')
            let date = dateArray[1] + '-' + dateArray[2] + '-' + dateArray[3]
            this.shiftDate = moment(date, "MM-DD-YYYY").format("MM-DD-YYYY")
            this.showModal = true
        },
        dayClicked(event) {
              this.showDayClickedModal = true
              this.workingDate = event
        },
        eventClicked(event, date) {
            this.showDayClickedModal = false
            this.showChangeModal = true
            this.managerFromId = event.managerFromId
            this.managerChangeFrom = event.name
            this.shiftCode = event.shiftCode
            this.dayToChange = event.shiftDate
        },
        submitChange() {
            let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
            let GMIndex = this.getManagers.findIndex(m => m.Role === 3)
            let shiftDayIndex = this.getSchedule.findIndex(d => d.start === this.dayToChange)
            let shiftChange = {
                RequestId: '',
                Id: this.getManagers[managerIndex].Id,
                ManagerId: this.getManagers[managerIndex].Id,
                ManagerToName: this.getManagers[managerIndex].Name,
                ManagerFromName: this.managerChangeFrom,
                EOW: this.eow,
                ShiftCode: this.shiftCode,
                Reason: this.reasonForChange,
                shiftDate: this.dayToChange,
                ManagerEmailAddress: this.getManagers[managerIndex].EmailAddress,
                GMEmailAddress: this.getManagers[GMIndex].EmailAddress,
                LocationId: this.$store.state.loggedInUser.locationId,
                RequestingManagerRole: this.$store.state.loggedInUser.role,
                managerFromId: this.managerFromId
            }
            this.submitShiftChange(shiftChange)
            this.closeModal()
            this.emptyFields()
            this.submissionCompletion()
        },
        emptyFields() {
            this.selectedManager = ''
            this.newShiftCode = ''
            this.reasonForChange = ''
        },
        submissionCompletion() {
           // this.checkMissingShifts()
            this.$emit('shiftSubmitted')
        },
        closeModal() {
            this.showDayClickedModal = false
            this.showChangeModal = false
        },
        submit() {
            let shiftDay = moment(this.workingDate, 'MM-DD-YYYY').format('MM-DD-YYYY')
            let managerIndex = this.getManagers.findIndex(x => x.Name === this.selectedManager)
            let shiftCodeIndex = this.getShiftCodes.findIndex(x => x.description === this.newShiftCode)
            let aNewShift = {
                LocationId: this.$store.state.loggedInUser.locationId,
                ShiftCode: this.getShiftCodes[shiftCodeIndex].statusId,
                ManagerId: this.getManagers[managerIndex].Id,
                Day: shiftDay,
                ShiftStatus: this.getShiftCodes[shiftCodeIndex].shiftStatus
            }
            if (aNewShift.Day && aNewShift.LocationId && aNewShift.ManagerId && aNewShift.ShiftCode && aNewShift.ShiftStatus) {
                this.submitNewShift(aNewShift)
                this.emptyFields()
                this.closeModal()
                this.submissionCompletion()
            }
            else alert('unable to submit new shift')
        }
    },
    data () {
        return {
            showDayClickedModal: false,
            selectedManager: '',
            newShiftCode: '',
            eow: this.$route.query.eow,
            locationId: this.$store.state.loggedInUser.locationId,
            workingDate: "",
            nextweekString: "",
            lastWeekString: "",
            showChangeModal: false,
            dayToChange: "",
            shiftCode: "",
            reasonForChange: "",
            managerFromId: "",
            managerInCharge:""
        }
    },
    computed: {
      ...mapGetters([
        'getWeek',
        'getManagers',
        'getShiftCodes',
        'getSchedule'
      ])
    },
    created() {
        this.nextweekString = "/ops/weeklySchedule/?eow=" + this.nextWeek()
        this.lastWeekString = "/ops/weeklySchedule/?eow=" + this.lastWeek()
    }
  }
</script>

<style scoped>
    @media(min-width:1400px) {
        .day {
            width: 13%;
            background: white;
            float: left;
            margin: 0px 8px;
            height: 55vh;
            box-shadow: 10px 10px 5px #999999;
            border: 1px solid black;
            border-radius: 5px;
        }
        .consequences {
            background: white;
            vertical-align: central;
            text-align: center;
            margin: auto;
            padding: auto;
            float: none;
            clear: both;
            overflow: auto;
            border: 1px solid black !important;
            border-radius: 5px;
            box-shadow: 10px 10px 5px #999999;
            width: 80%;
        }
    }
    @media only screen and (max-width: 1399px) and (min-width: 801px) {
        .day {
            width: 40% !important;
            background: white !important;
            float: left !important;
            margin: 0px 8px 8px !important;
            height: 55vh !important;
            box-shadow: 10px 10px 5px #999999 !important;
            border: 1px solid black !important;
            border-radius: 5px !important;
        }

        .consequences {
            background: white;
            vertical-align: central;
            text-align: center;
            margin: auto;
            padding: auto;
            float: none;
            clear: both;
            overflow: auto;
            border: 1px solid black !important;
            border-radius: 5px;
            box-shadow: 10px 10px 5px #999999;
            width: 80%;
        }
    }
    @media(max-width:800px) {
        .day {
            width: 80%;
            background: white;
            float: left;
            margin: 0px 8px 8px !important;
            box-shadow: 10px 10px 5px #999999 !important;
            border: 1px solid black !important;
            border-radius: 5px !important;
            overflow: auto !important;
        }
        .consequences {
            background: white;
            vertical-align: central;
            text-align: center;
            margin: auto;
            padding: auto;
            float: none;
            clear: both;
            overflow: auto;
            border: 1px solid black !important;
            border-radius: 5px;
            box-shadow: 10px 10px 5px #999999;
            width: 80%;
        }
    }
    .weekly{
        width:90%;
        margin:0 0 0 10px 0;
        padding:0px;
    }
   
</style>
