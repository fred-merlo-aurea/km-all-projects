CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Select_DomainTrackerID]
@DomainTrackerID int
AS
select CONVERT(varchar(200),dtf.DomainTrackerFieldsID) as DomainTrackerFieldsID, CONVERT(varchar(200),dtf.GroupDatafieldsID) as GroupDataFieldsID,
 dtf.Source, dtf.SourceID, gdf.ShortName as  GroupDataFieldsName , dtf.IsDeleted
 from DomainTrackerFields dtf
inner join GroupDatafields  gdf
on dtf.GroupDatafieldsID= gdf.GroupDatafieldsID
where DomainTrackerID=@DomainTrackerID and dtf.IsDeleted=0