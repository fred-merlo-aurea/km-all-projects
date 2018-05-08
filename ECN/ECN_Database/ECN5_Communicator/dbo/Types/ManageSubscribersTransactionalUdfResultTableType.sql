CREATE TYPE [dbo].[ManageSubscribersTransactionalUdfResultTableType] AS TABLE
(
	-- from input
	SubscriberInputTableRowID uniqueidentifier,
	TransactionID uniqueidentifier,
	TransactionSequenceNumber int,
    FieldShortName varchar(50),
    FieldValue varchar(500),    
    -- from results
    EmailID int,
    -- from GroupDataFields/EmailDataValues
    GroupDataFieldsID int,
    PRIMARY KEY(SubscriberInputTableRowID, TransactionID, FieldShortName)
)
