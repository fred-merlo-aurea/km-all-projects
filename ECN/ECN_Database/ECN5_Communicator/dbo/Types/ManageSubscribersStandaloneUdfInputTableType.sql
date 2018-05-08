CREATE TYPE [dbo].[ManageSubscribersStandaloneUdfInputTableType] AS TABLE
(
	SubscriberInputTableRowID uniqueidentifier,
    FieldShortName varchar(50),
    FieldValue varchar(500),
    PRIMARY KEY(SubscriberInputTableRowID, FieldShortName)
)
