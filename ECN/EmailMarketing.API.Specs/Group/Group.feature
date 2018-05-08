Feature: Group
	In order to provide customized user experience
	As an Email Marketing API consumer
	I want to CRUD (Create, Read, Update, Delete) Group

@Group_API
Scenario: GET an existing resource
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	And I have an existing GroupID of 49195
	When I invoke GET
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And The HTTP Response Group should be a valid API Model object
	And The API Model object should have a GroupID property matching the given GroupID

@Group_API
Scenario: POST a new resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I have a new Group Object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "201 Created"
	 And a Location Header should be sent
	 And the Location Header should end with GroupID
	 And HttpResponseContent should have ContentText ending with the RandomString

@Group_API
Scenario: GET when missing API Access Key
	 And I have a Customer ID of 1
   Given I have an existing GroupID of 3
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is missing'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Group_API
Scenario: GET with invalid API Access Key
   Given I have an invalid API Access Key of "TotsBogus"
	 And I have a Customer ID of 1
     And I have an existing GroupID of 49195
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is malformed: TotsBogus'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Group_API
Scenario: GET with valid but unknown API Access Key
   Given I have a valid API Access Key of "3385AEC7-3BD1-430A-A18C-54F33262636C"
	 And I have a Customer ID of 1
     And I have an existing GroupID of 49195
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'No user with authentication token "3385AEC7-3BD1-430A-A18C-54F33262636C"'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Group_API
Scenario: GET with incorrect API Access Key
   Given I have a valid API Access Key of "BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3"
	 And I have a Customer ID of 35
     And I have an existing GroupID of 49195
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "403 Forbidden"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'You are not authorized to access the selected resource.'
	 And the error HttpStatusCode should be '403 Forbidden'

@Group_API
Scenario: GET non-existent content
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
     And I have an nonexistent GroupID of 12345
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "404 Not Found"

@Group_API
Scenario: POST invalid data
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	  And I have a new Group Object
	  And I append invalid character '"' to the GroupName
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "400 Bad Request"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Entity: Group Method: Validate Message: GroupName has invalid characters. '
	 And the error HttpStatusCode should be '400 Bad Request'

