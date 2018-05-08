CREATE PROCEDURE [dbo].[e_BillingReport_Save]
	@BillingReportID int = -1,
	@CustomerIDs varchar(100),
	@ReportName varchar(100),
	@IncludeFulfillment bit,
	@IncludeMasterFile bit,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@IsRecurring bit,
	@RecurrenceType varchar(10),
	@EmailBillingRate decimal = 0.0,
	@MasterFileRate decimal = 0.0,
	@FulfillmentRate decimal = 0.0,
	@FromEmail varchar(50),
	@FromName varchar(50),
	@ToEmail varchar(50),
	@Subject varchar(100),
	@IsDeleted bit,
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@BaseChannelID int,
	@BlastFields varchar(200),
	@AllCustomers bit
AS
BEGIN
	if(@BillingReportID > 0)
	BEGIN
		UPDATE BillingReport
		SET CustomerIDs = @CustomerIDs, BillingReportName = @ReportName,FromName = @FromName, IncludeFulfillment = @IncludeFulfillment, IncludeMasterFile = @IncludeMasterFile, StartDate = @StartDate, EndDate = @EndDate, IsRecurring = @IsRecurring, RecurrenceType = @RecurrenceType, EmailBillingRate = @EmailBillingRate, MasterFileRate = @MasterFileRate, FulfillmentRate = @FulfillmentRate, FromEmail = @FromEmail, ToEmail = @ToEmail, Subject = @Subject, IsDeleted = @IsDeleted, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID, BlastFields = @BlastFields, AllCustomers = @AllCustomers, BaseChannelID = @BaseChannelID
		WHERE BillingReportID = @BillingReportID and IsDeleted = 0
		SELECT @BillingReportID
	END
	else
	BEGIN
	INSERT INTO BillingReport(CustomerIDs, BillingReportName, IncludeFulfillment, IncludeMasterFile,StartDate,EndDate, IsRecurring, RecurrenceType, EmailBillingRate, MasterFileRate, FulfillmentRate, FromEmail,FromName, ToEmail, Subject, IsDeleted, CreatedDate, CreatedUserID, BlastFields, AllCustomers, BaseChannelID)
	Values(@CustomerIDs, @ReportName, @IncludeFulfillment, @IncludeMasterFile,@StartDate, @EndDate, @IsRecurring, @RecurrenceType, @EmailBillingRate, @MasterFileRate, @FulfillmentRate, @FromEmail,@FromName, @ToEmail, @Subject, @IsDeleted, GETDATE(), @CreatedUserID, @BlastFields, @AllCustomers, @BaseChannelID)
	select @@IDENTITY;
	END
END