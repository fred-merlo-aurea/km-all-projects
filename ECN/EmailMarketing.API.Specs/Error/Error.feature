Feature: Error
	In order log errors from internal applications consuming Email Marketing via the MVC WebAPI
	As a Developer
	I want to format exceptions and POST them to the KM Common database

@Error_API
Scenario: Format an exception
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I set ControllerName to "internal/error"
	 And I set ActionName to "format"
	 And I have an Exception object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "200 OK"
	 And The HTTP Response Content should be a string
	 And the string should contain "inner message"
	 And the string should contain "outer message"

@Error_API
Scenario: POST a new resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I set ControllerName to "internal/error"
	 And I have a new Error Object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "201 Created"
	 And The HTTP Response Content should be an Error object
	 And the LogID property should be greater than 0
