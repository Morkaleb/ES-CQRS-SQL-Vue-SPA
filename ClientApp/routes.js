
import { store, actions } from './store/index'
import axios from 'axios'

export const routes = [
]

var instance = axios.create({
    baseURL: 'https:/localhost:8000',
    headers: { 'Access-Control-Allow-Origin': '' }
})
