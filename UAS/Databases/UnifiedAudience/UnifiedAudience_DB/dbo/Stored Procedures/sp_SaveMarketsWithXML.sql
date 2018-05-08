CREATE PROCEDURE [dbo].[sp_SaveMarketsWithXML]
(
	@MarketID int,
	@MarketName varchar(100), 
	@CurrentXML xml,
	@BrandID int
)
AS
BEGIN

	SET NOCOUNT ON


	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	
	if @MarketID=0
		begin
			INSERT INTO Markets (MarketName, MarketXML, BrandID) 
			values (@MarketName, @CurrentXML, @BrandID)
			select @@IDENTITY 			
		end
     else
        begin
			UPDATE Markets 
			SET MarketName = @MarketName, MarketXML = CAST(@CurrentXML as XML), BrandID = @BrandID 
			where MarketID = @MarketID

			select  @MarketID
        end

END