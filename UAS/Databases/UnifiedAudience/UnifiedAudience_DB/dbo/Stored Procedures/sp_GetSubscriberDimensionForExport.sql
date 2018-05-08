CREATE PROCEDURE [dbo].[sp_GetSubscriberDimensionForExport]
		@SubscriptionID int,
		@BrandID int
AS
BEGIN

	SET NOCOUNT ON;
	
	if @brandID = 0
		Begin
		SELECT 
			mg.displayname, 
				   x.mastervalue, 
				   x.subscriptionid, 
				   x.masterdesc  
		FROM
			RecordViewField rvf with (nolock) JOIN
			mastergroups mg with (nolock) ON mg.mastergroupid = rvf.mastergroupid left outer join
		(
		SELECT distinct
			masterGroupID,
			mastervalue, 
			sd.subscriptionid, 
			masterdesc 
		FROM   
			subscriptions s with (nolock) JOIN
			subscriptiondetails sd with (nolock) ON s.subscriptionid = sd.subscriptionid JOIN
			mastercodesheet m with (nolock) ON sd.masterid = m.masterid
		WHERE  
			sd.subscriptionid = @SubscriptionID 
				) x on rvf.MasterGroupID = x.mastergroupid
			order by rvf.RecordViewFieldID
		End
	Else
		Begin
		SELECT 
			mg.displayname, 
				   x.mastervalue, 
				   x.subscriptionid, 
				   x.masterdesc  
		FROM
			RecordViewField rvf with (nolock) JOIN 
			mastergroups mg with (nolock) ON mg.mastergroupid = rvf.mastergroupid left outer join
		(
  	    SELECT  distinct
			masterGroupID,
			mastervalue, 
			s.subscriptionid, 
			masterdesc 
		FROM   
			subscriptions s with (nolock) JOIN
			vw_BrandConsensus v WITH (nolock) on v.SubscriptionID = s.SubscriptionID join
			BrandDetails bd with (nolock) on bd.BrandID = v.BrandID join
			mastercodesheet mc  with (nolock) ON mc.masterid = v.masterid 
		WHERE  
			s.subscriptionid = @SubscriptionID and bd.BrandID = @BrandID 
				) x on rvf.MasterGroupID = x.mastergroupid
			order by rvf.RecordViewFieldID
		End
END