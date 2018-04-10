<template>
    <div class="cellholder container">
        <div class="pageChoice" @click="$router.push('weeklySchedule')">
            <strong>Weekly Schedule</strong>
            <hr />
            <p v-if="getUser.role == 3">View or approved this week's schedule</p>
            <p v-if="getUser.role !== 3">View this week's schedule</p>
        </div>
        <div class="pageChoice" @click="$router.push('MonthlySchedule')">
            <strong>Monthly Schedule</strong>
            <hr />
            <p>View/print this month's schedule</p>
        </div>
        <div class="pageChoice" @click="$router.push('payroll')" v-if="getUser.role === 0">
            <strong>PayRoll</strong>
            <hr />
            <p>PayRoll's schedule page</p>
        </div>
        <div class="pageChoice" v-if="getUser.role == 3" @click="$router.push('dailyShiftRequirements')">
            <strong>Shift requirements</strong>
            <hr />
            <p>Set the daily shift requirements for the restaurant</p>
        </div>
        <div class="pageChoice" v-if="getUser.role == 3">
            <strong>Shift requirements</strong>
            <hr />
            <p>Set the daily shift requirements for the restaurant</p>
        </div>
        <!--<div class="pageChoice" v-if="getUser.role == 3">
            <strong>Shift Change Requests</strong>
            <hr />
            <p>View or approve requested schedule changes</p>
        </div>-->
    </div>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'

    export default {
        name: "schedulingPages",
        methods: {
            ...mapActions([
                'fetchLoggedInUser'
            ])
        },
        computed: {
            ...mapGetters([
                'getUser'
                ])
        },
        created() {
            this.fetchLoggedInUser()
                .then(() => { })
        }
    }
</script>

<style>
    .pageChoice {
        float:left;
        padding-top: 2%;
        height: 20vh;
        width: 20vh;
        border: 1px solid black;
        border-radius: 5px;       
        margin: 2%;
        background-color: white;
    }
    .cellholder {
        text-align: center;
        height: 80%;
        width: 80%;
        border: 1px solid black;
        border-radius: 5px;
        box-shadow: 5px 5px grey;
        margin-left: 10%;
        margin-bottom: 25% !important;
        margin-top: 6% !important;
        padding: 1%;
        background-color: #64433C;
    }
</style>
