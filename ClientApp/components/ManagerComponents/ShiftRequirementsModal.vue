<template>
    <transition name="modal">
        <div class="modal-mask">
            <div class="modal-wrapper">
                <div class="modal-container">

                    <div class="modal-header">
                         Is this all correct?                      
                    </div>

                    <div class="modal-body">
                        <div v-for="day in Object.keys(getUnSubShiftReq)" class="dayCard">{{day}}: <ul>
                                     <li v-for="shift in getUnSubShiftReq[day]">{{shift.shiftDescription}}</li>
                                </ul></div>
                    </div>

                    <div class="modal-footer">
                        And a couple things in here
                            <button class="modal-default-button" @click="$emit('submit')"
                                    style="background-color:orangered">
                                Submit</button>
                            <button class="modal-default-button" @click="$emit('close')"
                                    style="background-color:#015351; color:white">
                                Close
                            </button>
                       
                    </div>
                </div>
            </div>
        </div>
    </transition>
</template>
<script>
    import { mapActions, mapGetters } from 'vuex'

    export default {
        name: 'ShiftRequirementsModal',
        computed: {
            ...mapGetters([
                'getUnSubShiftReq'
            ])
        },
        props: [],
        data () {
          return {
          }
        }
  }
</script>
<style>
    .dayCard{
        float:left;
        border:1px solid black;
        border-radius:5px;
        width: 30%;
        height: 20vh !important;
        padding: 2px;
        margin:2px;
        vertical-align:central;
        overflow-y:scroll;
    }
     .modal-mask {
         position: fixed;
         z-index: 9998;
         top: 0;
         left: 0;
         width: 100%;
         height: 100%;
         background-color: rgba(0, 0, 0, .5);
         display: table;
         transition: opacity .3s ease;
     }

     .modal-wrapper {
         display: table-cell;
         vertical-align: middle;
     }

     .modal-container {
         width: 70%;
         margin: 0px auto;
         padding: 20px 30px;
         background-color: #fff;
         border-radius: 2px;
         box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
         transition: all .3s ease;
         font-family: Helvetica, Arial, sans-serif;
         font-size:small;
     }
    .modal-footer{
        clear: both;
        padding-left:40%;
    }
     .modal-header h3 {
         margin-top: 0;
         color: #42b983;
     }

     .modal-body {
         margin: 20px 0;
     }

     .modal-default-button {
         border-radius:5px;
         margin:2px;
         padding:2px;         
         float:left;
     }

     /*
    * The following styles are auto-applied to elements with
    * transition="modal" when their visibility is toggled
    * by Vue.js.
    *
    * You can easily play with the modal transition by editing
    * these styles.
    */

     .modal-enter {
         opacity: 0;
     }

     .modal-leave-active {
         opacity: 0;
     }

         .modal-enter .modal-container,
         .modal-leave-active .modal-container {
             -webkit-transform: scale(1.1);
             transform: scale(1.1);
         }
</style>
