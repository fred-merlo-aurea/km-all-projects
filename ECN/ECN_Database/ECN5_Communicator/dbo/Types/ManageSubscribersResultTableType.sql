CREATE TYPE [dbo].[ManageSubscribersResultTableType] AS TABLE
(
	EmailAddress varchar(255),
	EmailIsValid bit,
	GroupID int, 
	OriginalGroupID int, 
	EmailID int, 
	EmailGroupID int,
	OldSubscribeTypeCode varchar(32),
	OldFormatTypeCode varchar(16),
	NewSubscribeTypeCode varchar(32),
	NewFormatTypeCode varchar(16),
	GlobalMasterSuppressed bit,
	ChannelMasterSuppressed bit,
	GroupMasterSupressionSuppressed bit,
	[Status] varchar(16),
	Result varchar(max)  -- backed by a Flags/Enum on the C# side, e.g. New, Subscribed or Skipped, MasterSuppressed etc.
)
