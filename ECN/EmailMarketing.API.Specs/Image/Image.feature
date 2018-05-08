Feature: Image
	In order to provide customized user experience
	As an Email Marketing API consumer
	I want to CRUD (Create, Read, Update, Delete) Image

@Image_API
Scenario: GET an existing resource
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	And I have a Customer ID of 1
	And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And The HTTP Response Message should be a valid API Model object
	And The API Model object should have a LayoutID property matching the given Layout ID

@Image_API
Scenario: PUT an update to an existing resource
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	And I have a Customer ID of 1
	And I have an existing Message Object with a LayoutID of 76660
	And I generate a RandomString to append to the DisplayAddress
	When I invoke PUT
	Then I should receive an HTTP Response
	And status should be "200 OK"
	And a Location Header should be sent
	And the Location Header should end with LayoutID
	And HttpResponseContent should have DisplayAddress ending with the RandomString

@Image_API
Scenario: POST a new resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	And I have a Customer ID of 1
	 And I have a new Image Object
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "201 Created"
	 And a Location Header should be sent
	 And the Location Header should end with ImageID
	 And HttpResponseContent should have ImageName ending with the RandomString
	 And I can store the Message object to test delete

@Image_API
Scenario: DELETE a resource
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I have a deletable Image Object
	When I invoke DELETE
	Then I should receive an HTTP Response
	 And status should be "204 No Content"

@Image_API
Scenario: GET when missing API Access Key
   Given I have an existing Layout ID of 76660
	 And I have a Customer ID of 1
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is missing'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET when missing CustomerID header
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
     And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "X-Customer-ID" is missing'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET with invalid API Access Key
   Given I have an invalid API Access Key of "TotsBogus"
	 And I have a Customer ID of 1
     And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "APIAccessKey" is malformed: TotsBogus'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET with invalid Customer ID header
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of TotsBogus
     And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Authentication Token Header "X-Customer-ID" is malformed: TotsBogus'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET with valid but unknown API Access Key
   Given I have a valid API Access Key of "3385AEC7-3BD1-430A-A18C-54F33262636C"
	 And I have a Customer ID of 1
     And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'No user with authentication token "3385AEC7-3BD1-430A-A18C-54F33262636C"'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET with valid but unknown Customer ID header
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 99999
     And I have an existing Layout ID of 76660
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "401 Unauthorized"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'No customer with ID "99999"'
	 And the error HttpStatusCode should be '401 Unauthorized'

@Image_API
Scenario: GET with incorrect API Access Key
   Given I have a valid API Access Key of "BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3"
	 And I have a Customer ID of 35
     And I have an existing Layout ID of 293229
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "403 Forbidden"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'You are not authorized to access the selected resource.'
	 And the error HttpStatusCode should be '403 Forbidden'

@Image_API
Scenario: GET with incorrect Customer ID header
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 14
     And I have an existing Layout ID of 293229
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "403 Forbidden"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'You are not authorized to access the selected resource.'
	 And the error HttpStatusCode should be '403 Forbidden'

@Image_API
Scenario: GET non-existent Message
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
     And I have an nonexistent Layout ID of 12345
	When I invoke GET
	Then I should receive an HTTP Response
	 And status should be "404 Not Found"

@Image_API
Scenario: PUT invalid data
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	And I have an existing Message Object with a LayoutID of 76660
	And I append invalid character '"' to the LayoutName
	When I invoke PUT
	Then I should receive an HTTP Response
	 And status should be "400 Bad Request"
	 And the HTTP Response Content should be an HttpError
	 And the error Message Should be 'Entity: Layout Method: Validate Message: LayoutName contains invalid characters. '
	 And the error HttpStatusCode should be '400 Bad Request'

@Image_API
Scenario: POST invalid data
	Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	  And I have a new Image Object
	  And I append invalid character '"' to the ImageName
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "400 Bad Request"
	 And the HTTP Response Content should be an HttpError
	 And the error HttpStatusCode should be '400 Bad Request'

