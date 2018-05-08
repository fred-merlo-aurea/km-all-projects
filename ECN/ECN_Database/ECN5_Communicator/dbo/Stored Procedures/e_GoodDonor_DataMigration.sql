CREATE PROCEDURE [dbo].[e_GoodDonor_DataMigration] (
	@sourceCustomerId int,
	@destinationCustomerId int,
	@sourceGroupId int,
	@destinationGroupId int
) AS
SET NOCOUNT ON


Update Emails 
set CustomerID = @destinationCustomerId 
where CustomerID = @sourceCustomerId and State in('NY','NJ')

Update EmailGroups 
set GroupID = @destinationGroupId 
where GroupID = @sourceGroupId and EmailID in (Select EMAILID from Emails e where e.CustomerID = @destinationCustomerId and e.State in ('ny','nj')) 

Update 
	EmailDataValues
set 
	GroupDatafieldsID = gdf2.GroupDatafieldsID
FROM 
	GroupDatafields gdf1 with(nolock)
	join GroupDatafields gdf2 with(nolock) on gdf1.ShortName = gdf2.ShortName
WHERE 
	gdf1.GroupID = @sourceGroupId
	and gdf1.IsDeleted = 0 
	and gdf2.GroupID = @destinationGroupId 
	and gdf2.IsDeleted = 0 
	and EmailID in (
		SELECT e.EMAILID from Emails e
                  join EmailGroups eg on e.EmailID = eg.EmailID
                  where e.CustomerID = @destinationCustomerId and e.State in ('ny','nj') and eg.GroupID = @destinationGroupId) 
