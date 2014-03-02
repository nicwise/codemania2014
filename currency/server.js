var express = require('express'),
    openexchange = require('./currency/openexchange');

var app = express();

app.get('/TripWallet/RateForCurrencyPair', openexchange.rateForCurrencyPair);
app.get('/codemania/rates', openexchange.ratesForCurrency);


app.listen(3000);
console.log('Listening on port 3000...');