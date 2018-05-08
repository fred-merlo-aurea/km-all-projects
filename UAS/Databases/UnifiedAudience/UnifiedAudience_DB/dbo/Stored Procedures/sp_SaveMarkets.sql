CREATE PROCEDURE [dbo].[sp_SaveMarkets]
(
	@MarketID int, 
	@pubIDs varchar(8000), 
	@MasterIDs varchar(8000), 
	@MarketName varchar(100)
)
AS
BEGIN

	SET NOCOUNT ON

	declare @mid int

	if @MarketID=0
		begin
			INSERT INTO Markets (MarketName) values (@MarketName)
			set @mid = (select @@IDENTITY )  
			
			-- insert into pub market
		    INSERT INTO PubMarkets (MarketID,PubID) 
				SELECT @mid as MarketID, Items as PubID 
				from fn_Split(@pubIDs,',')
			
			INSERT INTO MasterMarkets (MarketID,MasterID) 
				SELECT @mid as MarketID, Items as PubID 
				from fn_Split(@MasterIDs,',')	
			
		end
     else
        begin
			DELETE 
			FROM PubMarkets 
			where MarketID = @MarketID

			DELETE 
			FROM MasterMarkets 
			where MarketID = @MarketID
			
			UPDATE Markets 
				SET MarketName = @MarketName 
				where MarketID = @MarketID
			
			--insert into PubMarket
			INSERT INTO PubMarkets (MarketID,PubID) 
				SELECT @MarketID, Items as PubID 
				from fn_Split(@pubIDs,',') 
				
			INSERT INTO MasterMarkets (MarketID,MasterID) 
				SELECT @MarketID, Items as PubID 
				from fn_Split(@MasterIDs,',') 	
        end

END