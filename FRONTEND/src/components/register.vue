<template>
  <div>
    <div class="register__content" v-if="!registered && !loading">
      <div class="">
        <div class="register">
          <div class="">
            <p>Login:</p>
            <input type="text" name="" value="" v-model="params.login">
            <br>
            <br>
            <p>Hasło:</p>
            <input type="password" name="" value="" v-model="params.password">
            <br>
            <br>
            <p>Powtórz Hasło:</p>
            <input type="password" name="" value="" v-model="password_helper">
          </div>
        </div>
        <div class="register__button" v-on:click="registermethod()">
          Zarejestruj się
        </div>
      </div>
    </div>
    <div class="loading" v-if="loading">
      <img src="@/assets/images/loading.gif?1" alt="Loading">
    </div>
    <div class="error" v-if="error">
      {{ errortext }}
    </div>
    <div class="registered" v-if="registered">
      Udało ci się zarejestrować!
    </div>
  </div>
</template>

<script>
export default {
  name: 'register',
  data: function data() {
    return {
      apiUrl: "https://apiwsb.azurewebsites.net/",
      params: {
        id: 0,
        login: '',
        password: '',
        permissions: 0
      },
      password_helper: '',
      error: false,
      registered: false,
      loading: false,
    };
  },
  methods: {
    registermethod: function registermethod(){
      var that = this;
      this.error = false;
      that.registered = false;
      that.loading = true;
      if (this.params.password === this.password_helper) {
        if (this.params.login != '' && this.params.password != '') {
          this.$http
          .post(this.apiUrl+"Users/", that.params)
          .then(function (response) {
            if (response.data === 'Użytkownik z takim loginem już istnieje') {
              that.loading = false;
              that.error = true;
              that.errortext = 'Błąd rejestracji, takie konto już istnieje';
            }else{
              that.loading = false;
              that.registered = true;
            }
          })
          .catch(function () {
            window.alert("Sprawdź połączenie z internetem!");
          });
        }else{
          that.loading = false;
          this.error = true;
          this.errortext = 'Błąd rejestracji, proszę uzupełnić dane';
        }
      }else{
        that.loading = false;
        this.error = true;
        this.errortext = 'Błąd rejestracji, proszę wpisać te same hasła';
      }
    }
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.register{
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
    margin-top:30px;
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
.registered{
  text-align: center;
  color:#2e9e31;
  margin:20px 0 0 0;
  font-size:30px;
}
.loading{
  width: 100%;
  display: flex;
  justify-content: center;
}
</style>
