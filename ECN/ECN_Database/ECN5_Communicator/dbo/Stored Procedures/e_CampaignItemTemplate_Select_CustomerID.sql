CREATE  PROC dbo.e_CampaignItemTemplate_Select_CustomerID
(
    @CustomerID int,
	@ArchiveFilter varchar(20)
)
AS 
BEGIN
	select * 
	from CampaignItemTemplates
	where IsDeleted=0
	and CustomerID=@CustomerID
	and ISNULL(Archived,0) = CASE WHEN @ArchiveFilter = 'all' then ISNULL(Archived,0) WHEN @ArchiveFilter = 'active' then 0 WHEN @ArchiveFilter = 'archived' then 1 end
	
END