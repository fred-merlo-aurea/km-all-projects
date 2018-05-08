Feature: CustomField
	In order to provide customized user experience
	As an Email Marketing API consumer
	I want to CRUD (Create, Read, Update, Delete) CustomField

@CustomField_API
Scenario: GET an existing resource
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	And I have an existing GroupDataFieldsID of 254724
	When I invoke GET
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And The HTTP Response CustomField should be a valid API Model object
	And The API Model object should have a GroupDataFieldsID property matching the given GroupDataFieldsID

@CustomField_API
Scenario: POST a new resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I have a new CustomField Object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "201 Created"
	 And a Location Header should be sent
	 And the Location Header should end with GroupDataFieldsID
	 #And HttpResponseContent should have ShortName ending with the RandomString

@CustomField_API
Scenario: GET when missing API Access Key
   Given I have an existing GroupDataFieldsID of 254724
	 And I have a Customer ID of 1
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is missing'
	 And the error HttpStatusCode should be '401 Unauthorized'

@CustomField_API
Scenario: GET with invalid API Access Key
   Given I have an invalid API Access Key of "TotsBogus"
	 And I have a Customer ID of 1
     And I have an existing GroupDataFieldsID of 254724
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is malformed: TotsBogus'
	 And the error HttpStatusCode should be '401 Unauthorized'

@CustomField_API
Scenario: GET with valid but unknown API Access Key
   Given I have a valid API Access Key of "3385AEC7-3BD1-430A-A18C-54F33262636C"
	 And I have a Customer ID of 1
     And I have an existing GroupDataFieldsID of 254724
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'No user with authentication token "3385AEC7-3BD1-430A-A18C-54F33262636C"'
	 And the error HttpStatusCode should be '401 Unauthorized'

@CustomField_API
Scenario: GET with incorrect API Access Key
   Given I have a valid API Access Key of "BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3"
	 And I have a Customer ID of 35
     And I have an existing GroupDataFieldsID of 254724
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "403 Forbidden"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'You are not authorized to access the selected resource.'
	 And the error HttpStatusCode should be '403 Forbidden'

@CustomField_API
Scenario: GET non-existent content
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
     And I have an nonexistent GroupDataFieldsID of 2
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "404 Not Found"

@CustomField_API
Scenario: POST invalid data
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	  And I have a new CustomField Object
	  And I append invalid character '"' to the ShortName
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "400 Bad Request"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Entity: GroupDataFields Method: Validate Message: ShortName has invalid characters. '
	 And the error HttpStatusCode should be '400 Bad Request'
