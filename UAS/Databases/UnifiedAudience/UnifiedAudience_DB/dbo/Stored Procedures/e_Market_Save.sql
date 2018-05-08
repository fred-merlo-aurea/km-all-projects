CREATE PROCEDURE [dbo].[e_Market_Save]
	@MarketID int,
	@MarketName varchar(100),
	@MarketXML xml,
	@BrandID int,
	@DateCreated DateTime,
	@DateUpdated DateTime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	if (@BrandID = 0)
		begin
			set @BrandID = null;
		end

	if (@DateCreated is null)
		begin
			set @DateCreated = GETDATE()
		end

	if (@DateUpdated is null)
		begin
			set @DateUpdated = GETDATE()
		end
	
	if (@MarketID > 0)
		Begin
			Update Markets 
				set MarketName = @MarketName,
					MarketXML = @MarketXML,
					BrandID = @BrandID,
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID
			where MarketID = @MarketID

			select @MarketID;
		End		
	else
		Begin
			INSERT INTO Markets (MarketName,MarketXML,BrandID,DateCreated,CreatedByUserID) 
			values (@MarketName,@MarketXML,@BrandID,@DateCreated,@CreatedByUserID)

			select @@IDENTITY 
		End	
	
END