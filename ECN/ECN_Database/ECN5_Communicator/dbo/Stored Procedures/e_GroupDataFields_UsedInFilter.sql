CREATE PROCEDURE [dbo].[e_GroupDataFields_UsedInFilter](
	@GDFID int,
	@GroupID int
	)
AS
BEGIN
	declare @shortName varchar(500)
	select @shortName = '[[]' + ShortName + ']' from GroupDataFields gdf with(nolock) where gdf.GroupDataFieldsID = @GDFID
	if exists (select top 1 * from Filter f with(nolock) where PATINDEX('%' + @shortName +'%' , f.WhereClause) > 0 and ISNULL(f.IsDeleted, 0) = 0 and f.GroupID = @GroupID) 
	BEGIN
		select 1
	END
	else
	begin
		select 0
	end
END