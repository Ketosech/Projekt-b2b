<template>
  <div id="app">
    <navbar/>
    <div class="content">
      <router-view/>
    </div>
    <footerbar/>
  </div>
</template>

<script>
import navbar from '@/components/navbar.vue'
import footerbar from '@/components/footerbar.vue'

export default {
  name: 'App',
  components: {
    navbar,
    footerbar,
  },
  computed: {
    cartProducts () {
      return this.$store.getters.cartProducts;
    }
  },
  mounted: function mounted() {
    if (JSON.parse(this.$cookies.get('cart')) != '') {
      this.$store.dispatch('addToCart', JSON.parse(this.$cookies.get('cart')))
    }
  },
  watch: {
    cartProducts: {
      handler(){
        this.$cookies.set('cart', JSON.stringify(this.cartProducts));
      },
      deep: true
    },
  }
}
</script>

<style>
body, html{
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin:0px;
}
img{
  max-width: 100%;
}
h1,h2,h3,h4,h5,p{
  margin:0px;
}
a{
  text-decoration: none;
  color:inherit;
}
.content{
  min-height: 100vh;
}
</style>
