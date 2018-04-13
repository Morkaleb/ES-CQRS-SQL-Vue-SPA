import Vue from 'vue'
import Vuex from 'vuex'
import VueResource from 'vue-resource'
import axios from 'axios'
import moment from 'moment'

Vue.use(Vuex)

var instance = axios.create({
    baseURL: 'http://localhost:8000',
    headers: { 'Access-Control-Allow-Origin': '' }
})


// STATE
const state = {
   
}
//GETTERS

const getters = {
}
// MUTATIONS
const mutations = {
}



// ACTIONS
const actions = ({
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


