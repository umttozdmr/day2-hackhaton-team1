Feature: Day-2 Hackhaton Product Service API Testing

  Background:
    * url appHost + "product:80/"
    * header Accept = 'application/json'
    * def product =
      """
      {
        "id": 1,
        "sellerId": 2,
        "title": "Macbook Pro",
        "price": 100,
        "stock": 5
      }
      """
  @done
  Scenario: karate health
    Given print "Karate"
    When print "is"
    Then print "Here"

  Scenario: Service is running?
    Given path '/healthz'
    When method GET
    Then status 200

  Scenario: Product create
    Given path productsPath
    And request product
    When method POST
    Then status 201

  Scenario: Product presentation validation?
    Given path productsPath + "/" + product.id
    When method GET
    Then status 200
    And match response == product

  Scenario: Validation Product list
    Given path productsPath
    When method GET
    Then status 200



