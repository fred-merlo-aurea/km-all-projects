CREATE PROCEDURE [dbo].[sp_GetSubscriberCountByRadius]
	(
	@MinLat100 decimal(18,15),
	@MaxLat100 decimal(18,15),
	@MinLon100 decimal(18,15),
	@MaxLon100 decimal(18,15),
	@MinLat200 decimal(18,15),
	@MaxLat200 decimal(18,15),
	@MinLon200 decimal(18,15),
	@MaxLon200 decimal(18,15),
	@MinLat300 decimal(18,15),
	@MaxLat300 decimal(18,15),
	@MinLon300 decimal(18,15),
	@MaxLon300 decimal(18,15)	
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @subscriber_count_100 int;	
	declare @subscriber_count_200 int;	
	declare @subscriber_count_300 int;
	
	--COUNT OF SUBSCRIBERS WITHIN 100 MILE RADIUS
	if(@MinLat100<@MaxLat100 and @MinLon100<@MaxLon100)
		begin
			select @subscriber_count_100=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude>@MinLat100 and s.Latitude<@MaxLat100 and s.Longitude>@MinLon100 and s.Longitude<@MaxLon100 
			and s.IsLatLonValid=1
		end
	else if(@MinLat100>@MaxLat100 and @MinLon100<@MaxLon100)
		begin
			select @subscriber_count_100=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat100 and s.Latitude>@MaxLat100 and s.Longitude>@MinLon100 and s.Longitude<@MaxLon100 
			and s.IsLatLonValid=1
		 
		end
	else if(@MinLat100<@MaxLat100 and @MinLon100>@MaxLon100)
		begin
			select @subscriber_count_100=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude>@MinLat100 and s.Latitude<@MaxLat100 and s.Longitude<@MinLon100 and s.Longitude>@MaxLon100 
			and s.IsLatLonValid=1
		 
		end
	else if(@MinLat100>@MaxLat100 and @MinLon100>@MaxLon100)
		begin
			select @subscriber_count_100=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat100 and s.Latitude>@MaxLat100 and s.Longitude<@MinLon100 and s.Longitude>@MaxLon100 
			and s.IsLatLonValid=1
		 
		end
	
	--COUNT OF SUBSCRIBERS WITHIN 200 MILE RADIUS
	if(@MinLat200<@MaxLat200 and @MinLon200<@MaxLon200)
		begin
			select @subscriber_count_200=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude>@MinLat200 and s.Latitude<@MaxLat200 and s.Longitude>@MinLon200 and s.Longitude<@MaxLon200 
			and s.IsLatLonValid=1
		 
		end
	else if(@MinLat200>@MaxLat200 and @MinLon200<@MaxLon200)
		begin
			select @subscriber_count_200=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat200 and s.Latitude>@MaxLat200 and s.Longitude>@MinLon200 and s.Longitude<@MaxLon200 
			and s.IsLatLonValid=1
		 
		end
	else if(@MinLat200<@MaxLat200 and @MinLon200>@MaxLon200)
		begin
			select @subscriber_count_200=COUNT(s.SubscriptionID) 
			from Subscriptions s
			where s.Latitude>@MinLat200 and s.Latitude<@MaxLat200 and s.Longitude<@MinLon200 and s.Longitude>@MaxLon200 
			and s.IsLatLonValid=1
		end
	else if(@MinLat200>@MaxLat200 and @MinLon200>@MaxLon200)
		begin
			select @subscriber_count_200=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat200 and s.Latitude>@MaxLat200 and s.Longitude<@MinLon200 and s.Longitude>@MaxLon200 
			and s.IsLatLonValid=1 
		end
	
	
	--COUNT OF SUBSCRIBERS WITHIN 300 MILE RADIUS
	if(@MinLat300<@MaxLat300 and @MinLon300<@MaxLon300)
		begin
			select @subscriber_count_300=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude>@MinLat300 and s.Latitude<@MaxLat300 and s.Longitude>@MinLon300 and s.Longitude<@MaxLon300 
			and s.IsLatLonValid=1
		end
	else if(@MinLat300>@MaxLat300 and @MinLon300<@MaxLon300)
		begin
			select @subscriber_count_300=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat300 and s.Latitude>@MaxLat300 and s.Longitude>@MinLon300 and s.Longitude<@MaxLon300 
			and s.IsLatLonValid=1
		end
	else if(@MinLat300<@MaxLat300 and @MinLon300>@MaxLon300)
		begin
			select @subscriber_count_300=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude>@MinLat300 and s.Latitude<@MaxLat300 and s.Longitude<@MinLon300 and s.Longitude>@MaxLon300 
			and s.IsLatLonValid=1 
		end
	else if(@MinLat300>@MaxLat300 and @MinLon300>@MaxLon300)
		begin
			select @subscriber_count_300=COUNT(s.SubscriptionID) 
			from Subscriptions s WITH(NOLOCK)
			where s.Latitude<@MinLat300 and s.Latitude>@MaxLat300 and s.Longitude<@MinLon300 and s.Longitude>@MaxLon300 
			and s.IsLatLonValid=1 
		end
	
	select @subscriber_count_100, @subscriber_count_200-@subscriber_count_100, @subscriber_count_300-@subscriber_count_200

END