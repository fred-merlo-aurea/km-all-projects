CREATE  PROC [dbo].[e_AutoResponders_Save] 
(
	@AutoResponderID int = NULL,
    @BlastID int = NULL,
    @MailServer varchar(255) = NULL,
    @AccountName varchar(255) = NULL,
    @AccountPasswd varchar(255) = NULL,
    @ForwardTo varchar(255) = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @AutoResponderID is NULL or @AutoResponderID <= 0
	BEGIN
		INSERT INTO AutoResponders
		(
			BlastID,MailServer,AccountName,AccountPasswd,ForwardTo,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@BlastID,@MailServer,@AccountName,@AccountPasswd,@ForwardTo,@UserID,GetDate(),0
		)
		SET @AutoResponderID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE AutoResponders
			SET BlastID=@BlastID,MailServer=@MailServer,AccountName=@AccountName,AccountPasswd=@AccountPasswd,
				ForwardTo=@ForwardTo,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			AutoResponderID = @AutoResponderID
	END

	SELECT @AutoResponderID
END