CREATE  PROC [dbo].[e_MessageType_Save] 
(
	@MessageTypeID int = NULL,
    @BaseChannelID int = NULL,
    @Name varchar(50) = NULL,
    @Description varchar(255) = NULL,
    @Threshold bit = NULL,
    @Priority bit = NULL,
    @PriorityNumber int = NULL,
    @SortOrder int = NULL,
    @IsActive bit = NULL,
    @CustomerID bit = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @MessageTypeID is NULL or @MessageTypeID <= 0
	BEGIN
		INSERT INTO MessageType
		(
			BaseChannelID, Name, [Description], Threshold, Priority, PriorityNumber, SortOrder, IsActive, CreatedUserID, CustomerID ,CreatedDate, IsDeleted
		)
		VALUES
		(
			@BaseChannelID, @Name, @Description, @Threshold, @Priority, @PriorityNumber, @SortOrder, @IsActive, @UserID,@CustomerID , GetDate(), 0
		)
		SET @MessageTypeID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE MessageType
			SET BaseChannelID=@BaseChannelID,Name=@Name,[Description]=@Description,Threshold=@Threshold,Priority=@Priority,PriorityNumber=@PriorityNumber,
			SortOrder=@SortOrder,IsActive=@IsActive,UpdatedUserID=@UserID,CustomerID=@CustomerID, UpdatedDate=GETDATE()
		WHERE
			MessageTypeID = @MessageTypeID
	END

	SELECT @MessageTypeID
END