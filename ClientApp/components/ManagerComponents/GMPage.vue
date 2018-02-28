<template>
    <div class="container">
        Please select the Management page that you require
        <button class="btn-default"><router-link to="/dailyShiftRequirements">Daily shift requirements</router-link></button>
        <button class="btn-default"><router-link :to=this.week>check/approve next week schedule</router-link></button>
    </div>
</template>
<script>
    import { mapActions, mapGetters } from 'vuex'
    import moment from 'moment'

    export default {
        data() {
            return {
                loggedInUser: {},
                week: ""
            }
        },
        methods: {
            ...mapActions([
            ]),
            getEow() {
                let daysuntileow = 7 - moment().format('d');
                let eowstring = moment().add(daysuntileow, 'day').format('MM-DD-YYYY')
                return eowstring
            }
        },
        computed: {
            ...mapGetters([
            ])
        },
        created() {
            this.loggedInUser = this.$store.state.loggedInUser
            this.week = "/approveSchedule/?eow=" + this.getEow();
        }
    }
</script>
<stlye></stlye>
