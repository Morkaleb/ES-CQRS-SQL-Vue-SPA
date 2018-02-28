<template>
  <section class="card">
      <div class="card-content">
          <strong>Things To Consider</strong><hr />
          <div class="numberofshifts">
             <p v-for="manager in getManagerDays" v-if="manager.name != 'Cancel Shift'">
                     {{manager.name}}
                     <span v-if="manager.shifts < 5 || manager.shifts > 5" style="color:red">has {{manager.shifts}} shifts</span>
                     <span v-if="manager.shifts === 5">has 5 shifts</span>
              </p>
          </div>
          <div v-for="manager in this.consequences">
              <div v-if="manager.Role">
                  {{manager.Name}}
              </div>
          </div>
      </div>
  </section>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'

    export default {
        name: 'schedule-consequences',
        props: ['consequences'],
        data() {
            return {
                managerDays: [],
                schedule: []
            }
        },
        methods: {
            ...mapActions([]),
            getThisWeek () {
                this.schedule = this.$store.state.week
            }
        },
        computed: {
            ...mapGetters([
                'getWeek',
                'getManagerDays'
            ])
        },
        created() {
        }
    }
</script>

<style scoped>
  .card {
    border-radius: 5px;
    border: 1px solid darkgray;
    height: 500px;
  }
  strong {
    text-align: right;
  }
    .numberofshifts {
        border-radius:5px;
        padding:2px;
        margin-left: 5px;
        float:left;
        text-align:left;
        border:1px solid black;
    }
</style>
