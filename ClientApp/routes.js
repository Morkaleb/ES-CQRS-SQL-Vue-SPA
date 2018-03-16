import HomePage from 'components/home-page'
import MonthlyCalendar from 'components/calendar/monthly-calendar'
import GMApproveSchedule from 'components/Scheduling/GMApproveSchedule'
import PayrollApproval from 'components/Scheduling/PayrollApproval'
import ReviewScheduleChange from './components/Scheduling/ReviewScheduleChange'
import DailyShiftRequirements from './components/ManagerComponents/DailyShiftRequirements'
import ShiftRequirementsWeek from './components/ManagerComponents/ShiftRequirementsWeek'
import GmPage from './components/ManagerComponents/GmPage'
import payRoll from './components/Scheduling/payRoll'
import { store } from './store/index'
import axios from 'axios'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/schedule', component: MonthlyCalendar, display: 'Monthly Schedule', beforeEnter: checkAuth},
    { path: '/approveSchedule', component: GMApproveSchedule, display: 'GM Approval', beforeEnter: checkAuth },
    { path: '/payroll', component: payRoll, display: "Payroll", beforeEnder: checkAuth },
    { path: '/payrollApproval', component: PayrollApproval, display: 'Payroll Approval', beforeEnter: checkAuth},
    { path: '/reviewScheduleChange', component: ReviewScheduleChange, display: 'Review Schedule Change Requeast', beforeEnter: checkAuth},
    { path: '/dailyShiftRequirements', component: ShiftRequirementsWeek, display: 'Set restaurant requirements', beforeEnter: checkAuth },
    {path: '/GmPage', component: GmPage, display: 'Restaurant Management', beforeEnter: checkAuth }
]

var instance = axios.create({
    baseURL: 'http://192.168.0.37:8000/api/',
    headers: { 'Access-Control-Allow-Origin': 'http://192.168.0.37:8001' }
})

function checkAuth(to, from, next) {
    let retrievedToken = document.cookie
    if (retrievedToken[0] != "AuthToken") {
        var token = retrievedToken.split('=')[1]
        instance.get('http://192.168.0.37:8001/api/auth/checkToken/?token=' + token)
            .then((res) => {
                if (res.data == -1) {                   
                    window.sessionStorage.setItem('lastPage', window.location.href)
                    window.location.href = 'http://192.168.0.37:8001'
                } else next()
            })
    }
    else {
        window.sessionStorage.setItem('lastPage', window.location.href)
        window.location.href = 'http://192.168.0.37:8001'

    }
}
