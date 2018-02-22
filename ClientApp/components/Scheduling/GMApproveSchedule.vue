<template>
    <div class="container" style="width:100%">
      <h1 class="title" style="margin: 0 auto; font-size: x-large; text-align: center">Please confirm the week's schedule</h1>
      <hr/>
      <weekly-calendar :eow="this.eow" :location-id="this.locationId"></weekly-calendar>

      <DisapproveModal v-if="showModal" @close="rejected($event)"></DisapproveModal>
      <div style="width:50%; margin: 5px auto; vertical-align:central" >
        <button class="button is-info" @click="this.approveThisSchedule">Approve</button>
        <button class="button is-warning" @click="this.rejectSchedule">Reject</button>
      </div>
    </div>
</template>

<script>
  import { mapActions, mapGetters } from 'vuex'
  import WeeklyCalendar from '../Calendar/WeeklyCalendar'
  import DisapproveModal from './DisapproveModal'

  export default {
    name: 'gmapproveschedule',
    components: {
      WeeklyCalendar: WeeklyCalendar,
      DisapproveModal
    },
    methods: {
      ...mapActions([
        'approveSchedule'
      ]),
      approveThisSchedule () {
        let approval = {
          eow: this.eow,
          GmId: 1,
          RestaurantId: this.locationId
        }
        this.approveSchedule(approval)
      },
      rejectSchedule () {
        this.showModal = true
      },
      rejected (event) {
        this.showModal = false
      }
    },
    computed: {
      ...mapGetters([])
    },
    data () {
      return {
        eow: this.$route.query.eow,
        reason: '',
        showModal: false,
        locationId: this.$route.query.locationId
      }
    },
    created () {
    }
  }
</script>

<style scoped>

</style>
