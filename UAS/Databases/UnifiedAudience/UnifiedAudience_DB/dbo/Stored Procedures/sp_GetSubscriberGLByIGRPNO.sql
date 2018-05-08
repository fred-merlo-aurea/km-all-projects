CREATE PROCEDURE [dbo].[sp_GetSubscriberGLByIGRPNO]
(@xmlGroupnos TEXT)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @docHandle int

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlGroupnos  

	declare  @igroupno table(groupno uniqueidentifier)
	
	insert into @igroupno
	select groupno FROM OPENXML(@docHandle, N'/XML/n')   
	WITH   
	(  
		groupno uniqueidentifier '.'
	)  

	EXEC sp_xml_removedocument @docHandle;
	
	select distinct s.Latitude, s.Longitude, COUNT(s.SubscriptionID) as 'SubscriberID'  
	from Subscriptions s with(nolock) 
		inner join @igroupno g on s.IGRP_NO=g.groupno	
	where s.Latitude is not null and s.Longitude is not null and s.IsLatLonValid=1
	group by s.Latitude, s.Longitude;
END