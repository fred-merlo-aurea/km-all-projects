CREATE TYPE [dbo].[ManageSubscribersTransactionalUdfInputTableType] AS TABLE
(
	SubscriberInputTableRowID uniqueidentifier,
    TransactionID uniqueidentifier,
    TransactionSequenceNumber int,
    FieldShortName varchar(50),
    FieldValue varchar(500),
    PRIMARY KEY(SubscriberInputTableRowID, TransactionID, FieldShortName)
)
