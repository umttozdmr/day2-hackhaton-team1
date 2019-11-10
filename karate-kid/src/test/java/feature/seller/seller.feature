Feature: Day-2 Hackhaton Seller Service API Testing

  Background:
    * url appHost + ":5000/"
    * header Accept = 'application/json'
    * def seller =
      """
      {
        "id": 2,
        "userName": "testuser",
        "email": "test@user.com",
      }
      """

  Scenario: karate health
    Given print "Karate"
    When print "is"
    Then print "Here"

  Scenario: Service is running?
    Given path '/healthz'
    When method GET
    Then status 200

  Scenario: Seller create
    Given path sellerPath
    And request seller
    When method POST
    Then status 201

  @done
  Scenario: Seller presentation validation?
    Given path sellerPath + "/" + seller.id
    When method GET
    Then status 200
    And match response == seller

  Scenario: Validation Sellers list
    Given path sellerPath
    When method GET
    Then status 200
    And match response == '#notnull'

  Scenario: Validation products in seller
    Given path sellerPath + "/" + seller.id + productsPath
    When method GET
    Then status 200
    And print response
    And match response == '#notnull'

