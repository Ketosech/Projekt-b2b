<template>
  <div class="offers">
    <div class="offers__search">
      <div class="offers__search-content">
        <h2>Wyszukaj przedmiot:</h2>
        <div>
          <div class="offers__search-elem fix">
            <input type="text" name="product_name" value="" placeholder="Nazwa produktu" v-model="params.name">
            <div class="icon" v-if="params.name == ''">
              <img src="@/assets/images/icons/search-icon.png?1" alt="Search Icon" >
            </div>
            <div class="icon" v-if="params.name != ''" v-on:click="deleteproductname()">
              <img src="@/assets/images/icons/x-mark.png?1" alt="X-Mark Icon">
            </div>
          </div>
        </div>
        <!-- <div class="offers__search-button" v-on:click="getProducts()">
        Szukaj
      </div> -->
    </div>
  </div>
  <div class="offers__content">
    <div v-if="!loading">
      <div class="flex" v-if="offers[0].symbol != null && offers[0].id != 0">
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
            <p v-if="$cookies.get('user') != null">Ilość: {{ offer.quantity }}</p>
          </div>
          <div class="offers__content-button blue" v-on:click="addItem(offer)" v-if="$cookies.get('user') != null && !cartproducts.find(item => item.id === offer.id)">
            Dodaj do koszyka
          </div>
          <div class="offers__content-button red" v-on:click="removeItem(offer)" v-if="$cookies.get('user') != null && cartproducts.find(item => item.id === offer.id)">
            Usuń z koszyka
          </div>
          <a href="tel:111111111">
            <div class="offers__content-button">
              Zapytaj o ofertę
            </div>
          </a>
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
  name: 'offers',
  data: function data() {
    return {
      apiUrl: "https://apiwsb.azurewebsites.net/",
      params: {
        name: '',
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
      loading: true,
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
      this.params.name = ''
    },
    getProducts: function getProducts() {
      var that = this;
      that.loading = true;
      if (this.params.name == '') {
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
        .get(this.apiUrl+"Products/name="+this.params.name, {})
        .then(function (response) {
          that.loading = false;
          that.offers = response.data;
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
    'params.name': function() {
      if (!this.awaitingSearch) {
        setTimeout(() => {
          this.getProducts();
          this.awaitingSearch = false;
        }, 1300);
      }
      this.awaitingSearch = true;
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
      }
      &.blue{
        background-color: #277fbf;
      }
      &:last-of-type{
        margin-bottom: 0px;
      }
      &:hover{
        background-color: #c13a24;
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
</style>
