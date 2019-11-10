function fn() {    
  var env = karate.env; // get system property 'karate.env'
  karate.log('karate.env system property was:', env);
  if (!env) {
    env = 'e2e';
  }
  var config = {
	appHost: 'http://localhost:5000/',
    sellerPath: '/sellers',
    productsPath: '/products'
  };
  if (env == 'dev') {
    // customize
    // e.g. config.foo = 'bar';
  } else if (env == 'e2e') {
    config.appHost = 'http://www.mocky.io/v2'
  }
  return config;
}