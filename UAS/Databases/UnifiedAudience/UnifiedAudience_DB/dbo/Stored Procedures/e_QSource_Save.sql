create procedure e_QSource_Save
@QSourceID int,
@QSourceValue varchar(2),
@QSourceName varchar(100),
@QsourceGroupID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	SET NOCOUNT ON

	IF @QSourceID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE QSource
			SET QSourceValue = @QSourceValue,
				QSourceName = @QSourceName,
				QsourceGroupID = @QsourceGroupID,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE QSourceID = @QSourceID;

			SELECT @QSourceID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO QSource (QSourceValue,QSourceName,QsourceGroupID,DateCreated,CreatedByUserID)
			VALUES(@QSourceValue,@QSourceName,@QsourceGroupID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
go