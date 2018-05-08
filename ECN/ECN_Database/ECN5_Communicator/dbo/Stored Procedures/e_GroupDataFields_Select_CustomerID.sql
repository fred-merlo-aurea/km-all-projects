CREATE PROCEDURE [dbo].[e_GroupDataFields_Select_CustomerID] 
	@CustomerID int = null
AS     
BEGIN     		
	select gdf.*,g.CustomerID from GroupDatafields gdf with (NOLOCK)
	join Groups g  with (NOLOCK)
	on gdf.GroupID=g.GroupID
	where g.CustomerID=@CustomerID
	and gdf.IsDeleted=0 and gdf.DatafieldSetID is null
	order by gdf.ShortName
END

