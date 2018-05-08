CREATE PROCEDURE [dbo].[sp_GetSubscriberPubsForExport]
	(
		@SubscriptionID int,
		@BrandID int
	)
AS
BEGIN

	SET NOCOUNT ON;
	
	if @BrandID = 0
		Begin
			select PubCode, 
				pubname,  
				pt.ColumnReference, 
				subscriptionID, 
				pubtypedisplayname,
				PubSubscriptionID
			from PubSubscriptions ps with(nolock) 
				join Pubs p with(nolock) on ps.PubID = p.PubID 
				join PubTypes pt with(nolock) on p.PubTypeID = pt.PubTypeID 
			where subscriptionID = @SubscriptionID  
			order by pt.sortorder
		End
	Else
		Begin
			 select PubCode, 
				pubname,  
				pt.ColumnReference, 
				subscriptionID, 
				pubtypedisplayname,
				PubSubscriptionID
			from PubSubscriptions ps 
				join Pubs p with(nolock) on ps.PubID = p.PubID 
				join PubTypes pt with(nolock) on p.PubTypeID = pt.PubTypeID  
				join BrandDetails bd with(nolock) on bd.pubID = ps.PubID 
				join Brand b with(nolock) on b.BrandID = bd.BrandID 
			where subscriptionID = @SubscriptionID and bd.BrandID = @BrandID and b.IsDeleted = 0
			order by pt.sortorder
		End

END