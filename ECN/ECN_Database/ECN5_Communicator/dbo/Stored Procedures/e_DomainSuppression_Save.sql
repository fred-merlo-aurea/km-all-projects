CREATE  PROC [dbo].[e_DomainSuppression_Save] 
(
	@DomainSuppressionID int = NULL,
	@BaseChannelID int = NULL,
	@CustomerID varchar(50) = NULL,
    @Domain varchar(100) = NULL,
    @IsActive bit = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @DomainSuppressionID is NULL or @DomainSuppressionID <= 0
	BEGIN
		INSERT INTO DomainSuppression
		(
			BaseChannelID, CustomerID, Domain, IsActive, CreatedUserID, CreatedDate, IsDeleted
		)
		VALUES
		(
			@BaseChannelID, @CustomerID, @Domain, @IsActive, @UserID, GetDate(), 0
		)
		SET @DomainSuppressionID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE DomainSuppression
			SET BaseChannelID=@BaseChannelID, CustomerID=@CustomerID, Domain=@Domain, IsActive=@IsActive,
				UpdatedUserID=@UserID, UpdatedDate=GETDATE()
		WHERE
			DomainSuppressionID = @DomainSuppressionID
	END

	SELECT @DomainSuppressionID
END