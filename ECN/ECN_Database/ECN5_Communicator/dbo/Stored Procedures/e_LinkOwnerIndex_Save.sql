CREATE  PROC [dbo].[e_LinkOwnerIndex_Save] 
(
	@LinkOwnerIndexID int = NULL,
    @CustomerID int = NULL,
    @LinkOwnerName varchar(50) = NULL,
    @LinkOwnerCode varchar(10) = NULL,
    @ContactFirstName varchar(50) = NULL,
    @ContactLastName varchar(50) = NULL,
    @ContactName varchar(50) = NULL,
    @ContactPhone varchar(50) = NULL,
    @ContactEmail varchar(50) = NULL,
    @Address varchar(100) = NULL,
    @City varchar(50) = NULL,
    @State varchar(50) = NULL,
    @IsActive bit = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @LinkOwnerIndexID is NULL or @LinkOwnerIndexID <= 0
	BEGIN
		INSERT INTO LinkOwnerIndex
		(
			CustomerID,LinkOwnerName,LinkOwnerCode,ContactFirstName,ContactLastName,ContactPhone,ContactEmail,[Address],City,[State],IsActive,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@CustomerID,@LinkOwnerName,@LinkOwnerCode,@ContactFirstName,@ContactLastName,@ContactPhone,@ContactEmail,@Address,@City,@State,@IsActive,@UserID,GetDate(),0
		)
		SET @LinkOwnerIndexID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE LinkOwnerIndex
			SET CustomerID=@CustomerID, LinkOwnerName=@LinkOwnerName, LinkOwnerCode=@LinkOwnerCode, ContactFirstName=@ContactFirstName,
			 ContactLastName=@ContactLastName, ContactPhone=@ContactPhone, ContactEmail=@ContactEmail,
			 [Address]=@Address, City=@City, [State]=@State, IsActive=@IsActive, UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			LinkOwnerIndexID = @LinkOwnerIndexID
	END

	SELECT @LinkOwnerIndexID
END