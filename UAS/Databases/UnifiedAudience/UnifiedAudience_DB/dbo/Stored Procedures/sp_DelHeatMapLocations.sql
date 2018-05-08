CREATE PROCEDURE dbo.sp_DelHeatMapLocations
	-- Add the parameters for the stored procedure here
	(
	@location_id int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   delete 
   from dbo.HeatMapLocations 
   where [LocationID]=@location_id
    
END