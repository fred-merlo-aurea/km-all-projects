CREATE  PROC [dbo].[e_BlastEnvelopes_Save] 
(
	@BlastEnvelopeID int = NULL,
	@CustomerID int = NULL,
	@FromName varchar(100) = NULL,
	@FromEmail varchar(100) = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	IF @BlastEnvelopeID is NULL or @BlastEnvelopeID <= 0
	BEGIN
		INSERT INTO BlastEnvelopes
		(
			CustomerID, FromName, FromEmail, CreatedUserID, CreatedDate, UpdatedUserID, UpdatedDate, IsDeleted
		)
		VALUES
		(
			@CustomerID, @FromName, @FromEmail, @UserID, GetDate(), @UserID, GetDate(), 0
		)
		SET @BlastEnvelopeID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE BlastEnvelopes
			SET FromName=@FromName,FromEmail=@FromEmail,
			UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			BlastEnvelopeID = @BlastEnvelopeID
	END

	SELECT @BlastEnvelopeID
END