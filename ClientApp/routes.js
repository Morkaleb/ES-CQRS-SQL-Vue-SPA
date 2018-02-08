import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import MonthlyCalendar from 'components/calendar/monthly-calendar'
import GMApproveSchedule from 'components/Scheduling/GMApproveSchedule'
import PayrollApproval from 'components/Scheduling/PayrollApproval'
import ReviewScheduleChange from './components/Scheduling/ReviewScheduleChange'
import DailyShiftRequirements from './components/ManagerComponents/DailyShiftRequirements'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/counter', component: CounterExample, display: 'Counter', style: 'glyphicon glyphicon-education' },
    { path: '/fetch-data', component: FetchData, display: 'Fetch data', style: 'glyphicon glyphicon-th-list' },
    { path: '/schedule', component: MonthlyCalendar, display: 'Monthly Schedule' },
    { path: '/approveSchedule', component: GMApproveSchedule, display: 'GM Approval' },
    { path: '/payrollApproval', component: PayrollApproval, display: 'Payroll Approval' },
    { path: '/reviewScheduleChange', component: ReviewScheduleChange, display: 'Review Schedule Change Requeast' },
    { path: '/dailyShiftRequirements', component: ReviewScheduleChange, display: 'Set restaurant requirements'}
]
