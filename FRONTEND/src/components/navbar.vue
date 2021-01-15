<template>
  <div class="navbar">
    <div class="flex">
      <a href="/">
        <div class="navbar__logo">
          Nazwa test Logo 123
        </div>
      </a>
      <div id="nav">
        <router-link to="/admin/users" v-if="$cookies.get('user') != null && $cookies.get('user').permission == '1'">Modyfikacja&nbsp;użytkowników</router-link>
        <router-link to="/">Strona&nbsp;główna</router-link>
        <router-link to="/login" v-if="$cookies.get('user') == null">Logowanie</router-link>
        <router-link to="/shopping-cart" class="cart" v-if="$cookies.get('user') != null">Koszyk <div class="how-much" v-if="cartproducts.length >= 1">{{ cartproducts.length }}</div> </router-link>
        <a v-on:click="logout()" v-if="$cookies.get('user') != null">Wyloguj</a>
      </div>
    </div>
    <div class="login-name" v-if="$cookies.get('user') != null">
      Jesteś zalogowany jako: {{ $cookies.get('user').login }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'navbar',
  computed: {
    cartproducts () {
      return this.$store.getters.cartProducts;
    },
  },
  methods: {
    logout: function logout(){
      this.$cookies.remove("user");
      this.$router.push('/');
      window.location.reload(true);
    }
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.navbar{
  & .flex{
    display: flex;
    justify-content: space-between;
    align-items: center;
    @media(max-width: 960px){
      display: block;
    }
  }
  padding: 30px 0;
  margin: 10px 6.7vw 80px 4vw;
}

#nav {
  @media(max-width: 960px){
    text-align: right;
  }
}
#nav a {
  font-weight: bold;
  color: #277fbf;
  text-decoration: none;
  font-size:20px;
  margin:0 10px;
  transition: 0.5s color;
  cursor: pointer;
  @media(max-width: 1100px){
    font-size:18px;
  }
  @media(max-width: 960px){
    line-height: 2;
  }
}
#nav a:hover {
  color: #c13a24;
}

#nav a.router-link-exact-active {
  color: #c13a24;
}
#nav a.router-link-exact-active:hover {
  color: #277fbf;
}
.navbar__logo{
  font-size:30px;
  font-weight: bold;
  @media(max-width: 1100px){
    font-size:25px;
  }
  @media(max-width: 960px){
    margin-bottom: 20px;
  }
}
.cart{
  position: relative;
}
.how-much{
  padding: 6px;
  color: #fff;
  background-color: #c13a24;
  text-align: center;
  position: absolute;
  top: -40px;
  right: 0px;
  border-radius: 30px;
  font-size: 15px;
  min-width: 25px;
  min-height: 25px;
  display: flex;
  justify-content: center;
  align-items: center;
}
.login-name{
  margin-top:20px;
}
</style>
