<template>
    <div class="nav-left">
        <slot name="body">
            default body
        </slot>
    </div>
</template>

<script>
    export default {
        name:"nav-left"
    }
</script>

<style>
    .nav-left {
        margin: 4% 0 0 0;
        padding: 0 0 0 0;
        position: absolute;
        height: 100%;
        width: 16.5%;
        background-color: #555;
    }
</style>
