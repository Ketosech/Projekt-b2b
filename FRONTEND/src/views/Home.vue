<template>
  <div class="home">
    <offers v-if="$cookies.get('user') == null || $cookies.get('user').permission == '0'"/>
    <adminoffers v-if="$cookies.get('user') != null && $cookies.get('user').permission == '1'"/>
  </div>
</template>

<script>
// @ is an alias to /src
import offers from '@/components/offers.vue';
import adminoffers from '@/components/admin-offers.vue'

export default {
  name: 'Home',
  components: {
    offers,
    adminoffers
  },
  created: function created() {
  }
}
</script>
