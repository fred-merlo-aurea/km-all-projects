CREATE PROCEDURE o_HasServiceFeature_ClientID_ServiceName_FeatureName
@ClientID int,
@ServiceName varchar(100),
@FeatureName varchar(100)
AS
	select ISNULL(cgsfm.IsEnabled,'false') 
	from Client c with(nolock)
	join ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	join ClientGroup cg with(nolock) on cg.ClientGroupID = cgcm.ClientGroupID
	join ClientGroupServiceFeatureMap cgsfm with(nolock) on cg.ClientGroupID = cgsfm.ClientGroupID
	join ServiceFeature sf with(nolock) on sf.ServiceFeatureID = cgsfm.ServiceFeatureID
	join Service s with(nolock) on sf.ServiceID = s.ServiceID
	where c.ClientID = @ClientID
	and s.ServiceName = @ServiceName
	and sf.SFName = @FeatureName
