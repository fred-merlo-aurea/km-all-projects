CREATE PROCEDURE [dbo].[sp_SubscriberScores]
@BrandID int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	if (@brandID = 0)
	Begin
		select top 10 CAST((score/10)*10 as varchar) + '-' +  CAST((score/10)*10+9 as varchar)  as 'range', count(subscriptionID)  as 'subscribercount'
		from Subscriptions  with (NOLOCK)
		where CAST((score/10)*10 as varchar) + '-' +  CAST((score/10)*10+9 as varchar)<>'0-9'
		group by score/10
		order by (score/10)*10
	end
	Else
	Begin
		select top 10 CAST((score/10)*10 as varchar) + '-' +  CAST((score/10)*10+9 as varchar)  as 'range', count(BrandScoreID)  as 'subscribercount'
		from brandscore  with (NOLOCK)
		where BrandID = @BrandID and CAST((score/10)*10 as varchar) + '-' +  CAST((score/10)*10+9 as varchar)<>'0-9' 
		group by score/10
		order by (score/10)*10

	End
END
GO