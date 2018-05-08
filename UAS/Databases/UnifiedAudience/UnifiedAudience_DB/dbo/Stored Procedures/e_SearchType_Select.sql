CREATE PROCEDURE [dbo].[e_SearchType_Select]
AS
BEGIN

	SET NOCOUNT ON

	select * 
	from SearchType with(nolock)

END