<template>
  <div>
    <div class="login__content" v-if="!loading && $cookies.get('user') == null">
      <div class="">
        <div class="login">
          <div class="">
            <p>Login:</p>
            <input type="text" name="" value="" v-model="params.login">
            <br>
            <br>
            <p>Hasło:</p>
            <input type="password" name="" value="" v-model="params.password">
          </div>
          <br>
          <router-link to="/register">Nie masz konta? Zarejestruj się!</router-link>
        </div>
        <div class="login__button" v-on:click="loginmethod()">
          Zaloguj się
        </div>
      </div>
    </div>
    <div class="error" v-if="error">
      {{ errortext }}
    </div>
    <div class="login-error" v-if="$cookies.get('user') != null">
      Już jesteś zalogowany
    </div>
    <div class="loading" v-if="loading">
      <img src="@/assets/images/loading.gif?1" alt="Loading">
    </div>
  </div>
</template>

<script>
export default {
  name: 'login',
  data: function data() {
    return {
      apiUrl: "https://apiwsb.azurewebsites.net/",
      params: {
        login: '',
        password: '',
      },
      user: {},
      error: false,
      loading: false,
    };
  },
  methods: {
    loginmethod: function loginmethod(){
      var that = this;
      this.error = false;
      this.loading = true;
      if (this.params.login != '' && this.params.password != '') {
        this.$http
        .get(this.apiUrl+"Users/login="+this.params.login, {})
        .then(function (response) {
          if (response.data['password'] == that.params.password) {
            that.user = {
              login: response.data.login,
              permission: response.data.permissions
            };
            that.$cookies.set('user', JSON.stringify(that.user));
            that.loading = false;
            that.$router.push('/');
            window.location.reload(true);
          }else{
            this.loading = false;
            this.error = true;
            this.errortext = 'Błąd logowania, proszę sprawdzić dane';
          }
        })
        .catch(function () {
          that.loading = false;
          that.error = true;
          that.errortext = 'Błąd logowania, proszę sprawdzić dane';
        });
      }else{
        this.loading = false;
        this.error = true;
        this.errortext = 'Błąd logowania, proszę uzupełnić dane';
      }
    }
  },
  created: function created() {
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.login{
  & p{
    font-size:20px;
    margin:10px 0;
  }
  & input{
    width: 320px;
    height: 50px;
    font-size:25px;
  }
  &__content{
    display: flex;
    justify-content: center;
  }
  &__button{
    background-color: #277fbf;
    width: 305px;
    padding:15px 15px;
    text-align: center;
    color:#ffffff;
    margin-top:20px;
    cursor: pointer;
    font-size:20px;
    transition: 0.5s all;
    &:hover{
      background-color: #c13a24;
    }
  }
}
.error{
  text-align: center;
  color:red;
  margin:20px 0 0 0;
}
.login-error{
  text-align: center;
  color:red;
  margin:20px 0 0 0;
  font-size:35px;
}
.loading{
  width: 100%;
  display: flex;
  justify-content: center;
}
</style>
