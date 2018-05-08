CREATE PROCEDURE dbo.sp_SaveHeatMapLocations
	-- Add the parameters for the stored procedure here
	(
	@address varchar(50),
	@city varchar(20),
	@state varchar(20),
	@zip varchar(20),
	@radius varchar(10),
	@location_name varchar(50),
	@user varchar(20),
	@brandID int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	
    insert into dbo.HeatMapLocations
    ([Address], [City], [State],[Country], [Zip], [Radius],[LocationName],[CreatedBy],[BrandID])
    values(@address, @city, @state,'USA', @zip, @radius, @location_name, @user, @brandID)
    
END