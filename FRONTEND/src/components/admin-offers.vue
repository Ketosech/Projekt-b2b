<template>
  <div class="offers" v-if="$cookies.get('user') != null && $cookies.get('user').permission == '1'">
    <div class="offers__search" v-if="!addingproduct && editingproduct == ''">
      <div class="offers__search-content">
        <h2>Wyszukaj przedmiot:</h2>
        <div>
          <div class="offers__search-elem fix">
            <input type="text" name="product_name" value="" placeholder="Nazwa produktu" v-model="searcher.name">
            <div class="icon" v-if="searcher.name == ''">
              <img src="@/assets/images/icons/search-icon.png?1" alt="Search Icon" >
            </div>
            <div class="icon" v-if="searcher.name != ''" v-on:click="deleteproductname()">
              <img src="@/assets/images/icons/x-mark.png?1" alt="X-Mark Icon">
            </div>
          </div>
        </div>
        <!-- <div class="offers__search-button" v-on:click="getProducts()">
        Szukaj
      </div> -->
    </div>
  </div>

  <div class="offers__content adding-product" v-if="addingproduct">
    <h2>Dodaj Produkt</h2>
    <div class="offers__content-elem center">
      <div>
        <div class="offers__content-info">
          <div class="adding-product">
            <label for="name">Nazwa</label>
            <br>
            <input type="text" name="name" value="" placeholder="Nazwa" v-model="params.name">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Symbol</label>
            <br>
            <input type="text" name="" value="" placeholder="Symbol" v-model="params.symbol">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Cena</label>
            <br>
            <input type="number" name="" value="" placeholder="Cena" v-model="params.price">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Ilość</label>
            <br>
            <input type="number" name="" value="" placeholder="Ilość" v-model="params.quantity">
          </div>
          <br>
          <div class="offers__content-button blue" v-on:click="addproduct()">
            Dodaj
          </div>
          <div class="offers__content-button red" v-on:click="toggleproduct()">
            Anuluj
          </div>
        </div>
      </div>
    </div>
    <p class="error" v-if="error">Błąd dodawania produktu, uzupełnij dane</p>
  </div>

  <div class="offers__content adding-product" v-if="editingproduct != ''">
    <h2>Edytuj Produkt</h2>
    <div class="offers__content-elem center">
      <div>
        <div class="offers__content-info">
          <div class="adding-product">
            <label for="name">Nazwa</label>
            <br>
            <input type="text" name="name" value="" placeholder="Nazwa" v-model="editingproduct.name">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Symbol</label>
            <br>
            <input type="text" name="" value="" placeholder="Symbol" v-model="editingproduct.symbol">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Cena</label>
            <br>
            <input type="number" name="" value="" placeholder="Cena" v-model="editingproduct.price">
          </div>
          <br>
          <div class="adding-product">
            <label for="name">Ilość</label>
            <br>
            <input type="number" name="" value="" placeholder="Ilość" v-model="editingproduct.quantity">
          </div>
          <br>
          <div class="offers__content-button blue" v-on:click="editproduct()">
            Edytuj
          </div>
          <div class="offers__content-button red" v-on:click="toggleedit('')">
            Anuluj
          </div>
        </div>
      </div>
    </div>
    <p class="error" v-if="error">Błąd edytowania produktu, sprawdź poprawność danych</p>
  </div>

  <div class="offers__content" v-if="!addingproduct && editingproduct == ''">
    <div v-if="!loading">
      <transition name="fade">
        <h2 class="success__edit" v-if="askedited">Pomyślnie zedytowano produkt</h2>
      </transition>
      <div  class="flex" v-if="offers[0].symbol != null && offers[0].id != 0">
        <div class="offers__content-elem addoffer" v-on:click="toggleproduct()" v-if="offers[0].symbol != null && offers[0].id != 0">
          <div>
            <p>Dodaj produkt</p>
            <br>
            <img src="@/assets/images/add.png" alt="Add Icon">
          </div>
        </div>
        <div class="offers__content-elem" v-for="(offer, id) in offers" v-bind:key="id">
          <div class="offers__content-image">
            <img src="@/assets/images/no-image-available.jpg" alt="No image">
          </div>
          <div class="offers__content-info">
            <h2>{{ offer.name }}</h2>
            <small>Symbol: {{ offer.symbol }}</small>
            <div class="line">

            </div>
            <h4>Cena: {{ offer.price }} zł/szt.</h4>
            <p>Ilość: {{ offer.quantity }}</p>
          </div>
          <div class="offers__content-button blue" v-on:click="toggleedit(offer)">
            Edytuj produkt
          </div>
          <div class="offers__content-button red" v-on:click="buttonstoggle(offer.id)" v-if="buttons != offer.id">
            Usuń produkt
          </div>
          <div class="" v-if="buttons == offer.id">
            <div class="offers__content-button red" v-on:click="deleteproduct(offer.id)">
              Potwierdź
            </div>
            <div class="offers__content-button blue" v-on:click="buttonstoggle(offer.id)">
              Anuluj
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="loading flex" v-if="loading">
      <img src="@/assets/images/loading.gif?1" alt="Loading">
    </div>

    <div class="center error" v-if="!loading && offers[0].symbol == null && offers[0].id == 0">
      <h1>{{ offers[0].name }}</h1>
    </div>
  </div>
