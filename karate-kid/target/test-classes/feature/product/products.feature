Feature: Day-2 Hackhaton Product Service API Testing

  Background:
    * url appHost
    * header Accept = 'application/json'
    * def product =
      """
      {
        "id": "1",
        "name": "Stadia",
        "type": "Gaming",
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
    And match $.app == 'live'

  Scenario: Product create
    Given path productsPath
    When method POST
    And params product
    Then status 201

  Scenario: Seller presentation validation?
    Given path productsPath + "/" + product.id
    When method GET
    Then status 200
    And match $ == product

  Scenario: Validation Sellers list
    Given path productPath
    When method GET
    Then status 200
#    validaton righting


