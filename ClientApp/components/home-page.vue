<template>
    <div>
        Welcome {{getUser.firstName}} {{getUser.LastName}}
    </div>
</template>

<script>
    import { mapActions, mapGetters } from 'vuex'
export default {
        data() {
            return {
                user: {}
            }
        },
        methods: {
            ...mapActions([
                'fetchLoggedInUser'
            ])
        },
        computed: {
            ...mapGetters([
                'getUser'
            ])
        },
        created() {
            this.fetchLoggedInUser()
        }    
}
</script>

<style>
</style>
