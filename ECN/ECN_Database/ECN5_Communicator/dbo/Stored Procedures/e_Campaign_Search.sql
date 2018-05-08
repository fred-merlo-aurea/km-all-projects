CREATE PROCEDURE [dbo].[e_Campaign_Search]
	@CustomerID int,
	@Criteria varchar(500)
AS
	Select * from Campaign c with(nolock)
	where c.Customerid = @CustomerID and c.CampaignName like '%' + @Criteria + '%' and ISNULL(c.IsArchived,0) = 0 and ISNULL(c.IsDeleted,0) = 0
