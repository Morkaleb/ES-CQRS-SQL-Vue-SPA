<template>
<div class="container">
  <div v-for="request in getChangeRequest" id="request">
    <section class="hero is-light" id="changeRequest">
      <div class="hero-body">
        <h1 class="title"> {{ request.fromName }} would like to trade change their shift </h1>
        <h2 class="subtitle"> Shift Date is {{ request.shiftDate}}</h2>
        <p> This request would change the manager for that shift from  {{ request.fromName }} to {{ request.toName }}</p>
        <p> the shiftCode for this shift is: {{ request.shiftCode }}</p>
        <p> the reason for the request is: {{ request.reason }}</p>
        <hr/>
        <div>
          Reason for decision:
          <input type="text" v-model="gmReason" style="width:80%">
        </div> <br/>
        <div style="width: 50%; margin: 0 auto;">
          <router-link
            v-on:click.native="approveChange(request)"
            class="button is-info"
            to="/schedule"
           >
            Approve
          </router-link>
          <router-link
            style="margin-left: 20px"
            v-on:click.native="rejectChange(request)"
            class="button is-warning"
            to="/schedule"
          >
            Reject
          </router-link>
        </div>
      </div>
    </section>
  </div>
  <div style="margin: 0 auto">
    <weekly-calendar :locationId="104" :eow="this.getChangeRequest[0].eow"></weekly-calendar>
  </div>
  </div>
</template>

<script>
  import { mapActions, mapGetters } from 'vuex'
  import WeeklyCalendar from '../Calendar/WeeklyCalendar'

  export default {
    components: {WeeklyCalendar},
    methods: {
      ...mapActions([
        'fetchChangeRequest',
        'gmAcceptShiftChange',
        'gmRejectShiftChange'
      ]),
      components: {
        WeeklyCalendar: WeeklyCalendar
      },
      approveChange (request) {
        let approval = {GMId: 1, Request: request}
        this.gmAcceptShiftChange(approval)
      },
      rejectChange (request) {
        let rejection = {
          GMId: 1,
          Id: request.fromId,
          requestId: request.id,
          Reason: this.gmReason }
        this.gmRejectShiftChange(rejection)
      }
    },
    computed: {
      ...mapGetters([
        'getChangeRequest'
      ])
    },
    data () {
      return {
        Id: this.$route.params,
        gmReason: ''
      }
    },
    async created () {
      let params = this.$route.query
      this.fetchChangeRequest(params)
      await this.getChangeRequest
    }
  }
</script>

<style scoped>
  #changeRequest {
    margin-top: 5%;
    border-style: solid;
    border-radius: 5px;
    border-color: gray;
  }
  #request {
    width:auto;
    margin: 0 auto;
    padding: 20px;
  }
</style>
