Feature: BaseChannel
	In order to develop internal applications
	As the developer of an application consuming Email Marketing APIs
	I want to get BaseChannel information

@EmailDirect_API
Scenario: POST a new resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I set ControllerName to "internal/emaildirect"
	 And I have a new EmailDirect Object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "201 Created"
	 And The HTTP Response Content should be an EmailDirect object
	 And the EmailDirectID property should be greater than 0
