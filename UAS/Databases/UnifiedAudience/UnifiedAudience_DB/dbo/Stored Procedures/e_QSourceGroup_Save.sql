create procedure e_QSourceGroup_Save
@QSourceGroupID int,
@QSourceGroupName varchar(100),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
as
BEGIN

	SET NOCOUNT ON

	IF @QSourceGroupID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE QSourceGroup
			SET QSourceGroupName = @QSourceGroupName,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE QSourceGroupID = @QSourceGroupID;

			SELECT @QSourceGroupID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO QSourceGroup (QSourceGroupName,DateCreated,CreatedByUserID)
			VALUES(@QSourceGroupName,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
go