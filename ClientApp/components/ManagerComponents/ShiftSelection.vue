<template>
  <div>
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
            this.$emit('AddShiftCode', this.ShiftCode)
          } else {
            this.$emit('RemoveShiftCode', this.ShiftCode)
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

</style>
