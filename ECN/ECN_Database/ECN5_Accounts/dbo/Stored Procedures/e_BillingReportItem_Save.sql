CREATE PROCEDURE [dbo].[e_BillingReportItem_Save]
	@BillingReportID int,
	@BillingReportItemID int = null,
	@Amount decimal(15,2),
	@IsFlatRateItem bit,
	@ItemName varchar(100),
	@BaseChannelName varchar(100),
	@BaseChannelID int,
	@CustomerName varchar(100),
	@CustomerID int,
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@IsDeleted bit 
AS
	IF (@BillingReportItemID is not null and @BillingReportItemID > 0)
	BEGIN
		UPDATE BillingReportItem
		SET Amount = @Amount, InvoiceText = @ItemName,IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID
		WHERE BillingItemID = @BillingReportItemID
		SELECT @BillingReportID
	END
	ELSE
	BEGIN
		INSERT INTO BillingReportItem(Amount, BillingReportID, IsFlatRateItem, InvoiceText, BaseChannelID, BaseChannelName, CustomerID, CustomerName, IsDeleted, CreatedDate, CreatedUserID)
		VALUES(@Amount, @BillingReportID, @IsFlatRateItem, @ItemName, @BaseChannelID, @BaseChannelName, @CustomerID, @CustomerName, @IsDeleted, GETDATE(), @CreatedUserID)
		SELECT @@IDENTITY;
	END