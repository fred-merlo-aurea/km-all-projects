CREATE PROCEDURE [dbo].[Dashboard_getProductTypeDistributionList]
(
	@brandID int = 0
)
AS
Begin
	if @brandID = 0
	Begin
		select pt.pubtypeID, pt.PubTypeDisplayName, COUNT(distinct p.PubID) as ProductCounts, isnull([Counts], 0) as [Counts] 
		from 
				dbo.[Summary_Data] s with (NOLOCK)  join 
				PubTypes pt with (NOLOCK)  on pt.PubTypeID = s.PubTypeID join
				pubs p with (NOLOCK) on pt.pubtypeID = p.PubTypeID
		where
				 entityName='ProductType' and [Type]='Net' and isnull(brandID,0) =0 
		group by pt.pubtypeID, pt.PubTypeDisplayName, [Counts]
		order by PubTypeDisplayName

	end
	Else
	Begin
		select pt.pubtypeID, pt.PubTypeDisplayName, COUNT(distinct p.PubID) as ProductCounts, isnull([Counts],0) as [Counts] 
		from 
				dbo.[Summary_Data] s with (NOLOCK)  join 
				PubTypes pt with (NOLOCK)  on pt.PubTypeID = s.PubTypeID join
				Pubs p with (NOLOCK)  on p.PubTypeID = pt.PubTypeID   join
				brandDetails bd on p.pubID = bd.pubID 
				
		where
				 s.brandID = @brandID and bd.brandID = @brandID and entityName='ProductType' and [Type]='Net' 
		group by pt.pubtypeID, pt.PubTypeDisplayName, [Counts] 
		order by PubTypeDisplayName

	End

end