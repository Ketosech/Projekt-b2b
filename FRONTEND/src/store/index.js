import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    cart: [],
  },
  mutations: {
    pushProductToCart (state, product){
      console.log(product);
      if (product != '' && product != null) {
        if (product.length > 1 || product.id == undefined) {
          state.cart = product;
        }else{
          state.cart.push({
            id: product.id,
            name: product.name,
            price: product.price,
            quantity: 1,
            real_quantity: product.quantity,
            symbol: product.symbol
          });
        }
      }
    },
    removeProductfromCart (state, cartItem){
      if (cartItem != '' && cartItem != null) {
        state.cart.splice(state.cart.findIndex(item => item === cartItem), 1)
      }
     },
    incrementItemQuantity (state, cartItem) {
      if (cartItem.quantity < cartItem.real_quantity) {
        cartItem.quantity++;
      }
    },
    decrementItemQuantity (state, cartItem) {
      if (cartItem.quantity > 1) {
        cartItem.quantity--;
      }
    },
  },
  actions: {
    addToCart(context, product){
      if (product != '' && product != null) {

        var cartItem = context.state.cart.find(item => item.id === product.id);

        if (!cartItem) {
          context.commit('pushProductToCart', product);
        }else{
          context.commit('incrementItemQuantity', cartItem);
        }
      }
    },
    removeFromCart(context, product){
      var cartItem = context.state.cart.find(item => item.id === product.id);
      if (cartItem) {
        context.commit('removeProductfromCart', cartItem);
      }
    },
    decrementItem(context, product){
      if (product != '' && product != null) {
        var cartItem = context.state.cart.find(item => item.id === product.id);
        context.commit('decrementItemQuantity', cartItem);
      }
    },
    checkincart(context, product){
      if (product != '' && product != null) {
        return context.state.cart.find(item => item.id === product.id);
      }
    },
  },
  getters: {
    cartProducts(state) {
      return state.cart;
    },
    cartTotal(state, getters){
      return getters.cartProducts.reduce((total, product) => total += product.price * product.quantity, 0)
    }
  }
})
