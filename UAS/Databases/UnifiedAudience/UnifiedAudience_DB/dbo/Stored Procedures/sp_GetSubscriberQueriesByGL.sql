CREATE PROCEDURE [dbo].[sp_GetSubscriberQueriesByGL]
@MinLat decimal(18,15),
@MaxLat decimal(18,15),
@MinLon decimal(18,15),
@MaxLon decimal(18,15),
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@RadiusMax int,
@BrandID int
AS
BEGIN
	SET NOCOUNT ON;
	
	declare	@query varchar(MAX)
	
	if @BrandID = 0
		begin
			if(@MinLat<@MaxLat and @MinLon<@MaxLon)
				begin
					SET @query = 
					' select distinct 1, s.SubscriptionID as ''SubscriberID''' +
					' from Subscriptions s WITH(NOLOCK)' +
					' where s.Latitude>' + cast(@MinLat as varchar(20)) + ' and s.Latitude<' + cast(@MaxLat as varchar(20)) + ' and s.Longitude>' + cast(@MinLon as varchar(20)) + ' and s.Longitude<' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat>@MaxLat and @MinLon<@MaxLon)
				begin
					SET @query = 
					' select distinct 1,  s.SubscriptionID as ''SubscriberID''' +
					' from Subscriptions s WITH(NOLOCK) ' +
					' where s.Latitude<' + cast(@MinLat as varchar(20)) + ' and s.Latitude>' + cast(@MaxLat as varchar(20)) + ' and s.Longitude>' + cast(@MinLon as varchar(20)) + ' and s.Longitude< ' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat<@MaxLat and @MinLon>@MaxLon)
				begin
					SET @query = 
					' select distinct 1,  s.SubscriptionID as ''SubscriberID''' +
					' from Subscriptions s WITH(NOLOCK)' +
					' where s.Latitude>' + cast(@MinLat as varchar(20)) + ' and s.Latitude<' + cast(@MaxLat as varchar(20)) + ' and s.Longitude<' + cast(@MinLon as varchar(20)) + ' and s.Longitude>' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1	and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat>@MaxLat and @MinLon>@MaxLon)
				begin
					SET @query = 
					' select  distinct 1, s.SubscriptionID as ''SubscriberID''' +
					' from Subscriptions s ' +
					' where s.Latitude<' + cast(@MinLat as varchar(20)) + ' and s.Latitude>' + cast(@MaxLat as varchar(20)) + ' and s.Longitude<' + cast(@MinLon as varchar(20)) + ' and s.Longitude>' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
		end
	else
		begin
			if(@MinLat<@MaxLat and @MinLon<@MaxLon)
				begin
					SET @query = 
					' select distinct 1,  distinct s.SubscriptionID as ''SubscriberID''' +
					' from Subscriptions s WITH(NOLOCK) ' +
					'	join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID ' +
					'	join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID ' +
					'	join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID ' +
					' where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude>' + cast(@MinLat as varchar(20)) + ' and s.Latitude<' + cast(@MaxLat as varchar(20)) + ' and s.Longitude>' + cast(@MinLon as varchar(20)) + ' and s.Longitude<' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat>@MaxLat and @MinLon<@MaxLon)
				begin
					SET @query = 
					' select distinct 1,  distinct s.SubscriptionID as ''SubscriberID'' ' +
					' from Subscriptions s WITH(NOLOCK) ' +
					'	join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID ' +
					'	join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID ' +
					'	join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID ' +
					' where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude<' + cast(@MinLat as varchar(20)) + ' s.Latitude>' + cast(@MaxLat as varchar(20)) + ' s.Longitude>' + cast(@MinLon as varchar(20)) + ' s.Longitude<' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat<@MaxLat and @MinLon>@MaxLon)
				begin
					SET @query = 
					' select  distinct 1,  s.SubscriptionID as ''SubscriberID'' ' +
					' from Subscriptions s WITH(NOLOCK)  ' +
					'	join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID ' +
					'	join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID ' +
					'	join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID ' +
					' where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude>' + cast(@MinLat as varchar(20)) + ' s.Latitude<' + cast(@MaxLat as varchar(20)) + ' s.Longitude<' + cast(@MinLon as varchar(20)) + ' s.Longitude>' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
			else if(@MinLat>@MaxLat and @MinLon>@MaxLon)
				begin
					SET @query = 
					' select  distinct 1,  s.SubscriptionID as ''SubscriberID'' ' +
					' from Subscriptions s WITH(NOLOCK)  ' +
					'	join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID ' +
					'	join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID ' +
					'	join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID ' +
					' where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude<' + cast(@MinLat as varchar(20)) + ' s.Latitude>' + cast(@MaxLat as varchar(20)) + ' s.Longitude<' + cast(@MinLon as varchar(20)) + ' s.Longitude>' + cast(@MaxLon as varchar(20)) +
					' and s.IsLatLonValid=1 and (master.dbo.fn_CalcDistanceBetweenLocations(' + cast(@Latitude as varchar(20)) + ',' + cast(@Longitude as varchar(20)) + ', s.Latitude, s.Longitude, 0) <= ' + cast(@RadiusMax as varchar(20)) + ')' 
				end
		end	
	print @query;
	Select @query;
END
