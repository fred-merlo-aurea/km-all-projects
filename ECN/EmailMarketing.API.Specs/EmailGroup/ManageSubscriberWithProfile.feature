Feature: ManageSubscriberWithProfile
	In order manage Email Group member profiles
	As an API user
	I want methods to control Email Group membership and update profile and user defined fields 


@EmailGroup_API
@AddSubscribersWithProfile_API
Scenario: Add Subscribers 
# hard coding group-id and API access key here to
# test that group-id is preserved in failure cases
   Given I initialize a Subscriber Profile Test-Data Provider
     And I have a valid API Access Key of "8CAB09B9-BEC9-453F-A689-E85D5C9E4898"
	 And I have a Customer ID of 1
	 And I set ControllerName to "EmailGroup"
	 And I set ActionName to "methods/ManageSubscriberWithProfile"
	 And I set Test Data Provider GroupID to 234388 
		 #  when GroupID is -1 it will be filled with the Group ID defined above.
	 And I have an Email Profile List
	| Email Address                 | Group ID | Format  | Subscribe Type | UDF 1 Name | UDF 1 Value | UDF 2 Name | UDF 2 Value | TF 1.1 N      | TF 1.1 V    | TF 1.2 N      | TF 1.2 V  | TF 2.1 N  | TF 2.1 V    | TF 2.2 N      | TF 2.2 V  |
	| #GlobalMasterSupressionList#  | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #ChannelMasterSupressionList# | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #MasterSupressionGroup#       | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| invalid@format.com            | -1       | Unknown | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| invalid@subscribe-type.com    | -1       | HTML    | MasterSuppress |            |             |            |             |               |             |               |           |           |             |               |           |
	| invalid@for-unsubscribe.com   | -1       | HTML    | Unsubscribe    |            |             |            |             |               |             |               |           |           |             |               |           |
	| invalid_email                 | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| invalid@udfs.com              | -1       | HTML    | Subscribe      | BadUDF     | BadUDF_Val  |            |             |               |             |               |           |           |             |               |           |
	| invalid2@udfs.com             | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    | BadUDF     | BadUDF_Val  |               |             |               |           |           |             |               |           |
	| invalid@udfsandtype.com       | -1       | HTML    | MasterSuppress | BadUDF     | BadUDF_Val  |            |             |               |             |               |           |           |             |               |           |
	| invalid@udfsandformat.com     | -1       | Unknown | Subscribe      | BadUDF     | BadUDF_Val  |            |             |               |             |               |           |           |             |               |           |
	| invalid@missingtfpkey.com     | -1       | HTML    | Subscribe      |            |             |            |             | Property1Name | Prop1Value  |               |           |           |             |               |           |
	| invalid2@missingtfpkey.com    | -1       | HTML    | Subscribe      |            |             |            |             | BadTF         | BadTF       |               |           |           |             |               |           |
	| invalid@tf.com                | -1       | HTML    | Subscribe      |            |             |            |             | RECORDKEY     | 54321       | BadTF         | BadTFVal  |           |             |               |           |
	| invalid@tfandtype.com         | -1       | HTML    | MasterSuppress |            |             |            |             | RECORDKEY     | 54321       | BadTF         | BadTFVal  |           |             |               |           |
	| invalid@tfandformat.com       | -1       | Unknown | Subscribe      |            |             |            |             | RECORDKEY     | 54321       | BadTF         | BadTFVal  |           |             |               |           |
	| #NotInTestGroup#              | -1       | HTML    | Unsubscribe    |            |             |            |             |               |             |               |           |           |             |               |           |
	| #NotInTestGroup# 2            | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #SubscribedInTestGroup#       | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #SubscribedInTestGroup# 2     | -1       | HTML    | Unsubscribe    |            |             |            |             |               |             |               |           |           |             |               |           |
	| #UnsubscribedInTestGroup#     | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #UnsubscribedInTestGroup# 2   | -1       | HTML    | Unsubscribe    |            |             |            |             |               |             |               |           |           |             |               |           |
	| #rand#-new01@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             |               |             |               |           |           |             |               |           |
	| #rand#-new02@valid.com        | -1       | HTML    | Subscribe      | EMPTY LIST |             |            |             |               |             |               |           |           |             |               |           |
	| #rand#-new03@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | EMPTY LIST    |             |               |           |           |             |               |           |
	| #rand#-new04@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | EMPTY SET     |             |               |           |           |             |               |           |
	| #rand#-new05@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | EMPTY SET     |             | EMPTY SET     |           |           |             |               |           |
	| #rand#-new06@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    |            |             |               |             |               |           |           |             |               |           |
	| #rand#-new07@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    | UDF2       | UDF2_Val    |               |             |               |           |           |             |               |           |
	| #rand#-new08@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | RECORDKEY     | 54321       |               |           |           |             |               |           |
	| #rand#-new09@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | RECORDKEY     | 54321       |               |           | RECORDKEY | 98765       |               |           |
	| #rand#-new10@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | RECORDKEY     | 54321       | Property1Name | Prop1Val  |           |             |               |           |
	| #rand#-new11@valid.com        | -1       | HTML    | Subscribe      |            |             |            |             | RECORDKEY     | 54321       | Property1Name | Prop1Val1 | RECORDKEY | 98765       | Property1Name | Prop1Val2 |
	| #rand#-new12@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    |            |             | RECORDKEY     | 54321       |               |           |           |             |               |           |
	| #rand#-new13@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    |            |             | RECORDKEY     | 54321       |               |           | RECORDKEY | 98765       |               |           |
	| #rand#-new14@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    |            |             | RECORDKEY     | 54321       | Property1Name | Prop1Val  |           |             |               |           |
	| #rand#-new15@valid.com        | -1       | HTML    | Subscribe      | UDF1       | UDF1_Val    |            |             | RECORDKEY     | 54321       | Property1Name | Prop1Val1 | RECORDKEY | 98765       | Property1Name | Prop1Val2 |
	| foo@bar.com                   | -1       | HTML    | Subscribe      | UDF1       | #RAND#      | UDF2       | #RAND#      | RECORDKEY     | pkey-value1 | Property1Name | t1-#RAND# | RECORDKEY | pkey-#RAND# | Property1Name | t2-#RAND# |
	#MissingTransactionalPrimaryKeyField

	When I invoke POST
	Then I should receive an HTTP Response
	 And status should be "200 OK"
	 And HttpRequestContent should contain an object
	 And the object should be an Enumeration of SubscriptionResult
	 And the Enumeration of SubscriptionResult should validate as
		| Email Address                 | Group ID | Status | Result                                                                  |
		| #GlobalMasterSupressionList#  | -1       | M      | Skipped, MasterSuppressed                                               |
		| #ChannelMasterSupressionList# | -1       | M      | Skipped, MasterSuppressed                                               |
		| #MasterSupressionGroup#       | -1       | M      | Skipped, MasterSuppressed                                               |
		| invalid@format.com            | -1       | None   | Skipped, InvalidFormatTypeCode                                          |
		| invalid@subscribe-type.com    | -1       | None   | Skipped, InvalidSubscribeTypeCode                                       |
		| invalid@for-unsubscribe.com   | -1       | None   | Skipped, UnknownSubscriber                                              |
		| invalid_email                 | -1       | None   | Skipped, InvalidEmailAddress                                            |
		| invalid@udfs.com              | -1       | None   | Skipped, UnknownCustomField                                             |
		| invalid2@udfs.com             | -1       | None   | Skipped, UnknownCustomField                                             |
		| invalid@udfsandtype.com       | -1       | None   | Skipped, InvalidSubscribeTypeCode, UnknownCustomField                   |
		| invalid@udfsandformat.com     | -1       | None   | Skipped, InvalidFormatTypeCode, UnknownCustomField                      |
		| invalid@missingtfpkey.com     | -1       | None   | Skipped, MissingTransactionalPrimaryKeyField                            |
		| invalid2@missingtfpkey.com    | -1       | None   | Skipped, UnknownTransactionalField, MissingTransactionalPrimaryKeyField |
		| invalid@tf.com                | -1       | None   | Skipped, UnknownTransactionalField                                      |
		| invalid@tfandtype.com         | -1       | None   | Skipped, InvalidSubscribeTypeCode, UnknownTransactionalField            |
		| invalid@tfandformat.com       | -1       | None   | Skipped, InvalidFormatTypeCode, UnknownTransactionalField               |
		| #NotInTestGroup#              | -1       | None   | Skipped, UnknownSubscriber                                              |
		| #NotInTestGroup# 2            | -1       | S      | New, Subscribed                                                         |
		| #SubscribedInTestGroup#       | -1       | S      | Updated, Subscribed                                                     |
		| #SubscribedInTestGroup# 2     | -1       | U      | Updated, Unsubscribed                                                   |
		| #UnsubscribedInTestGroup#     | -1       | S      | Updated, Subscribed                                                     |
		| #UnsubscribedInTestGroup# 2   | -1       | U      | Updated, Unsubscribed                                                   |
		| #rand#-new01@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new02@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new03@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new04@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new05@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new06@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new07@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new08@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new09@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new10@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new11@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new12@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new13@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new14@valid.com        | -1       | S      | New, Subscribed                                                         |
		| #rand#-new15@valid.com        | -1       | S      | New, Subscribed                                                         |
		| foo@bar.com                   | -1       | S      | Updated, Subscribed                                                     |

	 And I can cleanup EmailGroup Test Records from the DataBase


