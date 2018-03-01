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

let preMutatedShiftsReq = {
    Monday: [],
    Tuesday: [],
    Wednesday: [],
    Thursday: [],
    Friday: [],
    Saturday: [],
    Sunday: []
}

// STATE
const state = {
    schedule: [],
    managers: [],
    shiftCodes: [],
    changeRequest: {},
    week: [],
    loggedInUser: {},
    shiftRequirements: {
        Monday: [],
        Tuesday: [],
        Wednesday: [],
        Thursday: [],
        Friday: [],
        Saturday: [],
        Sunday: []
    },
    preSubmittedShiftsReq: {
        Monday: [],
        Tuesday: [],
        Wednesday: [],
        Thursday: [],
        Friday: [],
        Saturday: [],
        Sunday: []
    },
    managerDays: []
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
    },
    getUnSubShiftReq: state => {
        return state.preSubmittedShiftsReq
    },
    getManagerDays: state => {
        return state.managerDays
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
    },
    setPrepShiftRequirements: (state, payload) => {
        state.preSubmittedShiftsReq = payload
    },
    setManagerDays: (state, payload) => {
        state.managerDays = payload
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
        let theManagerDays = []
        let storeNumber = payload
        console.log(document.cookie)
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
                            YOUR_DATA: {
                                ManagerId: schedule[shift].managerId,
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
                        statusId: code.statusId,
                        description: code.description,
                        daysToOwe: code.daysToOwe,
                        shiftStatus: code.shiftStatus,
                        id: code.Id
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
                let theseManagerDays = []
                for (let i = 7; i > 0; i--) {
                    let day = {
                        date: moment(payload.eow).subtract(i - 1, 'day').format("MM-DD-YYYY"),
                        shifts:[]
                    }
                    weeklist.push(day)
                }
                console.log(data)
                if (data) {
                    for (var day in data.days) {
                        weeklist[weeklist.findIndex(d => d.date == data.days[day].date)].shifts = data.days[day].shifts
                        for (var shift in data.days[day].shifts) {
                            let index = theseManagerDays.findIndex(m => m.name == data.days[day].shifts[shift].managerName)
                            if (index === -1) {
                                theseManagerDays.push({ name: data.days[day].shifts[shift].managerName, shifts: 1 })
                            } else {
                                theseManagerDays[index].shifts++
                            }
                        }
                    }
                }
                commit('setWeek', weeklist)
                commit('setManagerDays', theseManagerDays)
               
            })
    },
    approveSchedule: ({ commit }, payload) => {
        instance.post('http://localhost:8000/api/Schedule/approveSchedule', payload)
            .then((res) => {
            })
    },
    submitDailyShiftRequirements: ({ commit }, payload) => {
        let submittal = {
            GmId: state.loggedInUser.id,
            Id: state.loggedInUser.locationId,
            Monday: [],
            Tuesday: [],
            Wednesday: [],
            Thursday: [],
            Friday: [],
            Saturday: [],
            Sunday: [],
        }
        state.preSubmittedShiftsReq.Monday.forEach( (shift) => {
            submittal.Monday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Tuesday.forEach((shift) => {
            submittal.Tuesday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Wednesday.forEach((shift) => {
            submittal.Wednesday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Thursday.forEach((shift) => {
            submittal.Thursday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Friday.forEach((shift) => {
            submittal.Friday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Saturday.forEach((shift) => {
            submittal.Saturday.push(shift.shiftCode)
        })
        state.preSubmittedShiftsReq.Sunday.forEach((shift) => {
            submittal.Sunday.push(shift.shiftCode)
        })
        instance.post('http://localhost:8000/api/Restaurant/shiftrequirements', submittal)
                .then((res) => {
                    setTimeout(() => {
                        instance.get('http://localhost:8000/api/r/LocationDailyShiftRequirements/?locationId=' + state.loggedInUser.locationId)
                        .then((response) => {
                        })
                    }, 300)
                })
    },    
    gmRejectSchedule: ({ commit }, payload) => { },
    fetchLoggedInUser: ({ commit }, payload) => {
        try {
            var user = {}
            let token = window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1]
            return instance.get('http://localhost:8001/api/auth/checkToken/?token=' + token)
                .then((index) => {
                    if (index.data != -1) {
                        return instance.get('http://localhost:8000/api/r/ManagerTable/?Id=' + index.data)
                            .then((user) => {
                                commit('setLoggedInUser', user.data[0])
                            })
                    }
                })
        } catch (e) {
            console.log(e)
        }
    },
    fetchDailyShiftRequirements: ({ commit }, payload) => {
        instance.get('https://localhost:8000/api/r/LocationDailyShiftRequirements/?Id=' + payload)
            .then((res) => {
                commit('setShiftRequirements', res.data[0])
            })
    },
    addShiftToDay: ({ commit }, payload) => {
        if (payload.day == "Monday") {
            preMutatedShiftsReq.Monday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Tuesday") {
            preMutatedShiftsReq.Tuesday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Wednesday") {
            preMutatedShiftsReq.Wednesday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Thursday") {
            preMutatedShiftsReq.Thursday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Friday") {
            preMutatedShiftsReq.Friday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Saturday") {
            preMutatedShiftsReq.Saturday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        if (payload.day == "Sunday") {
            preMutatedShiftsReq.Sunday.push({ shiftCode: payload.shift.shiftCode, shiftDescription: payload.shift.ShiftDescription })
        }
        commit('setPrepShiftRequirements', preMutatedShiftsReq)
    },
    removeShiftFromDay: ({ commit }, payload) => {
        if (payload.day == "Monday") {
            let index = preMutatedShiftsReq.Monday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Monday.splice(index, 1)
        }
        if (payload.day == "Tuesday") {
            let index = preMutatedShiftsReq.Tuesday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Tuesday.splice(index, 1)
        }
        if (payload.day == "Wednesday") {
            let index = preMutatedShiftsReq.Wednesday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Wednesday.splice(index, 1)
        }
        if (payload.day == "Thursday") {
            let index = preMutatedShiftsReq.Thursday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Thursday.splice(index, 1)
        }
        if (payload.day == "Wednesday") {
            let index = preMutatedShiftsReq.Wednesday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Wednesday.splice(index, 1)
        }
        if (payload.day == "Thursday") {
            let index = preMutatedShiftsReq.Thursday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Thursday.splice(index, 1)
        }
        if (payload.day == "Friday") {
            let index = preMutatedShiftsReq.Friday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Friday.splice(index, 1)
        }
        if (payload.day == "Saturday") {
            let index = preMutatedShiftsReq.Saturday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Saturday.splice(index, 1)
        }
        if (payload.day == "Sunday") {
            let index = preMutatedShiftsReq.Sunday.findIndex(s => s == payload.shiftCode)
            preMutatedShiftsReq.Sunday.splice(index, 1)
        }        
        commit('setPrepShiftRequirements', preMutatedShiftsReq)
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


