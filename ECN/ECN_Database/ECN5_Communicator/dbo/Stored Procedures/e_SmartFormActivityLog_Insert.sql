CREATE  PROC [dbo].[e_SmartFormActivityLog_Insert] 
(
	@SFID int,
	@CustomerID int,
	@GroupID int,
	@EmailID int,
	@EmailType varchar(10),
	@EmailTo varchar(500),
	@EmailFrom varchar(500),
	@EmailSubject varchar(500),
	@SendTime datetime,
	@UserID int
)
AS 
BEGIN
	INSERT INTO SmartFormActivityLog
	(
		SFID,CustomerID,GroupID,EmailID,EmailType,EmailFrom,EmailTo,EmailSubject,SendTime,CreatedUserID,CreatedDate,IsDeleted
	)
	VALUES
	(
		@SFID,@CustomerID,@GroupID,@EmailID,@EmailType,@EmailFrom,@EmailTo,@EmailSubject,@SendTime,@UserID,GetDate(),0
	)
	SELECT @@IDENTITY
END
