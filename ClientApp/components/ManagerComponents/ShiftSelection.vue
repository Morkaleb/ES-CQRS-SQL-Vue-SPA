<template>
  <div class="type">
    <input type="checkbox" @click="clicked()"> {{this.shiftDescription}}
  </div>
</template>

<script>
    export default {
    name: 'shift',
    props: ['ShiftCode', 'shiftDescription'],
    methods: {
    clicked () {
    this.selected = !this.selected
    if (this.selected) {
    this.$emit('AddShiftCode', { shiftCode:this.ShiftCode, ShiftDescription:this.shiftDescription })
    } else {
    this.$emit('RemoveShiftCode', { shiftCode: this.ShiftCode, ShiftDescription: this.shiftDescription })
    }
    }
    },
    data () {
    return {
    selected: false
    }
    }
    }
</script>

<style scoped>
    .type{
        font-size:xx-small !important;
    }
</style>
