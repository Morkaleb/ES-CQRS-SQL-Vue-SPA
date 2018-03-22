<template>
    <div class="column is-one-fifth" style="height:700px; background-color:#555; Color:white !important;">
        <div style="margin-top:20%; padding-top:20%;">
            <span>Select a start date</span>
            <b-field>
                <b-datepicker placeholder="Click to select..."
                              icon="calendar-today"
                              v-model="startDate">
                </b-datepicker>
            </b-field>
            <span>Select an end date</span>
            <b-field>
                <b-datepicker placeholder="Click to select..."
                              icon="calendar-today"
                              v-model="endDate">
                </b-datepicker>
            </b-field>
            <button class="btn btn-default"
                    style="padding:5px; margin-left:35%; margin-top:10px; vertical-align:central;"
                    v-on:click="this.downloadButton">
                Download
            </button>
        </div>
    </div>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'
    const thisMonth = new Date().getMonth()
    const thisYear = new Date().getFullYear()
    export default {
        data() {
            return {
                startDate: new Date(thisYear, thisMonth, 1),
                endDate: new Date(thisYear, thisMonth, 28)
            }
        },
        methods: {
            ...mapActions(['download']),
            downloadButton() {                
                let aStartDate = this.startDate.toString().split(' ')
                let stringgyDate1 = aStartDate[1] + "-" + aStartDate[2] + "-" + aStartDate[3]
                let anEndtDate = this.endDate.toString().split(' ')
                let stringgyDate2 = anEndtDate[1] + "-" + anEndtDate[2] + "-" + anEndtDate[3] 
                let payload = { startDate: stringgyDate1, endDate: stringgyDate2 }
                this.download(payload)
            }
        }
    }
</script>

<style>
    b-datepicker{
        margin: 5px;
        padding:5px;
        width:80%;
        float:left;
    }
    b-field{
        color:white;
    }
</style>
