CREATE PROCEDURE [dbo].[e_SecurityGroupPermission_Select_SecurityGroupID]  
(
@SecurityGroupID int
)
as
Begin
	select * from SecurityGroupPermission with (NOLOCK)
	where
			SecurityGroupID = @SecurityGroupID

End
