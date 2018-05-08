CREATE PROCEDURE [dbo].[e_Adhoc_Save]
	@AdhocID int,
	@AdhocName varchar(50),    
	@CategoryID int,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @AdhocID > 0
		BEGIN						
			UPDATE Adhoc
			SET AdhocName = @AdhocName,
				CategoryID = @CategoryID,
				SortOrder = @SortOrder
			WHERE AdhocID = @AdhocID;
		
			SELECT @AdhocID;
		END
	ELSE
		BEGIN
			INSERT INTO Adhoc (AdhocName,CategoryID,SortOrder)
			VALUES(@AdhocName,@CategoryID,@SortOrder);SELECT @@IDENTITY;
		END		

END