Feature: EmailGroup
	In order manage Email Group members
	As an API user
	I want methods to control Email Group membership

@EmailGroup_API
@AddSubscribers_API
Scenario: Add Subscribers 
# hard coding group-id and API access key here to
# test that group-id is preserved in failure cases
   Given I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
     And I set the API Access Key with the Test Data Provider
	 And I set ActionName to "methods/ManageSubscribers"
	 And I have an Email List
	 #  when GroupID is -1 it will be filled with the ID of a valid "%test%" group
		| Email Address                 | Group ID | Format  | Subscribe Type |
		| invalid@group.com             | 99999    | HTML    | Subscribe      |
		| invalid@format.com            | -1       | Unknown | Subscribe      |
		| invalid@subscribe-type.com    | -1       | HTML    | MasterSuppress |
		| invalid@for-unsubscribe.com   | -1       | HTML    | Unsubscribe    |
		| invalid_email                 | -1       | HTML    | Subscribe      |
		| invalid_email_and_group       | 99999    | HTML    | Subscribe      |
		| #rand#-new@valid.com          | -1       | HTML    | Subscribe      |
		| #GlobalMasterSupressionList#  | -1       | HTML    | Subscribe      |
		| #ChannelMasterSupressionList# | -1       | HTML    | Subscribe      |
		| #MasterSupressionGroup#       | -1       | HTML    | Subscribe      |
		| #NotInTestGroup#              | -1       | HTML    | Unsubscribe    |
		| #NotInTestGroup# 2            | -1       | HTML    | Subscribe      |
		| #SubscribedInTestGroup#       | -1       | HTML    | Subscribe      |
		| #SubscribedInTestGroup# 2     | -1       | HTML    | Unsubscribe    |
		| #UnsubscribedInTestGroup#     | -1       | HTML    | Subscribe      |
		| #UnsubscribedInTestGroup# 2   | -1       | HTML    | Unsubscribe    |
	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "200 OK"
	 And HttpRequestContent should contain an object
	 And the object should be an Enumeration of SubscriptionResult
	 And the Enumeration of SubscriptionResult should validate as
		| Email Address                 | Group ID | Status | Result                                       |
		| invalid@group.com             | 99999    | None   | Skipped, InvalidGroupId                      |
		| invalid@format.com            | -1       | None   | Skipped, InvalidFormatTypeCode               |
		| invalid@subscribe-type.com    | -1       | None   | Skipped, InvalidSubscribeTypeCode            |
		| invalid@for-unsubscribe.com   | -1       | None   | Skipped, UnknownSubscriber                   |
		| invalid_email                 | -1       | None   | Skipped, InvalidEmailAddress                 |
		| invalid_email_and_group       | 99999    | None   | Skipped, InvalidEmailAddress, InvalidGroupId |
		| #rand#-new@valid.com          | -1       | S      | New, Subscribed                              |
		| #GlobalMasterSupressionList#  | -1       | M      | Skipped, MasterSuppressed                    |
		| #ChannelMasterSupressionList# | -1       | M      | Skipped, MasterSuppressed                    |
		| #MasterSupressionGroup#       | -1       | M      | Skipped, MasterSuppressed                    |
		| #NotInTestGroup#              | -1       | None   | Skipped, UnknownSubscriber                   |
		| #NotInTestGroup# 2            | -1       | S      | New, Subscribed                              |
		| #SubscribedInTestGroup#       | -1       | S      | Skipped, Duplicate                           |
		| #SubscribedInTestGroup# 2     | -1       | U      | Updated, Unsubscribed                        |
		| #UnsubscribedInTestGroup#     | -1       | S      | Updated, Subscribed                          |
		| #UnsubscribedInTestGroup# 2   | -1       | U      | Skipped, Duplicate                           |
	 And I can cleanup EmailGroup Test Records from the DataBase
