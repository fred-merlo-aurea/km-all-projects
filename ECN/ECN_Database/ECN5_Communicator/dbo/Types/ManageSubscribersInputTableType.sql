CREATE TYPE [dbo].[ManageSubscribersInputTableType] AS TABLE
(
	GroupID int,
	EmailAddress varchar(255),
	FormatTypeCode varchar(16),
	SubscribeTypeCode varchar(32),
	PRIMARY KEY(GroupID,EmailAddress)
)
