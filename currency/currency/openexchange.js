var http = require('http'),
    request = require('request');


var currencyData = undefined;
var currencyDataHasExpired = true;


translateCurrencyRates = function (source, target, rates) {

    if (source == target)
        return 1;

    if (source == rates.base) {
        //going from USD, usualy
        if (rates.rates[target] != undefined) {
            return rates.rates[target];
        } else {
            return -1;
        }
    }

    if (target == rates.base) {
        //going TO USD, usually
        if (rates.rates[source] != undefined) {
            return 1.0 / rates.rates[source];
        } else {
            return -1;
        }
    }

    //two pairs which are not the base on either side


    if (rates.rates[source] != undefined && rates.rates[target] != undefined) {
        var sourceInBase = 1.0 / rates.rates[source];
        return sourceInBase * rates.rates[target];
    } else {
        return -1;
    }


}

checkCurrencyExpiry = function () {
    if (currencyDataHasExpired || currencyData == undefined)
        return true;

    if (new Date() > currencyDataLastUpdate) {
        return true;
    }


    return false;
};

ensureCurrencyTableIsUptodate = function (res) {
    if (checkCurrencyExpiry()) {

        request('http://openexchangerates.org/api/latest.json?app_id=acdb02503fa74418adef52afbbc31631', function (error, response, body) {
            if (!error && response.statusCode == 200) {


                currencyData = JSON.parse(body);
                currencyDataHasExpired = false;

                var date = new Date();
                date.setTime(date.getTime() + (12 * 60 * 60 * 1000));
                currencyDataLastUpdate = date;

                res(currencyData);
            } else {
                if (currencyData != undefined) {
                    res(currencyData);
                }
            }
        });
    } else {
        res(currencyData);
    }
};

getCurrencyForPair = function (baseCurrency, targetCurrency, res) {

    ensureCurrencyTableIsUptodate(function (currencyTable) {
        res(translateCurrencyRates(baseCurrency, targetCurrency, currencyData).toFixed(4));
    });
};

getCurrencyTableForBase = function (baseCurrency, res) {
    ensureCurrencyTableIsUptodate(function (currencyTable) {
        var output = {};
        output.baseCurrency = baseCurrency;
        output.currencys = [];

        Object.keys(currencyTable.rates).forEach(function (item) {
            var val = currencyTable.rates[item];
            getCurrencyForPair(baseCurrency, item, function(rate) {
                output.currencys.push({id: item, rate: parseFloat(rate)});
            });

        });
        res(output);
    });
};

exports.rateForCurrencyPair = function (req, res) {
    var baseCurrency = req.query.baseCurrency;
    var targetCurrency = req.query.targetCurrency;

    getCurrencyForPair(baseCurrency, targetCurrency, function(rate) {
        res.send({"BaseCurrency": baseCurrency, "TargetCurrency": targetCurrency, "Rate": rate});
    });


};

exports.ratesForCurrency = function (req, res) {
    var currency = req.query.baseCurrency;
    getCurrencyTableForBase(currency, function (rate_table) {
        res.send(rate_table);
    });
};