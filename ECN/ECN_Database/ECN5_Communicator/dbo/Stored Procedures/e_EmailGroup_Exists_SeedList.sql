CREATE PROCEDURE [dbo].[e_EmailGroup_Exists_SeedList]
	@EmailID int,
	@CustomerID int
AS
	declare @SeedListID int
	select @SeedListID = GroupID 
	from Groups g with(nolock)
	where g.CustomerID = @CustomerID and g.IsSeedList = 1

	if @SeedListID is not null and @SeedListID > 0
	BEGIN
		if exists (Select top 1 * from EmailGroups eg with(nolock) where eg.EmailID = @EmailID and eg.GroupID = @SeedListID)
		BEGIN
			SELECT 1
		END
		else
		BEGIN
			 SELECT 0
		END

	END
	ELSE
	BEGIN
		SELECT 0
	END
