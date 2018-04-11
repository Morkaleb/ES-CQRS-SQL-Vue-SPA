import HomePage from 'components/home-page'
import MonthlyCalendar from 'components/Calendar/MonthlyCalendar'
import GMApproveSchedule from 'components/Scheduling/GMApproveSchedule'
import PayrollApproval from 'components/Scheduling/PayrollApproval'
import ReviewScheduleChange from './components/Scheduling/ReviewScheduleChange'
import DailyShiftRequirements from './components/ManagerComponents/DailyShiftRequirements'
import ShiftRequirementsWeek from './components/ManagerComponents/ShiftRequirementsWeek'
import GmPage from './components/ManagerComponents/GmPage'
import payRoll from './components/Scheduling/payRoll'
import schedulingPages from './components/schedulingPages'
import { store, actions } from './store/index'
import ChangeRequests from './components/Scheduling/scheduleChangeRequests'
import axios from 'axios'

export const routes = [
    { path: '/ops/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/ops/schedule', component: schedulingPages, display: 'Monthly Schedule', beforeEnter: checkAuth },
    { path: '/ops/monthlyschedule', component: MonthlyCalendar, display: 'Monthly Schedule', beforeEnter: checkAuth },
    { path: '/ops/weeklySchedule', component: GMApproveSchedule, display: 'GM Approval', beforeEnter: checkAuth },
    { path: '/ops/payroll', component: payRoll, display: "Payroll", beforeEnter: checkAuth },
    { path: '/ops/payrollApproval', component: PayrollApproval, display: 'Payroll Approval', beforeEnter: checkAuth },
    { path: '/ops/reviewScheduleChange', component: ReviewScheduleChange, display: 'Review Schedule Change Requeast', beforeEnter: checkAuth },
    { path: '/ops/dailyShiftRequirements', component: ShiftRequirementsWeek, display: 'Set restaurant requirements', beforeEnter: checkAuth },
    { path: '/ops/schedulingPages', component: schedulingPages, display: 'Restaurant Management', beforeEnter: checkAuth },
    { path: '/ops/scheduleChangeRequests', component: ChangeRequests, display:'Schedule Change Requests', beforeEnter:checkAuth}
]

var instance = axios.create({
    baseURL: 'https://wsbis.whitespotonline.com:4443/ops/ops/api/',
    headers: { 'Access-Control-Allow-Origin': 'https://wsbis.whitespotonline.com:4443' }
})

function checkAuth(to, from, next) {
    let retrievedToken = document.cookie
    if (retrievedToken[0] != "AuthToken") {
        var token = retrievedToken.split('=')[1]
        instance.get('https://wsbis.whitespotonline.com:4443/signIn/api/auth/checkToken/?token=' + token)
            .then((res) => {
                if (res.data == -1) {                   
                    window.sessionStorage.setItem('lastPage', window.location.href)
                    window.location.href = 'https://wsbis.whitespotonline.com:4443/signIn'
                } else next()
            })
    }
    else {
        window.sessionStorage.setItem('lastPage', window.location.href)
        window.location.href = 'https://wsbis.whitespotonline.com:4443/'
    }   
}

function preload() {
    this.actions.fetchLoggedInUser()
        .then(() => {
            actions.fetchRequiredShifts(this.store.state.loggedInUser.locationId)
        })
        .then(() => {
            actions.fetchWeek()
        })
}
