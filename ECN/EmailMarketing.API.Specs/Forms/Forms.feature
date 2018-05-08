Feature: Forms
	In order support internal forms implementation
	As a Forms Developer
	I want to get the Forms User given a Base-Channel API Access Key

@Forms_API
Scenario: Get Forms Customers for BaseChannel
	Given I have a valid API Access Key of "412B9D1D-33A2-466D-8F87-70C607E649D6"
	And I set ControllerName to "internal/forms"
	And I set ActionName to "methods/GetFormsCustomersForBaseChannel"
	When I invoke GET
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And The HTTP Response Content should be a list of Forms Customer objects