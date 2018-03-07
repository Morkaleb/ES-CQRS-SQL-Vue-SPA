<template>
    <div class="container" style="width:100%">
        <h1 class="title" style="margin: 0 auto; font-size: x-large; text-align: center">
            Please confirm the week's schedule
        </h1>
        <hr />
        <weekly-calendar
                         :monday="this.monday"
                         :tuesday="this.tuesday"
                         :wednesday="this.wednesday"
                         :thursday="this.thursday"
                         :friday="this.friday"
                         :saturday="this.saturday"
                         :sunday="this.sunday"
                         @shiftSubmitted="this.shiftSubmitted">
            </weekly-calendar> <hr />
        <div>
            <ScheduleConsequences :consequences="this.getManagers" class="consequences"></ScheduleConsequences>
        </div>
        <DisapproveModal v-if="disapproveModal" @close="closeModal"></DisapproveModal>
        <ApproveModal v-if="approvalModal" @close="closeModal"></ApproveModal>
        <div style="width:50%; margin: 5px auto; vertical-align:central">
            <button class="button is-info" @click="this.approveScehudule">Approve</button>
            <button class="button is-warning" @click="this.rejectSchedule">Reject</button>
        </div>
    </div>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'
    import WeeklyCalendar from '../Calendar/WeeklyCalendar'
    import DisapproveModal from './DisapproveModal'
    import ApproveModal from './ApproveModal'
    import shift from '../Calendar/Shift'
    import ScheduleConsequences from '../Calendar/ScheduleConsequences'
    import moment from 'moment'

    export default {
    name: 'gmapproveschedule',
    components: { WeeklyCalendar, DisapproveModal, ApproveModal, ScheduleConsequences },
    methods: {
        ...mapActions([
            'approveSchedule',
            'fetchLoggedInUser',
            'fetchVacationHistory',
            'fetchRequiredShifts',
            'fetchManagers',
            'fetchWeek',
            'fetchShiftCodes'
        ]),
        distributeWeek(week) {
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
        },
        closeModal() {
            this.disapproveModal = false
            this.approvalModal = false
        },
        approveThisSchedule() {
            let approval = {
                eow: this.eow,
                GmId: this.$store.state.loggedInUser.id,
                RestaurantId: this.$store.state.loggedInUser.locationId
            }
            this.approveSchedule(approval)
        },
        rejectSchedule() {
            this.disapproveModal = true
        },
        approveScehudule() {
            this.approvalModal = true
        },
        shiftSubmitted() {
            var param = {
                eow: this.eow,
                locationId: this.$store.state.loggedInUser.locationId
            }
            setTimeout(() => {
                this.fetchWeek(param)
                    .then(() => {
                        this.distributeWeek(this.getWeek)
                    })
            }, 200)
        },
        rejected (event) {
            this.disapproveModal = false
        }
    },
    computed: {
        ...mapGetters([
            'getWeek',
            'getManagers'
        ])
    },
    data () {
        return {
            eow: this.$route.query.eow,
            reason: '',
            disapproveModal: false,
            approvalModal: false,
            monday: {},
            tuesday: {},
            wednesday: {},
            thursday: {},
            friday: {},
            saturday: {},
            sunday: {},
        }
    },
    created () {
        this.fetchLoggedInUser()
            .then(() => {
                var param = {
                    eow: this.eow,
                    locationId: this.$store.state.loggedInUser.locationId
                }
                this.fetchWeek(param)
                    .then(() => {
                        this.distributeWeek(this.getWeek)
                        this.fetchManagers(this.$store.state.loggedInUser.locationId)
                        this.fetchShiftCodes(this.state)
                        this.fetchVacationHistory(this.$store.state.loggedInUser.locationId)
                        this.fetchRequiredShifts(this.$store.state.loggedInUser.locationId)
                        this.fetchManagers(this.$store.state.loggedInUser.locationId)
                    })
            })
          
           
        }
    }
</script>

<style scoped>

    .consequences {
        background: white;
        vertical-align: central;
        text-align: center;
        margin: auto;
        padding: auto;
        float: none;
        clear: both;
        height: 50vh !important;
        border: 1px solid black !important;
        border-radius: 5px;
        box-shadow: 10px 10px 5px #999999;
    }
</style>
