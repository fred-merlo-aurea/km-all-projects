CREATE PROCEDURE e_FilterDetailSelectedValue_Save
@FilterDetailSelectedValueId int,
@FilterDetailId int,
@SelectedValue varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterDetailSelectedValueId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FilterDetailSelectedValue
			SET FilterDetailId = @FilterDetailId,
				SelectedValue = @SelectedValue,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterDetailSelectedValueId = @FilterDetailSelectedValueId;
		
			SELECT @FilterDetailSelectedValueId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FilterDetailSelectedValue (FilterDetailId,SelectedValue,DateCreated,CreatedByUserID)
			VALUES(@FilterDetailId,@SelectedValue,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO