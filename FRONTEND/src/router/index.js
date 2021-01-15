import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../components/login.vue')
  },
  {
    path: '/register',
    name: 'Register',
    component: () => import('../components/register.vue')
  },
  {
    path: '/shopping-cart',
    name: 'Koszyk',
    component: () => import('../components/shopping-cart.vue')
  },
  {
    path: '/admin/offers',
    name: 'Admin Offers',
    component: () => import('../components/admin-offers.vue')
  },
  {
    path: '/admin/users',
    name: 'Admin Users',
    component: () => import('../components/admin-users.vue')
  },
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router
