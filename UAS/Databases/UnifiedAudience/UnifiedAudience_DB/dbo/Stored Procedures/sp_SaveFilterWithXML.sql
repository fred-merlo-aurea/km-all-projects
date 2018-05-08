CREATE PROCEDURE [dbo].[sp_SaveFilterWithXML]
(
	@FilterName varchar(100), 
	@CurrentXML xml,
	@UserID int,
	@FilterType varchar(50),
	@PubID int,
	@BrandID int
)
AS
BEGIN

	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	
	INSERT INTO Filters (Name, FilterXML, FilterType, PubID, CreatedUserID, CreatedDate, BrandID) values (@FilterName, @CurrentXML, @FilterType, @PubID, @UserID, GETDATE(), @BrandID)
	select @@IDENTITY 
	
END