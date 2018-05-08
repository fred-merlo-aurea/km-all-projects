CREATE PROCEDURE [dbo].[e_AdhocCategory_Save]
	@CategoryID int,
	@CategoryName varchar(50),
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @CategoryID > 0
		BEGIN						
			UPDATE AdhocCategory
			SET CategoryName = @CategoryName,
				SortOrder = @SortOrder
			WHERE CategoryID = @CategoryID;
		
			SELECT @CategoryID;
		END
	ELSE
		BEGIN
			INSERT INTO AdhocCategory (CategoryName,SortOrder)
			VALUES(@CategoryName,@SortOrder);SELECT @@IDENTITY;
		END

END