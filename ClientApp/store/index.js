import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import axios from 'axios'
import moment from 'moment'

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
                let data = response.data
                for (var day in data) {
                    var schedule = data[day].schedule
                    for (var shift in schedule) {
                        var theShift = {
                            title: schedule[shift].managerName + ' ' + schedule[shift].shiftCode,
                            start: data[day].shiftdate,
                            end: data[day].shiftdate,
                            YOUR_DATA : {
                                class: schedule[shift].shiftCode,
                                EOW: data[day].eow,
                            }
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
    },
    submitNewShift: ({ commit }, payload) => {
        axios.post('http://localhost:8000/api/Schedule/set', payload)
            .then((res) => {
                console.log(res)
            })
    },
    submitShiftChange: ({ commit }, newshift) => {
        Vue.http.post('http://localhost:8000/api/Schedule/changeDay', newshift)
            .then((res) => {
                console.log(res)
            })
    },
    fetchChangeRequest: ({ commit }, payload) => {
        console.log(payload)
        let changeRequest = {}
        let requestString = stringifyRequest(payload)
        return axios.get('http://localhost:8000/api/r/ChangeRequestsTable' + requestString)
            .then((response) => {
                console.log(response)
                changeRequest = response.data
                commit('setChangeRequest', changeRequest)
            })
    },
    gmAcceptShiftChange: ({ commit }, payload) => {
        axios.post('http://localhost:8000/api/Schedule/GMApproveChange', payload)
            .then((res) => {
                console.log(res)
            })
    },
    payrollAcceptShiftChange: ({ commit }, payload) => {
        axios.post('http://localhost:8000/api/Schedule/PayrollApproveChange', payload)
            .then((res) => {
                console.log(res)
            })
    },
    gmRejectShiftChange: ({ commit }, payload) => {
        axios.post('http://localhost:8000/api/Schedule/GMRejectChange', payload)
            .then((res) => {
                console.log(res)
            })
    },
    fetchWeek: ({ commit }, payload) => {
        let requestString = stringifyRequest(payload)
        return axios.get('http://localhost:8000/api/r/WeeklyCalendarPage' + requestString)
            .then((response) => {
                let data = response.data[0]
                let weeklist = []
                for (var day in data.days) {
                    weeklist.push(data.days[day])
                }
                commit('setWeek', weeklist)
            })
    },
    approveSchedule: ({ commit }, payload) => {
        axios.post('http://localhost:8000/api/Schedule/approveSchedule', payload)
            .then((res) => {
                console.log(res)
            })
    },
    submitDailyShiftRequirements: ({ commit }, payload) => {
        for (var i = 0; i < 7; i++) {
            console.log(i)
            var shifts = {
                Id: 104,
                GmId: 1,
                ShiftTypes: payload,
                DayOfTheWeek: i
            }
            console.log(shifts)
            axios.post('http://localhost:8000/api/Restaurant/shiftrequirements', shifts)
                .then((res) => {
                    console.log(res)
                })
        }
    },
    gmRejectSchedule: ({ commit }, payload) => { }
})

export default new Vuex.Store({
    state,
    mutations,
    actions,
    getters
});


const stringifyRequest = (request) => {
    let requestArray = Object.keys(request)
        .map(key => '?' + key + '=' + request[key])
    if (requestArray.length > 1) {
        let returnable = ''
        for (let i = 0; i <= requestArray.length - 1; i++) {
            returnable += requestArray[i]
            if (i !== requestArray.length - 1) returnable += '&'
        } return [returnable]
    } else { return requestArray }
}