#	| invalid@format.com            | -1       | Unknown | Subscribe      | foo        |                  |            |                  |          |          |          |          |          |          |          |          |
#	| invalid@subscribe-type.com    | -1       | HTML    | MasterSuppress | foo        | bar              |            |                  |          |          |          |          |          |          |          |          |
#	| invalid@for-unsubscribe.com   | -1       | HTML    | Unsubscribe    | foo        | bar              | baz        |                  |          |          |          |          |          |          |          |          |
#	| invalid_email                 | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              |          |          |          |          |          |          |          |          |
#	| invalid_email_and_group       | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    |          |          |          |          |          |          |          |
#	| #rand#-new@valid.com          | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      |          |          |          |          |          |          |
#	| #GlobalMasterSupressionList#  | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | bbb      |          |          |          |          |          |
#	| #ChannelMasterSupressionList# | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      |          |          |          |          |          |
#	| #MasterSupressionGroup#       | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      | ddd      |          |          |          |          |
#	| #NotInTestGroup#              | -1       | HTML    | Unsubscribe    | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      | ddd      | eee      |          |          |          |
#	| #NotInTestGroup# 2            | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      | ddd      | eee      | fff      |          |          |
#	| #SubscribedInTestGroup#       | -1       | HTML    | Subscribe      | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      | ddd      | eee      | fff      | ggg      |          |
#	| #SubscribedInTestGroup# 2     | -1       | HTML    | Unsubscribe    | foo        | bar              | baz        | qwx              | xyzzy    | aaa      | ccc      | ddd      | eee      | fff      | ggg      | hhh      |
#	| invalid_email_and_group       | 99999    | HTML    | Subscribe      |            |                  |            |                  |          |          |          |          |          |          |          |          |
#	| invalid@group.com             | 99999    | HTML    | Subscribe      |            |                  |            |                  |          |          |          |          |          |          |          |          |
#		| invalid@group.com             | 99999 | None | Skipped, InvalidGroupId                            |
#		| invalid_email_and_group       | 99999 | None | Skipped, InvalidEmailAddress, InvalidGroupId       |