</div>
</template>

<script>
import wnumb from "wnumb";
// import { store } from 'vuex';

export default {
  name: 'adminoffers',
  data: function data() {
    return {
      apiUrl: "https://apiwsb.azurewebsites.net/",
      searcher: {
        name: '',
      },
      params: {
        id: 0,
        symbol: '',
        name: '',
        price: '',
        quantity: ''
      },
      priceFormat: wnumb({
        mark: "",
        thousand: " ",
        prefix: "",
        suffix: " ",
      }),
      headers:{
        Accept: 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'multipart/form-data'
      },
      offers: '',
      error: false,
      loading: true,
      addingproduct: false,
      askedited: false,
      editingproduct: '',
      buttons: false,
      awaitingSearch: false,
    };
  },
  computed: {
    cartproducts () {
      return this.$store.getters.cartProducts;
    },
  },
  methods: {
    addItem: function addItem(product) {
      this.$store.dispatch('addToCart', product);
    },
    removeItem: function removeItem(product) {
      this.$store.dispatch('removeFromCart', product);
    },
    checkincart: function checkincart(product) {
      this.$store.dispatch('checkincart', product);
    },
    deleteproductname: function deleteproductname() {
      this.searcher.name = '';
    },
    toggleproduct: function toggleproduct() {
      this.addingproduct = !this.addingproduct;
    },
    buttonstoggle: function buttonstoggle(val) {
      if (this.buttons == '') {
        this.buttons = val;
      }else{
        this.buttons = '';
      }
    },
    toggleedit: function toggleedit(val) {
      if (val != '') {
        this.editingproduct = val;
      }else{
        window.location.reload(true);
      }
    },
    getProducts: function getProducts() {
      var that = this;
      that.loading = true;
      if (this.searcher.name == '') {
        this.$http
        .get(this.apiUrl+"Products/", {})
        .then(function (response) {
          that.loading = false;
          that.offers = response.data;
        })
        .catch(function () {
          window.alert("Sprawdź połączenie z internetem!");
        });
      }else{
        this.$http
        .get(this.apiUrl+"Products/name="+this.searcher.name, {})
        .then(function (response) {
          that.loading = false;
          that.offers = response.data;
        })
        .catch(function () {
          window.alert("Sprawdź połączenie z internetem!");
        });
      }
    },
    addproduct: function addproduct() {
      var that = this;
      that.error = false;
      if (that.params.symbol != '' && that.params.name != '' && that.params.price != '' && that.params.quantity != '') {
        that.$http
        .post(that.apiUrl+"Products/", that.params)
        .then(function (response) {
          console.log(response);
          window.location.reload(true);
        })
        .catch(function () {
          window.alert("Sprawdź połączenie z internetem!");
        });
      }else{
        that.error = true;
      }
    },
    editproduct: function editproduct() {
      var that = this;
      that.error = false;
      if (that.editingproduct != '' && that.editingproduct.name != '' && that.editingproduct.price != null && that.editingproduct.quantity != null && that.editingproduct.symbol != '') {
        that.$http
        .put(that.apiUrl+"Products/", that.editingproduct)
        .then(function (response) {
          that.askedited = true;
          that.editingproduct = '';
          console.log(response);
        })
        .catch(function () {
          window.alert("Sprawdź połączenie z internetem!");
        });
      }else{
        that.error = true;
      }
    },
    deleteproduct: function deleteproduct(id) {
      var that = this;
      that.loading = true;
      if (id != '') {
        this.$http
        .delete(this.apiUrl+"Products/id="+id, {})
        .then(function (response) {
          that.loading = false;
          console.log(response);
          window.location.reload(true);
        })
        .catch(function () {
          window.alert("Sprawdź połączenie z internetem!");
        });
      }
    },
  },
  created: function created() {
    var that = this;
    that.getProducts();
  },
  watch: {
    'searcher.name': function() {
      if (!this.awaitingSearch) {
        setTimeout(() => {
          this.getProducts();
          this.awaitingSearch = false;
        }, 1300);
      }
      this.awaitingSearch = true;
    },
    'askedited': function() {
      setTimeout(() => {
        this.askedited = false;
      }, 3000);
    },
    'params.price': function(val) {
      if (this.params.price != null) {
        this.params.price = parseInt(val);
      }
    },
    'params.quantity': function(val) {
      if (this.params.quantity != null) {
        this.params.quantity = parseInt(val);
      }
    },
    'editingproduct.price': function(val) {
      if (this.editingproduct.price != null) {
        this.editingproduct.price = parseInt(val);
      }
    },
    'editingproduct.quantity': function(val) {
      if (this.editingproduct.quantity != null) {
        this.editingproduct.quantity = parseInt(val);
      }
    },
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.offers{
  display: flex;
  margin:0 4vw;
  min-height: 100vh;
  @media(max-width: 960px){
    display: block;
  }
  &__content{
    max-width: 71vw;
    width: 100%;
    padding:0 2vw;
    @media(max-width: 960px){
      padding:0px;
      max-width: 100%;
    }
    &.adding-product{
      max-width: 100vw;
      & h2{
        text-align: center;
      }
      & .error{
        color:red;
      }
    }
    & .flex{
      display: flex;
      flex-wrap: wrap;
    }
    & .loading{
      width: 100%;
      display: flex;
      justify-content: center;
    }
    & .error{
      text-align: center;
      font-size:30px;
    }
    &-elem{
      width: calc((100% / 4) - 30px);
      margin: 0 15px 50px 15px;
      -webkit-box-shadow: 5px 5px 15px 5px rgba(0, 0, 0, 0.35);
      -moz-box-shadow: 5px 5px 15px 5px rgba(0, 0, 0, 0.35);
      box-shadow: 5px 5px 15px 5px rgba(0, 0, 0, 0.35);
      @media(max-width: 1600px){
        width: calc((100% / 3) - 30px);
      }
      @media(max-width: 1300px){
        width: calc((100% / 2) - 30px);
      }
      @media(max-width: 650px){
        width: calc((100% / 1) - 30px);
      }
      &.center{
        margin:30px auto;
      }
      &.addoffer{
        min-height: 580px;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        text-align: center;
      }
    }
    &-info{
      padding: 15px 10px;
      min-height: 210px;
      & h2{
        font-size: 25px;
        margin-bottom: 5px;
      }
      & h4{
        font-size: 20px;
        margin-bottom: 5px;
      }
      & p{
        font-size: 20px;
      }
      & .line{
        width: 100%;
        height: 2px;
        background-color: #277fbf;
        margin:10px 0;
      }
      & .adding-product{
        & input{
          width: 98%;
          height: 30px;
          font-size:20px;
          margin-top:10px;
          &.error_input{
            border:1px solid red;
          }
        }
      }
    }
    &-button{
      font-size: 20px;
      text-align: center;
      color: #fff;
      width: 100%;
      margin: 10px 0;
      cursor: pointer;
      transition: all 0.5s;
      padding: 13px 0;
      background-color: #277fbf;
      &.red{
        background-color: #c13a24;
        &:hover{
          background-color: #277fbf;
        }
      }
      &.blue{
        background-color: #277fbf;
        &:hover{
          background-color: #c13a24;
        }
      }
      &:last-of-type{
        margin-bottom: 0px;
      }
    }
  }
  &__search{
    max-width: 23vw;
    width: 100%;
    border-right: 1px solid #000;
    position: relative;
    padding: 0 30px 0 0;
    @media(max-width: 960px){
      max-width: 100%;
      margin-bottom: 90px;
    }
    &-content{
      position: sticky;
      top:30px;
      @media(max-width: 960px){
        position: unset;
        text-align: center;
      }
      & h2{
        font-size:30px;
        margin-bottom: 20px;
        @media(max-width: 1440px){
          font-size: 22px;
        }
        @media(max-width: 960px){
          font-size: 30px;
        }
      }
    }
    &-elem{
      margin:30px 0 30px 0;
      max-width: 320px;
      width: 100%;
      @media(max-width: 960px){
        margin:50px auto 30px auto;
      }
      &.fix{
        display: flex;
        justify-content: flex-start;
        align-items: center;
        position: relative;
      }
      & .price{
        display: flex;
        justify-content: flex-start;
        align-items: center;
        position: relative;
      }
      & .icon{
        position: absolute;
        right:0px;
        cursor: pointer;
      }
      & input{
        border:0px;
        border-bottom: 1px solid #000;
        padding: 5px 20px 5px 3px;
        cursor: auto;
        width: 100%;
        text-align: center;
        &.fix{
          margin:0 10px;
        }
        &:focus{
          outline:0px;
        }
      }
    }
    &-button{
      font-size: 20px;
      text-align: center;
      color: #fff;
      background-color: #277fbf;
      max-width: 320px;
      width: 100%;
      margin:50px 0 10px 0;
      cursor: pointer;
      transition: all 0.5s;
      padding: 13px 0;
      border-radius: 40px;
      &:hover{
        background-color: #c13a24;
      }
    }
  }
}
.success{
  &__edit{
    font-size:30px;
    text-align: center;
    color:#2e9e31;
    margin-bottom: 50px;
  }
}

.fade-enter-active, .fade-leave-active {
  transition: opacity .5s;
}
.fade-enter, .fade-leave-to{
  opacity: 0;
}
</style>
