<template>
  <div v-if="$cookies.get('user') != null && $cookies.get('user').permission == '1'">
    <div class="adminlogin" v-if="!loading">
      <div class="">
        <div class="adminlogin__elem" v-for="(user, key) in users" v-bind:key="key">
          <div class="item" v-if="$cookies.get('user').login != user.login">
            <div class="flex">
              <div class="adminlogin__elem-info">
                <div>
                  <p>Login:</p>
                  <h2>{{ user.login }}</h2>
                  <div class="line">

                  </div>
                  <p>Prawa:</p>
                  <h2>{{ permission_text[user.permissions] }}</h2>
                </div>
              </div>
              <div class="center">
                <div class="adminlogin__elem-delete" v-on:click="deleteuser(user.id)">
                  <img src="@/assets/images/icons/delete.png?1" alt="Delete Icon">
                </div>
                <div class="adminlogin__elem-permission" v-on:click="changepermission(user)">
                  Zmień prawa
                </div>
              </div>
            </div>
            <div class="success" v-if='response && response_id === user.id'>
              {{ response }}
            </div>
          </div>
        </div>
      </div>
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
      users: {},
      permission_text: {
        0: 'Użytkownik',
        1: 'Administrator',
      },
      loading: false,
      response: false,
      response_id: '',
    };
  },
  methods: {
    changepermission: function changepermission(val){
      console.log(val);
      var user = val;
      if (user['permissions'] === 0) {
        user['permissions'] = 1;
      }else if (user['permissions'] === 1) {
        user['permissions'] = 0;
      }
      var that = this;
      this.loading = true;
      this.response = '';
      this.response_id = user['id'];
      this.$http
      .put(this.apiUrl+"Users", user)
      .then(function (response) {
        that.loading = false;
        that.response = response.data;
      })
      .catch(function () {
        window.alert("Sprawdź połączenie z internetem!");
      });
    },
    deleteuser: function deleteuser(val){
      var that = this;
      this.loading = true;
      this.response = '';
      this.response_id = val;
      this.$http
      .delete(this.apiUrl+"Users/id="+val, {})
      .then(function (response) {
        that.loading = false;
        that.response = response.data;
        window.location.reload(true);
      })
      .catch(function () {
        window.alert("Sprawdź połączenie z internetem!");
      });
    },
    getusers: function getusers(){
      var that = this;
      this.loading = true;
      this.$http
      .get(this.apiUrl+"Users", {})
      .then(function (response) {
        that.loading = false;
        that.users = response.data;
      })
      .catch(function () {
        window.alert("Sprawdź połączenie z internetem!");
      });
    }
  },
  created: function created() {
    this.getusers();
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.adminlogin{
  &__elem{
    display: flex;
    & .item{
      display: inline-block;
      margin:15px auto;
      & .flex{
        display: flex;
        align-items: center;
        padding: 20px 15px;
        border: 1px solid #000;
        min-width: 320px;
      }
    }
    &-info{
      display: flex;
      align-items: center;
      margin:0 auto 0 0px;
      & p{
        margin-bottom: 5px;
        font-size: 20px;
      }
      & .line{
        width: 100%;
        height: 2px;
        background-color: #277fbf;
        margin:10px 0;
      }
    }
    & .center{
      text-align: center;
    }
    &-delete{
      padding:0 15px;
      cursor: pointer;
      margin:0 0 10px 0;
    }
    &-permission{
      background-color: #277fbf;
      padding: 10px 20px;
      color:#ffffff;
      border-radius: 20px;
      cursor: pointer;
      transition: background-color 0.5s;
      &:hover{
        background-color: #c13a24;
      }
    }
  }
}
.success{
  color: #2bb936;
  margin:10px 0 0 0;
  font-weight: bold;
  text-align: center;
  font-size:16px;
}
.loading{
  width: 100%;
  display: flex;
  justify-content: center;
}
</style>
