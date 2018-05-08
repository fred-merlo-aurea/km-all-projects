CREATE TYPE [dbo].[ManageSubscribersStandaloneUdfResultTableType] AS TABLE
(
	-- from input
	SubscriberInputTableRowID uniqueidentifier,
    FieldShortName varchar(50),
    FieldValue varchar(500),    
    -- from @results
    EmailID int,
    -- from GroupDataFields/EmailDataValues
    GroupDataFieldsID int,
    OldFieldValue varchar(500),
    PRIMARY KEY(SubscriberInputTableRowID, FieldShortName)
)
