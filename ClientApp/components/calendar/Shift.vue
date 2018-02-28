<template>
    <div class="card" v-if="this.shiftType != 'Cancelled' && this.managerName !='Cancel Shift'" v-on:click="eventClicked()">
        <div style="padding:2px 2px;">Shift: {{shiftType}}</div>
        <div style="padding:2px 2px;">Manager: {{managerName}}</div>
    </div>
</template>

<script>
    export default {
        name: "shift",
        data() {
            return {}
        },
        props: [
            'shiftType',
            'managerName',
            'managerFromId',
            'date',
            'shiftCode'
        ],
        methods: {
            eventClicked() {
                let Event = {
                    name: this.managerName,
                    managerFromId: this.managerFromId,
                    Date: this.date,
                    shiftCode: this.shiftCode,
                    shiftDate:""
                }
                console.log(event)
                this.$emit('eventClicked', Event)
            }
        }

    }
</script>

<style>
    .card {
        margin: 6px 2px;
        border: 1px solid black;
        border-radius: 5px;
        font-size: small;
        padding: 2px;
    }
</style>
