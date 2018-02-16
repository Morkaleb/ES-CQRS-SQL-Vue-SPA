import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import MonthlyCalendar from 'components/calendar/monthly-calendar'
import GMApproveSchedule from 'components/Scheduling/GMApproveSchedule'
import PayrollApproval from 'components/Scheduling/PayrollApproval'
import ReviewScheduleChange from './components/Scheduling/ReviewScheduleChange'
import DailyShiftRequirements from './components/ManagerComponents/DailyShiftRequirements'
import { store } from './store/index'
import axios from 'axios'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/counter', component: CounterExample, display: 'Counter', style: 'glyphicon glyphicon-education' },
    { path: '/fetch-data', component: FetchData, display: 'Fetch data', style: 'glyphicon glyphicon-th-list' },
    { path: '/schedule', component: MonthlyCalendar, display: 'Monthly Schedule', beforeEnter: checkAuth},
    { path: '/approveSchedule', component: GMApproveSchedule, display: 'GM Approval', beforeEnter: checkAuth},
    { path: '/payrollApproval', component: PayrollApproval, display: 'Payroll Approval', beforeEnter: checkAuth},
    { path: '/reviewScheduleChange', component: ReviewScheduleChange, display: 'Review Schedule Change Requeast', beforeEnter: checkAuth},
    { path: '/dailyShiftRequirements', component: ReviewScheduleChange, display: 'Set restaurant requirements', beforeEnter: checkAuth}
]

var instance = axios.create({
    baseURL: 'http://localhost:8000/api/',
    headers: { 'Access-Control-Allow-Origin': 'http://192.168.0.37:8001' }
})

function checkAuth(to, from, next) {
    let token = window.localStorage.getItem("Auth-Token").split(':')[1].split('"')[1]
    if (token) {
        instance.get('http://localhost:8001/api/Auth/checkToken/?token=' + token)
            .then((res) => {
                if (res.data == -1) {
                    console.log(res.data)
                    window.location('http://localhost:8001')
                }
                else {
                    console.log(res.data)
                    next();
                }
            })
    } else {
        console.log('hitter')
        window.location('http://localhost:8001')
    }
}
