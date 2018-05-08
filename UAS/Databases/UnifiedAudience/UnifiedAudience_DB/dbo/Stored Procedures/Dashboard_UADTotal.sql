CREATE PROCEDURE [dbo].[Dashboard_UADTotal]
(
	@brandID int = 0
)
AS
Begin
	
	declare @maxdate date
	
	if @brandID = 0
	Begin

		select @maxdate = MAX(date) from Summary_Data with (NOLOCK)
		where isnull(BrandID,0) = 0 and EntityName='Subscriptions' and Type='UniqueSubscriber'
	
		select EntityName, Counts from Summary_Data with (NOLOCK)
		where isnull(BrandID,0) = 0 and EntityName='Subscriptions' and Type='UniqueSubscriber' and DateMonth = MONTH(@maxdate) and DateYear = YEAR(@maxdate) 
		union
		select EntityName, Counts from Summary_Data  with (NOLOCK)
		where isnull(BrandID,0) = 0 and Type='Net' and CountsType = 'cumulative' and DateMonth = MONTH(@maxdate) and DateYear = YEAR(@maxdate)
	
	End	
	Else
	Begin

		select @maxdate = MAX(date) from Summary_Data with (NOLOCK)
		where BrandID = @BrandID and EntityName='Subscriptions' and Type='UniqueSubscriber'

		select EntityName, Counts from Summary_Data with (NOLOCK)
		where 
				BrandID = @BrandID and
				EntityName='Subscriptions' and 
				Type='UniqueSubscriber' and 
				DateMonth = MONTH(@maxdate) and 
				DateYear = YEAR(@maxdate) 
		union
		select EntityName, sum(Counts)
		from 
		dbo.[Summary_Data] sd with (NOLOCK) join
		BrandDetails bd on bd.pubID = sd.pubID 
		where	
				bd.BrandID = @brandID and 
				[Type] = 'New' and 
				Entityname in ('PubSubscriptions','SubscriberOpenActivity','SubscriberClickActivity') and
				sd.[Date] <= @maxdate
		group by EntityName
		union
		select EntityName, sum(Counts)
		from 
		dbo.[Summary_Data] sd with (NOLOCK) 
		where	
				sd.BrandID = @brandID and 
				[Type] = 'New' and 
				Entityname in ('SubscriberVisitActivity') and
				sd.[Date] <= @maxdate
		group by EntityName

	End
End
GO