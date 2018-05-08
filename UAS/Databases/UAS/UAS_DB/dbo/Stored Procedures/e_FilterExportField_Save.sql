CREATE PROCEDURE e_FilterExportField_Save
@FilterExportFieldId int,
@FilterScheduleId int,
@ExportColumn varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterExportFieldId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FilterExportField
			SET FilterScheduleId = @FilterScheduleId,
				ExportColumn = @ExportColumn,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterExportFieldId = @FilterExportFieldId;
		
			SELECT @FilterExportFieldId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FilterExportField (FilterScheduleId,ExportColumn,DateCreated,CreatedByUserID)
			VALUES(@FilterScheduleId,@ExportColumn,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO