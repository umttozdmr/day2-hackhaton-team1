Feature: Day-2 Hackhaton Seller Service API Testing

  Background:
    * url appHost
    * header Accept = 'application/json'
    * def seller =
      """
      {
        "id": "15",
        "username": "testuser",
        "email": "test@user.com",
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

  Scenario: Seller create
    Given path sellerPath
    When method POST
    And params seller
    Then status 201

  Scenario: Seller presentation validation?
    Given path sellerPath + "/" + seller.id
    When method GET
    Then status 200
    And match $ == seller

  Scenario: Validation Sellers list
    Given path sellerPath
    When method GET
    Then status 200
#    validaton righting

  Scenario: Validation products in seller
    Given path sellerPath + "/" + seller.id + productsPath
    When method GET
    Then status 200
#    validation righting

