CREATE  PROC [dbo].[e_Sample_Save] 
(
	@SampleID int = NULL,
	@SampleName varchar(255) = NULL,
	@CustomerID int = NULL,
	@WinningBlastID int = NULL,
	@UserID int = NULL,
	@ABWinnerType varchar(10) = 'clicks',
	@DidNotReceiveAB bit = 1,
	@DeliveredOrOpened varchar(50)
)
AS 
BEGIN
	IF @SampleID is NULL or @SampleID <= 0
	BEGIN
		INSERT INTO [Sample]
		(
			SampleName,CustomerID,CreatedUserID,CreatedDate,IsDeleted, WinningBlastID,ABWInnerType, DidNotReceiveAB, DeliveredOrOpened
		)
		VALUES
		(
			@SampleName,@CustomerID,@UserID,GetDate(),0, @WinningBlastID, @ABWInnerType, @DidNotReceiveAB, @DeliveredOrOpened
		)
		SET @SampleID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE [Sample]
			SET SampleName=@SampleName,CustomerID=@CustomerID,UpdatedUserID=@UserID,UpdatedDate=GETDATE(), WinningBlastID=@WinningBlastID, ABWinnerType=@ABWinnerType, DidNotReceiveAB = @DidNotReceiveAB, DeliveredOrOpened = @DeliveredOrOpened
		WHERE
			SampleID = @SampleID
	END
	SELECT @SampleID
END