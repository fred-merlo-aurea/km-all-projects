
CREATE PROCEDURE [dbo].[e_Reports_Save]
@ReportID int,
@ReportName varchar(200),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ProvideID bit
AS

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
			ProvideID = @ProvideID
		WHERE ReportID = @ReportID;
		
		SELECT @ReportID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Reports (ReportName,IsActive,DateCreated,CreatedByUserID,ProvideID)
		VALUES(@ReportName,@IsActive,@DateCreated,@CreatedByUserID,@ProvideID);SELECT @@IDENTITY;
	END

