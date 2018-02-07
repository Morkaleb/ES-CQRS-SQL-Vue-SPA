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
          <div style="width: 50%; margin: 0 auto;">
            <button
              @click="approveChange(request)"
              class="button is-info" >
              Approve
            </button>
            <button
              style="margin-left: 20px"
              @click="rejectChange(request)"
              class="button is-warning">
              Reject
            </button>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
  import { mapActions, mapGetters } from 'vuex'
  export default {
    methods: {
      ...mapActions([
        'fetchChangeRequest',
        'payrollAcceptShiftChange',
        'gmRejectShiftChange'
      ]),
      approveChange (request) {
        let approval = {GMId: 1, Request: request}
        this.payrollAcceptShiftChange(approval)
      },
      rejectChange (request) {
        let approval = {GMId: 1, Request: request}
        this.gmRejectShiftChange(approval)
      }
    },
    computed: {
      ...mapGetters([
        'getChangeRequest'
      ])
    },
    data () {
      return {
        Id: this.$route.params
      }
    },
    created () {
      let params = this.$route.query
      this.fetchChangeRequest(params)
    }
  }
</script>

<style scoped>
  #changeRequest {
    margin-top: 10%;
    border-style: solid;
    border-radius: 5px;
    border-color: gray;
  }
  #request {
    width:auto;
    margin: 0 auto;
    padding: 20px;
  }
  p {
    font-size: px;
  }
</style>
