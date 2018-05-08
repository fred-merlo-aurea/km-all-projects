Feature: User
	In order support internal forms implementation
	As a Forms Developer
	I want to get the Forms User given a Base-Channel API Access Key

@User_API
Scenario: Get Forms User
	Given I have a valid API Access Key of "412B9D1D-33A2-466D-8F87-70C607E649D6"
	And I have a Customer ID of 1
	And I set ControllerName to "internal/user"
	And I set ActionName to "methods/GetFormsUser"
	When I invoke GET
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And The HTTP Response Content should be a valid User object
