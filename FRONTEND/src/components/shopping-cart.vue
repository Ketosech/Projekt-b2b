<template>
  <div class="container">
    <h1 class="shoppingcart__header upper" v-if="cartproducts !=''">Koszyk</h1>
    <div class="shoppingcart" v-if="cartproducts !=''">
      <div class="shoppingcart__elem" v-for="(product, id) in cartproducts" v-bind:key="id">
        <div class="shoppingcart__elem-content">
          <h2>Nazwa: {{ product.name }}</h2>
          <small>Symbol: {{ product.symbol }}</small>
          <p>Cena: {{ product.price }} PLN</p>
          <p>Ilość: {{ product.quantity }} / {{ product.real_quantity }} szt.</p>
        </div>
        <div class="shoppingcart__elem-modif">
          <div class="plus icon" v-on:click="addItem(product)">
            <img src="@/assets/images/icons/plus.png?1" alt="Plus Icon">
          </div>
          <div class="minus icon" v-on:click="decrementItem(product)">
            <img src="@/assets/images/icons/minus.png?1" alt="Minus Icon">
          </div>
          <div class="delete icon" v-on:click="removeItem(product)">
            <img src="@/assets/images/icons/delete.png?1" alt="Delete Icon">
          </div>
        </div>
      </div>
    </div>
    <div class="shoppingcart__checkout" v-if="cartproducts !=''">
      <h1>Suma: {{ priceFormat.to(Number((total).toFixed(1))) }}</h1>
    </div>
    <div class="center" v-if="cartproducts !=''">
      <div class="shoppingcart__checkout-button" v-on:click="buyitems()">
        Kupuj
      </div>
    </div>
      <h1 class="shoppingcart__header" v-if="cartproducts == ''">Brak przedmiotów w koszyku</h1>
  </div>
</template>

<script>
import wnumb from "wnumb";

export default {
  name: 'shopping-cart',
  data: function data() {
    return {
      priceFormat: wnumb({
        mark: ".",
        thousand: " ",
        prefix: "",
        suffix: " zł",
      }),
    };
  },
  methods: {
    addItem: function addItem(product) {
      this.$store.dispatch('addToCart', product);
    },
    removeItem: function removeItem(product) {
      this.$store.dispatch('removeFromCart', product);
    },
    decrementItem: function decrementItem(product) {
      this.$store.dispatch('decrementItem', product);
    },
    buyitems: function buyitems() {
      this.$cookies.remove('cart');
      this.$store.dispatch('addToCart', {});
      window.location.reload(true);
    },
  },
  created: function created() {
  },
  computed: {
    cartproducts () {
      return this.$store.getters.cartProducts;
    },
    total () {
      return this.$store.getters.cartTotal;
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.container{
  max-width: 960px;
  margin:50px auto;
  @media(max-width: 1100px){
    margin:50px;
  }
}
.shoppingcart{
  border:4px solid #000;
  &__elem{
    padding:10px;
    border-bottom:4px solid #277fbf;
    display: flex;
    justify-content: space-between;
    &:last-of-type{
      border-bottom:0px solid #277fbf;
    }
    &-content{
      & h2{
        font-size:26px;
      }
      & small{
        margin:15px 0;
      }
      & p{
        font-size:20px;
        margin:5px 0;
      }
    }
    &-modif{
      display: flex;
      align-items: center;
      & .icon{
        margin:0 10px;
        border: 1px solid #000;
        padding: 5px;
        cursor: pointer;
        display: flex;
        align-items: center;
      }
    }
  }
  &__header{
    font-size:40px;
    margin-bottom: 20px;
    text-align: center;
    &.upper{
      text-transform: uppercase;
    }
  }
  &__checkout{
    text-align: right;
    padding:10px;
    & h1{
      font-size:25px;
    }
    &-button{
      display: inline-block;
      font-size: 20px;
      text-align: center;
      color: #fff;
      margin: 10px 0;
      cursor: pointer;
      transition: all 0.5s;
      padding: 15px 40px;
      border-radius: 30px;
      background-color: #277fbf;
      &:hover{
        background-color: #c13a24;
      }
    }
  }
}
.center{
  text-align: center;
}
</style>
