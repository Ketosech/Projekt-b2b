import Vue from 'vue'
import App from './App.vue'
import VueCookies from 'vue-cookies'
import axios from 'axios'
import store from './store'
import router from './router'

Vue.prototype.$http = axios;

Vue.config.productionTip = false

Vue.use(VueCookies)

new Vue({
  store,
  router,
  render: h => h(App)
}).$mount('#app')
