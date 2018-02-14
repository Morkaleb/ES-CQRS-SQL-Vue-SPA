<template>
    <div>
       <br/><hr /><hr />
        Welcome {{getUser.managerName}}
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
        created(){
            this.fetchLoggedInUser(window.localStorage["Auth-Token"])
            this.user = this.getUser()
        }    
}
</script>

<style>
</style>
