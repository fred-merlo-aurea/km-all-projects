CREATE PROCEDURE [dbo].[sp_GetSelectedSubscriberCount]
(
@xmlLocations xml
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   	declare @locations TABLE (latitude decimal(18,15),  longitude decimal(18,15))
                            

	insert into @locations
		SELECT LocationValues.ID.value('./@Latitude','decimal(18,15)'), LocationValues.ID.value('./@Longitude','decimal(18,15)') 
		FROM @xmlLocations.nodes('/Locations') as LocationValues(ID) ;
	
	select s.SubscriptionID as 'SubscriberID' 
	from Subscriptions s 
		inner join @locations l
	on s.Latitude=l.latitude
		and s.Longitude=l.longitude;
			
END