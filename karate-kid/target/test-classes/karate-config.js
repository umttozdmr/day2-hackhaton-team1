function fn() {    
  var env = karate.env; // get system property 'karate.env'
  if (!env) {
    env = 'dev';
  }
  var config = {
	appHost: 'http://',
    sellerPath: '/sellers',
    productsPath: '/products'
  };
  if (env == 'dev') {
    // customize
    // e.g. config.foo = 'bar';
  } else if (env == 'e2e') {
  }
  return config;
}