CREATE PROCEDURE e_FilterDetail_Save
@FilterDetailId int,
@FilterId int,
@FilterTypeId int,
@FilterField varchar(50),
@AdHocFromField varchar(50),
@AdHocToField varchar(50),
@AdHocFieldValue varchar(50),
@SearchCondition varchar(50),
@FilterObjectType varchar(50),
@FilterGroupID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterDetailId> 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FilterDetail
			SET FilterId = @FilterId,
				FilterTypeId = @FilterTypeId,
				FilterField = @FilterField,
				SearchCondition = @SearchCondition,
				FilterGroupID = @FilterGroupID,
				AdHocFromField = @AdHocFieldValue,
				AdHocToField = @AdHocToField,
				AdHocFieldValue = @AdHocFieldValue,
				FilterObjectType = @FilterObjectType,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterDetailId = @FilterDetailId;
		
			SELECT @FilterDetailId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FilterDetail (FilterId,FilterTypeId,FilterField,AdHocFromField,AdHocToField,AdHocFieldValue,SearchCondition,FilterObjectType,FilterGroupID,DateCreated,CreatedByUserID)
			VALUES(@FilterId,@FilterTypeId,@FilterField,@AdHocFromField,@AdHocToField,@AdHocFieldValue,@SearchCondition,@FilterObjectType,@FilterGroupID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO