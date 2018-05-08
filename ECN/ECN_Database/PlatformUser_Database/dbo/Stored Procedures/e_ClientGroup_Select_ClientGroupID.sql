CREATE PROCEDURE [dbo].[e_ClientGroup_Select_ClientGroupID]
@ClientGroupID int
AS
	select *
	from ClientGroup with(nolock)
	where ClientGroupID = @ClientGroupID
GO
