import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import axios from 'axios'
import moment from 'moment'

Vue.use(Vuex)

var instance = axios.create({
    baseURL: 'http://192.168.0.37:8000/api/',
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
    managerDays: [],
    historicalVacation: [],
    weeklyRequirements:{},
    missingShifts:{}
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
    },
    getVacationHistory: state => {
        return state.historicalVacation
    },
    getWeeklyRequirements: state => {
        return state.weeklyRequirements
    },
    getMissingShifts: state => {
        return state.missingShifts
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
    },
    setVacationHistory: (state, payload) => {
        state.historicalVacation = payload
    },
    setRequiredShifts: (state, payload) => {
        state.weeklyRequirements = payload
    },
    setMissingShifts: (state, payload) =>
    {
        state.missingShifts = payload
    }
}



// ACTIONS
const actions = ({
    checkAuth: () => {
        let token = window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1]
        instance.get('http://192.168.0.37:8001/api/Auth/checkToken/?token=' + token)
            .then((res) =>{
            })
    },
    fetchSchedule: ({ commit }, payload) => {
        let managerSchedule = []
        let theManagerDays = []
        let storeNumber = payload
        instance.get('http://192.168.0.37:8000/api/r/CalendarPage/?LocationId=' + storeNumber)
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
       instance.get('http://192.168.0.37:8000/api/r/ShiftStatusTable')
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
    fetchChangeRequest: ({ commit }, payload) => {
        let changeRequest = {}
        let requestString = stringifyRequest(payload)
        return instance.get('http://192.168.0.37:8000/api/r/ChangeRequestsTable' + requestString)
            .then((response) => {
                changeRequest = response.data
                commit('setChangeRequest', changeRequest)
            })
    },
    fetchManagers: ({ commit }, payload) => {
        var managerList = []
        let storeNumber = payload
        instance.get('http://192.168.0.37:8000/api/r/ManagerTable/?locationId='+ storeNumber)
            .then((response) => {
                let data = response.data
                for (let person in data) {
                    let human = data[person]
                    var manager = {
                        Name: human.firstName + " " + human.lastName,
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
    fetchLoggedInUser: ({ commit }, payload) => {
        try {
            var user = {}
            let retrievedToken = document.cookie
            var token = retrievedToken.split('=')[1]
            return instance.get('http://192.168.0.37:8001/api/auth/checkToken/?token=' + token)
                .then((index) => {
                    if (index.data != -1) {
                        return instance.get('http://192.168.0.37:8000/api/r/ManagerTable/?Id=' + index.data)
                            .then((user) => {
                                commit('setLoggedInUser', user.data[0])
                            })
                    }
                    if(index.data == -1) window.location.href = 'http://192.168.0.37:8001'
                })
        } catch (e) {
            console.log(e)
        }
    },
    fetchDailyShiftRequirements: ({ commit }, payload) => {
        instance.get('http://192.168.0.37:8000/api/r/LocationDailyShiftRequirements/?Id=' + payload)
            .then((res) => {
                if (res.data[0].monday) {
                    commit('setShiftRequirements', res.data[0])
                }
            })
    },
    fetchWeek: ({ commit }, payload) => {
        let requestString = stringifyRequest(payload)
        return instance.get('http://192.168.0.37:8000/api/r/WeeklyCalendarPage' + requestString)
            .then((response) => {
                let data = response.data[0]
                let weeklist = []
                let theseManagerDays = []
                for (var manager in store.state.managers) {
                    theseManagerDays.push({
                        name: store.state.managers[manager].Name,
                        shifts: 0,
                        daysToOwe:0
                    })
                }
                for (let i = 7; i > 0; i--) {
                    let day = {
                        date: moment(payload.eow, "MM-DD-YYYY").subtract(i - 1, 'day').format("MM-DD-YYYY"),
                        shifts: []
                    }
                    weeklist.push(day)
                }
                if (data) {
                    for (var day in data.days) {
                        weeklist[weeklist.findIndex(d => d.date == data.days[day].date)].shifts = data.days[day].shifts
                        for (var shift in data.days[day].shifts) {
                            let index = theseManagerDays.findIndex(m => m.name == data.days[day].shifts[shift].managerName)
                            var owed = 0
                            if (data.days[day].shifts[shift].shiftCode != null) {
                                if (data.days[day].shifts[shift].shiftCode.split(" ")[1] == "(Owed)") {
                                    owed = 1
                                }
                            }
                            if (index === -1) {
                                theseManagerDays.push({
                                    name: data.days[day].shifts[shift].managerName,
                                    shifts: 1,
                                    daysToOwe: owed
                                })
                            } else {
                                theseManagerDays[index].shifts++
                                theseManagerDays[index].daysToOwe += owed
                            }
                        }
                    }
                }
                commit('setWeek', weeklist)
                commit('setManagerDays', theseManagerDays)
                //commit('setMissingShifts', data.missingShifts)
            })
    },
    fetchVacationHistory: ({ commit }, payload) => {
        instance.get('http://192.168.0.37:8000/api/r/HistoricalVacationTable/?locationId=' + payload)
            .then((res) => {
                commit('setVacationHistory', res.data)
            })
    },
    fetchRequiredShifts: ({ commit }, payload) => {
        instance.get("http://192.168.0.37:8000/api/r/LocationDailyShiftRequirements/?id=" + payload)
            .then((res) => {
                commit('setRequiredShifts', res.data[0])
            })
    },
    submitNewShift: ({ commit }, payload) => {
        instance.post('http://192.168.0.37:8000/api/Schedule/set',  payload)
           .then((res) => {
            })
    },
    submitShiftChange: ({ commit }, newshift) => {
        console.log(newshift)
       instance.post('http://192.168.0.37:8000/api/Schedule/changeDay', newshift)
            .then((res) => {
            })
    },
    gmAcceptShiftChange: ({ commit }, payload) => {
        instance.post('http://192.168.0.37:8000/api/Schedule/GMApproveChange', payload)
            .then((res) => {
            })
    },
    payrollAcceptShiftChange: ({ commit }, payload) => {
        instance.post('http://192.168.0.37:8000/api/Schedule/PayrollApproveChange', payload)
            .then((res) => {
            })
    },
    gmRejectShiftChange: ({ commit }, payload) => {
        instance.post('http://192.168.0.37:8000/api/Schedule/GMRejectChange', payload)
            .then((res) => {
            })
    },
    approveSchedule: ({ commit }, payload) => {
        instance.post('http://192.168.0.37:8000/api/Schedule/approveSchedule', payload)
            .then((res) => {
            })
    },
    submitDailyShiftRequirements: ({ commit }, payload) => {
        console.log(submittal)
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
        instance.post('http://192.168.0.37:8000/api/Restaurant/shiftrequirements', submittal)
                .then((res) => {
                    setTimeout(() => {
                        instance.get('http://192.168.0.37:8000/api/r/LocationDailyShiftRequirements/?locationId=' + state.loggedInUser.locationId)
                        .then((response) => {
                        })
                    }, 300)
                })
    },
    gmRejectSchedule: ({ commit }, payload) => { },
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
    },
    checkShiftRequirements:({commit}, payload) =>{
        let missingShifts = {
                monday:[],
                tuesday:[],
                wednesday:[],
                thursday:[],
                friday:[],
                satruday:[],
                sunday:[]
            }
        var requirements = state.weeklyRequirements
        var schedule = state.week
        for(shift in requirements.monday) {
            let index = schedule[0].shifts.indexOf(s => s.shiftCode ===  requirements.monday[shift])
            if( index === -1) {
                missingShifts.monday.push(requirements.monday[shift])
            }
        }
        for(shift in requirements.tuesday) {
            let index = schedule[1].shifts.indexOf(s => s.shiftCode ===  requirements.tuesday[shift])
            if(index === -1) {
                missingShifts.tuesday.push(requirements.tuesday[shift])
            }
        }
        for(shift in requirements.wednesday) {
            if(schedule[2].shifts.indexOf(s => s.shiftCode ===  requirements.wednesday[shift]) === -1) {
                missingShifts.wednesday.push(requirements.wednesday[shift])
            }
        }
        for(shift in requirements.thursday) {
            if(schedule[3].shifts.indexOf(s => s.shiftCode ===  requirements.thursday[shift]) === -1) {
                missingShifts.thursday.push(requirements.thursday[shift])
            }
        }
        for(shift in requirements.friday) {
            if(schedule[4].shifts.indexOf(s => s.shiftCode ===  requirements.friday[shift]) === -1) {
                missingShifts.friday.push(requirements.friday[shift])
            }
        }
        for(shift in requirements.satruday) {
            if(schedule[5].shifts.indexOf(s => s.shiftCode ===  requirements.satruday[shift]) === -1) {
                missingShifts.satruday.push(requirements.satruday[shift])
            }
        }
        for(shift in requirements.sunday) {
            if(schedule[6].shifts.indexOf(s => s.shiftCode ===  requirements.sunday[shift]) === -1) {
                missingShifts.sunday.push(requirements.sunday[shift])
            }
        }
        commit('setMissingShifts', missingShifts)
    },
    download: ({ commit }, payload) => {
        window.location.href = 'http://192.168.0.37:8000/api/Schedule/download/' + request
    },
    exportMonth: ({ commit }, payload) => {
        window.location.href = 'http://192.168.0.37:8000/api/Excel/scedule'
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


