CREATE PROCEDURE [dbo].[spGetWebSiteAndPromocodeByGroupID]
 @GroupID int,   
 @Website varchar(250),  
 @PromoCode varchar(250), 
 @DiscountForEveryPromocode bit = 0
AS
BEGIN  
 if @DiscountForEveryPromocode = 1 and ltrim(rtrim(@Website)) <> '' -- check multi company discount if website is passed
 begin
	 select
		top 1 e.EmailID, e.EmailAddress, e.Website
	 from   
		emails e with(nolock)  
		join EmailGroups eg with(nolock) on e.EmailID = eg.EmailID
	 where
		eg.GroupID = @GroupID
		and e.Website = @Website
 end
 else
 begin
	 --select 
		--top 1 e.Website, edv.DataValue as 'Promocode'
	 --from   
		--emails e   
		--join EmailGroups eg on e.EmailID = eg.EmailID
		--join EmailDataValues edv on edv.EmailID = e.EmailID
		--join GroupDatafields gdf on gdf.GroupDatafieldsID = edv.GroupDatafieldsID
	 --where
		--eg.GroupID = @GroupID   
		--and e.Website = @Website   
		--and edv.DataValue = @PromoCode
		--and gdf.DatafieldSetID is null and gdf.ShortName = 'promoCode'
		--and edv.DataValue is not null
		
	select '' as 'Website'		
 end     
END
