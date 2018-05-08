CREATE PROCEDURE [dbo].[e_Filter_Save]
	@FilterID int,
	@FilterName varchar(25),
	@FilterDetails TEXT,
	@ProductID int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF (@FilterID > 0)
		BEGIN
			UPDATE Filter
				SET 
				FilterDetails = @FilterDetails,			
				FilterName = @FilterName,
				ProductID = @ProductID,
				DateCreated = @DateCreated,
				DateUpdated = @DateUpdated,
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID
			
			WHERE FilterID = @FilterID

			SELECT @FilterID;
		END	
	ELSE
		BEGIN
			INSERT INTO Filter (FilterName, FilterDetails, ProductID, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID) 
			values (@FilterName, @FilterDetails, @ProductID, @DateCreated, @DateUpdated, @CreatedByUserID, @UpdatedByUserID)

			SELECT @@IDENTITY 
		END	
	
END