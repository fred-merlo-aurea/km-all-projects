CREATE function [dbo].[fnGetPubsForMarket](@MarketID int)
RETURNS VARCHAR(8000)
AS
BEGIN
	Declare @dv varchar(8000)
	set @dv = ''
	
	select @dv= @dv + p.PubName + ', ' 
	from Pubs p 
		join pubmarkets pm on p.pubID = pm.pubID 
		join PubTypes pt on p.PubTypeID = pt.PubTypeID
	where MarketID = @MarketID 
	order by pt.PubTypeDisplayName, p.pubname
	
	select @dv = case when @dv <> '' then SUBSTRING(@dv,1, len(@dv)-1) end
	return @dv
END