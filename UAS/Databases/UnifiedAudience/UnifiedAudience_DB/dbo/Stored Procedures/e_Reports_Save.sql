CREATE PROCEDURE [dbo].[e_Reports_Save]
@ReportID int,
@ReportName varchar(200),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ProvideID bit,
@ProductID int,
@URL varchar(250),
@IsCrossTabReport bit,
@Row varchar(50),
@Column varchar(50),
@SuppressTotal bit,
@Status bit,
@ReportTypeID int
AS
BEGIN

	SET NOCOUNT ON

	IF @ReportID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE Reports
			SET ReportName = @ReportName,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				ProvideID = @ProvideID,
				ProductID = @ProductID,
				URL = @URL,
				IsCrossTabReport = @IsCrossTabReport,
				[Row] = @Row,
				[Column] = @Column,
				SuppressTotal = @SuppressTotal,
				[Status] = @Status,
				ReportTypeID = @ReportTypeID
			WHERE ReportID = @ReportID;
		
			SELECT @ReportID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Reports (ReportName,IsActive,DateCreated,CreatedByUserID,ProvideID,ProductID,URL,IsCrossTabReport,[Row],[Column],SuppressTotal,[Status],ReportTypeID)
			VALUES(@ReportName,@IsActive,@DateCreated,@CreatedByUserID,@ProvideID,@ProductID,@URL,@IsCrossTabReport,@Row,@Column,@SuppressTotal,@Status,@ReportTypeID);SELECT @@IDENTITY;
		END

END