<template>
    <div class="week">
        <DailyShiftRequirements class="day" day="Monday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Tuesday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Wednesday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Thursday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Friday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Saturday"></DailyShiftRequirements>
        <DailyShiftRequirements class="day" day="Sunday"></DailyShiftRequirements>
    </div>
</template>

<script>
    import DailyShiftRequirements from './DailyShiftRequirements'

    export default {
        name: 'GmPage',
        components: {
            DailyShiftRequirements
        }
    }
</script>

<style>
    .day{
        position:relative;
        float:left;
        width:14%;
    }
    .week{
        display:inline-block;
        width:100%;
    }
</style>
