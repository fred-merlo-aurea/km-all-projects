CREATE PROCEDURE dbo.sp_GetSubscriberByGL
	(
	@MinLat decimal(18,15),
	@MaxLat decimal(18,15),
	@MinLon decimal(18,15),
	@MaxLon decimal(18,15),
	@Latitude decimal(18,15),
	@Longitude decimal(18,15),
	@RadiusMax int,
	@BrandID int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	if @BrandID = 0
		begin
			if(@MinLat<@MaxLat and @MinLon<@MaxLon)
				begin
					select s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK)
					where s.Latitude>@MinLat and s.Latitude<@MaxLat and s.Longitude>@MinLon and s.Longitude<@MaxLon 
						and s.IsLatLonValid=1 
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat>@MaxLat and @MinLon<@MaxLon)
				begin
					select s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK)
					where s.Latitude<@MinLat and s.Latitude>@MaxLat and s.Longitude>@MinLon and s.Longitude<@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat<@MaxLat and @MinLon>@MaxLon)
				begin
					select s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK)
					where s.Latitude>@MinLat and s.Latitude<@MaxLat and s.Longitude<@MinLon and s.Longitude>@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat>@MaxLat and @MinLon>@MaxLon)
				begin
					select s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s
					where s.Latitude<@MinLat and s.Latitude>@MaxLat and s.Longitude<@MinLon and s.Longitude>@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
		end
	else
		begin
			if(@MinLat<@MaxLat and @MinLon<@MaxLon)
				begin
					select distinct s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK)
						join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID
						join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID
						join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID
					where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude>@MinLat and s.Latitude<@MaxLat and s.Longitude>@MinLon and s.Longitude<@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat>@MaxLat and @MinLon<@MaxLon)
				begin
					select distinct s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK)
						join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID
						join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID
						join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID
					where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude<@MinLat and s.Latitude>@MaxLat and s.Longitude>@MinLon and s.Longitude<@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat<@MaxLat and @MinLon>@MaxLon)
				begin
					select distinct s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK) 
						join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID
						join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID
						join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID
					where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude>@MinLat and s.Latitude<@MaxLat and s.Longitude<@MinLon and s.Longitude>@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
			else if(@MinLat>@MaxLat and @MinLon>@MaxLon)
				begin
					select distinct s.SubscriptionID as 'SubscriberID', s.ADDRESS, s.Latitude, s.Longitude, s.ZIP 
					from Subscriptions s WITH(NOLOCK) 
						join PubSubscriptions ps WITH(NOLOCK) on ps.SubscriptionID = s.SubscriptionID
						join BrandDetails bd WITH(NOLOCK) on bd.PubID = ps.PubID
						join Brand b WITH(NOLOCK) on b.BrandID = bd.BrandID
					where bd.BrandID = @BrandID and b.IsDeleted = 0 and s.Latitude<@MinLat and s.Latitude>@MaxLat and s.Longitude<@MinLon and s.Longitude>@MaxLon 
						and s.IsLatLonValid=1
						and (master.dbo.fn_CalcDistanceBetweenLocations(@Latitude, @Longitude, s.Latitude, s.Longitude, 0) <= @RadiusMax)
					order by s.ADDRESS 
				end
		end	

END