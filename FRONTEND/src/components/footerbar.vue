<template>
  <div id="footerbar">
    <div class="footer">
      <div class="footer__content">
        <div class="footer__content-elem">
          <strong>Projekt programowanie zaawansowane ©{{ new Date().getFullYear() }}</strong> <br>
          Adres: adresowa 1/2 <br>
          Mail: <a href="mailto:adres@test.pl">adres@test.pl</a> <br>
          Tel.: <a href="tel:123123123">+48 123 123 123</a>
        </div>
        <div class="footer__content-elem">
          Created by <br>
          Rafał Kukawski <br>
          Remigiusz Walkowiak <br>
          Krzysztof Waleryszak
        </div>
      </div>
    </div>
  </div>
</template>


<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.footer{
  border-top:4px solid #000;
  padding:20px 0;
  margin-top:150px;
  &__content{
    margin:0 8vw;
    display: flex;
    justify-content: space-between;
    &-elem{
      font-size:20px;
    }
  }
}
</style>
