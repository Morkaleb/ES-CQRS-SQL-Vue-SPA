import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import axios from 'axios'

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
    schedule: [],
    managers: [],
    shiftCodes: [],
    changeRequest: {},
    week: []
}
//GETTERS

const getters = {
    getSchedule: state => {
        return state.schedule
    },
    getManagers: state => {
        return state.managers
    },
    getShiftCodes: state => {
        return state.shiftCodes
    },
    getChangeRequest: state => {
        return state.changeRequest
    },
    getWeek: state => {
        return state.week
    }
}
// MUTATIONS
const mutations = {
    setSchedule: (state, payload) => {
        state.schedule = payload
    },
    setManagers: (state, payload) => {
        state.managers = payload
    },
    setShiftCodes: (state, payload) => {
        state.shiftCodes = payload
    },
    setChangeRequest: (state, payload) => {
        state.changeRequest = payload
    },
    setWeek: (state, payload) => {
        state.week = payload
    }
}

// ACTIONS
const actions = ({
    fetchSchedule: ({ commit }, payload) => {
        let managerSchedule = []
        let storeNumber = payload
        axios.get('http://localhost:8000/api/r/CalendarPage/?LocationId=' + storeNumber)
            .then((response) => {
                console.log('hit')
                console.log(response)
                let data = response.data
                for (var day in data) {
                    var schedule = data[day].schedule
                    for (var shift in schedule) {
                        var theShift = {
                            title: schedule[shift].managerName + ' ' + schedule[shift].shiftCode,
                            EOW: data[day].eow,
                            start: data[day].shiftdate,
                            end: data[day].shiftdate,
                            class: schedule[shift].shiftCode
                        }
                        managerSchedule.push(theShift)
                    }
                }
                commit('setSchedule', managerSchedule)
            })
    },
    fetchShiftCodes: ({ commit }) => {
        let shiftCodes = []
        axios.get('http://localhost:8000/api/r/ShiftStatusTable')
            .then((response) => {
                console.log('hit2')
                console.log()
                let data = response.data
                for (var shift in data) {
                    var code = data[shift]
                    let shiftCode = {
                        code: code.statusId,
                        description: code.description
                    }
                    shiftCodes.push(shiftCode)
                }
                commit('setShiftCodes', shiftCodes)
            })
    },
    fetchManagers: ({ commit }) => {
        var managerList = []
        axios.get('http://localhost:8000/api/r/ManagerTable')
            .then((response) => {
                console.log('hit3')
                console.log(response)
                let data = response.data
                for (let person in data) {
                    let human = data[person]
                    var manager = {
                        Name: human.managerName,
                        Id: human.managerId,
                        EmailAddress: human.emailAddress,
                        Role: human.role
                    }
                    managerList.push(manager)
                }
                var Cancellar = {
                    Name: 'Cancel Shift',
                    Id: 'Cancel Shift'
                }
                managerList.push(Cancellar)
                commit('setManagers', managerList)
            })
    }
})

export default new Vuex.Store({
    state,
    mutations,
    actions,
    getters
});
