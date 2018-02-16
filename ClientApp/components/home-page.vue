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
            this.fetchLoggedInUser(window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1])
        }    
}
</script>

<style>
</style>
