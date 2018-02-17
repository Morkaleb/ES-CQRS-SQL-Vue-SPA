import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import axios from 'axios'
import moment from 'moment'

Vue.use(Vuex)

var instance = axios.create({
    baseURL: 'http://localhost:8000/api/',
    headers: { 'Access-Control-Allow-Origin': 'http://192.168.0.37:8001' }
})

// STATE
const state = {
    schedule: [],
    managers: [],
    shiftCodes: [],
    changeRequest: {},
    week: [],
    loggedInUser: {},
    shiftRequirements: {}
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
    },
    getUser: state => {
        return state.loggedInUser
    },
    getShiftRequirements: state => {
        return state.shiftRequirements
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
    },
    setLoggedInUser: (state, payload) => {
        state.loggedInUser = payload
    },
    setShiftRequirements: (state, payload) => {
        state.shiftRequirements = payload
    }
}

// ACTIONS
const actions = ({
    checkAuth: () => {
        let token = window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1]
        instance.get('http://localhost:8001/api/Auth/checkToken/?token=' + token)
            .then((res) =>{
            })
    },
    fetchSchedule: ({ commit }, payload) => {
        let managerSchedule = []
        let storeNumber = payload
        instance.get('http://localhost:8000/api/r/CalendarPage/?LocationId=' + storeNumber)
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
       instance.get('http://localhost:8000/api/r/ShiftStatusTable')
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
    fetchManagers: ({ commit }, payload) => {
        var managerList = []
        let storeNumber = payload
        instance.get('http://localhost:8000/api/r/ManagerTable/?locationId='+ storeNumber)
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
       instance.post('http://localhost:8000/api/Schedule/set', payload)
            .then((res) => {
            })
    },
    submitShiftChange: ({ commit }, newshift) => {
       instance.post('http://localhost:8000/api/Schedule/changeDay', newshift)
            .then((res) => {
            })
    },
    fetchChangeRequest: ({ commit }, payload) => {
        let changeRequest = {}
        let requestString = stringifyRequest(payload)
        return instance.get('http://localhost:8000/api/r/ChangeRequestsTable' + requestString)
            .then((response) => {
                changeRequest = response.data
                commit('setChangeRequest', changeRequest)
            })
    },
    gmAcceptShiftChange: ({ commit }, payload) => {
        instance.post('http://localhost:8000/api/Schedule/GMApproveChange', payload)
            .then((res) => {
            })
    },
    payrollAcceptShiftChange: ({ commit }, payload) => {
        instance.post('http://localhost:8000/api/Schedule/PayrollApproveChange', payload)
            .then((res) => {
            })
    },
    gmRejectShiftChange: ({ commit }, payload) => {
        instance.post('http://localhost:8000/api/Schedule/GMRejectChange', payload)
            .then((res) => {
            })
    },
    fetchWeek: ({ commit }, payload) => {
        let requestString = stringifyRequest(payload)
        return instance.get('http://localhost:8000/api/r/WeeklyCalendarPage' + requestString)
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
        instance.post('http://localhost:8000/api/Schedule/approveSchedule', payload)
            .then((res) => {
            })
    },
    submitDailyShiftRequirements: ({ commit }, payload) => {
        for (var i = 0; i < 7; i++) {
            var shifts = {
                Id: 104,
                GmId: 1,
                ShiftTypes: payload,
                DayOfTheWeek: i
            }
            instance.post('http://localhost:8000/api/Restaurant/shiftrequirements', shifts)
                .then((res) => {
                })
        }
    },
    gmRejectSchedule: ({ commit }, payload) => { },
    fetchLoggedInUser: ({ commit }, payload) => {
        var user = {}
        let token = window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1]
        instance.get('http://192.168.0.37:8001/api/auth/checkToken/?token=' + token)
            .then( (index) => {
                if (index.data != -1) {
                    user = instance.get('http://localhost:8000/api/r/ManagerTable/?Id=' + index.data)
                        .then((user) => {
                            commit('setLoggedInUser', user.data[0])
                        })
                }
            })
    },
    fetchDailyShiftRequirements: ({ commit }, payload) => {
        instance.get('https://localhost:8000/api/r/LocationDailyShiftRequirements/?Id=' + payload)
            .then((res) => {
                commit('setShiftRequirements', res.data[0])
            })
    }   
})

const store = new Vuex.Store({
    state,
    mutations,
    actions,
    getters
});

export default store;

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
