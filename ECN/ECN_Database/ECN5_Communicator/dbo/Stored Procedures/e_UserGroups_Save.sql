CREATE  PROC [dbo].[e_UserGroups_Save] 
(
	@UGID int = 0,
	@UserID int,
	@GroupID int,
	@LoggingUserID int = null
)
AS 
BEGIN
	
	if @UGID <= 0
		select @UGID = UGID from UserGroups where UserID = @UserID and GroupID = @GroupID and ISNULL(IsDeleted , 0) = 0
		
	if @UGID <= 0
	BEGIN
		INSERT INTO UserGroups
		(
			UserID, GroupID, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@UserID, @GroupID, @LoggingUserID, GetDate(),0
		)
		SET @UGID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE UserGroups
			SET UserID=@UserID,GroupID=@GroupID,UpdatedUserID=@LoggingUserID,UpdatedDate=GETDATE()
		WHERE
			UGID = @UGID
	END

	SELECT @UGID
END