CREATE PROCEDURE [dbo].[sp_GetSubscriberDimension]
	(
		@SubscriptionID int,
		@BrandID int
	)
AS
BEGIN

	SET NOCOUNT ON;
	
	if @BrandID = 0
		Begin
       select 
			mg.DisplayName, 
				MasterValue, 
				MasterDesc, 
				sd.subscriptionID 
       from 
		   SubscriptionDetails sd with (nolock) join 
		   Mastercodesheet m with (nolock) on sd.MasterID = m.MasterID join 
		   MasterGroups mg with (nolock) on mg.MasterGroupID = m.MasterGroupID join
		   subscriptions s with (nolock) on s.subscriptionID = sd.subscriptionID 
       where 
			sd.subscriptionID = @SubscriptionID
		End
	Else
		Begin
		select 
			distinct mg.DisplayName, 
				MasterValue, 
				MasterDesc, 
				v.subscriptionID 
		from   vw_BrandConsensus v with (nolock)
			   join mastercodesheet m with (nolock) on m.masterid = v.masterid 		
			   join mastergroups mg with (nolock) on mg.mastergroupid = m.mastergroupid 					   
			   join subscriptions s with (nolock)on s.subscriptionID = v.subscriptionID		
		where 
			v.subscriptionID = @SubscriptionID and v.BrandID = @BrandID 
		End
END