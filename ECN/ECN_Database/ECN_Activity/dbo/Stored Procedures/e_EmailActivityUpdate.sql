CREATE PROCEDURE [dbo].[e_EmailActivityUpdate]
	@OldEmailAddress VARCHAR(255),
	@NewEmailAddress VARCHAR(255),
	@GroupID int,
	@CustomerID int,
	@FormID int,
	@Comments VARCHAR(355)
AS
	BEGIN

	DECLARE @OldEmailID int
	DECLARE @NewEmailID int
	select @OldEmailID=e.EmailID from ecn5_communicator..Emails e with (NOLOCK)
	JOIN ecn5_communicator..EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID 
	where  e.EmailAddress = @OldEmailAddress and e.CustomerID = @CustomerID and eg.GroupID= @GroupID
	select @NewEmailID=e.EmailID from ecn5_communicator..Emails e with (NOLOCK)
	JOIN ecn5_communicator..EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID 
	where  e.EmailAddress = @NewEmailAddress and e.CustomerID = @CustomerID and eg.GroupID= @GroupID


	INSERT INTO [EmailActivityUpdate]
		(
			OldEmailID,OldEmailAddress,NewEmailID,NewEmailAddress,UpdateTime,ApplicationSourceID,SourceID,Comments
		)
		VALUES
		(
			@OldEmailID,
			@OldEmailAddress,
			@NewEmailID,
			@NewEmailAddress,
			GETDATE(),
			96,
			@FormID,
			@Comments
		)
	END
RETURN 0
