CREATE PROCEDURE [dbo].[e_ClientGroup_Select_MasterClientID]
@MasterClientID int
AS
	select *
	from ClientGroup with(nolock)
	where  MasterClientID = @MasterClientID