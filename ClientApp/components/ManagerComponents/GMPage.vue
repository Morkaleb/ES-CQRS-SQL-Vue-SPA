<template>
    <div class="week">
        <ShiftRequirementsModal v-if="showModal" @close="closeModal()" @submit="submit()"></ShiftRequirementsModal>
        <h2 class="header">Set Daily Shift Requirements</h2>
        <DailyShiftRequirements class="day" day="Monday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Tuesday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Wednesday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Thursday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Friday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Saturday"></DailyShiftRequirements>
        <DailyShiftRequirements v-on:AddShiftDayCode="addshiftDay($event)"class="day" day="Sunday"></DailyShiftRequirements>
        <button
                v-on:click="submitShiftsRequirements"
                class="btn btn-default"
                style="vertical-align:central;
                background-color:#015351;
                color: white;
                margin-top:10px;
                margin-left:45%">
        Submit</button>
    </div>
</template>

<script>
    import DailyShiftRequirements from './DailyShiftRequirements'
    import { mapActions, mapGetters } from 'vuex'
    import ShiftRequirementsModal from './ShiftRequirementsModal'

    export default {
        name: 'GmPage',
        data() {
            return {
                showModal: false
            }
        },
        components: {
            DailyShiftRequirements,
            ShiftRequirementsModal
        },
        computed: {
            ...mapGetters([
                'getUnSubShiftReq'
            ])
        },
        methods: {
            ...mapActions([
                'submitDailyShiftRequirements'
            ]),
            addShiftDay(event) {
                console.log(1)
            },
            submitShiftsRequirements() {
                this.showModal = true
            },
            closeModal() {
                this.showModal = false
            },
            submit() {
                this.submitDailyShiftRequirements();
                this.showModal = false
            }
        }
    }
</script>

<style>
    .day{
        position:relative;
        float:left;
        width:14%;
    }
    .week{
        display:inline-block;
        width:100%;
    }
    .header{
        vertical-align: central;
        text-align: center;
        width: 0px auto;
        margin:0px;
        padding:0px;
    }
</style>
