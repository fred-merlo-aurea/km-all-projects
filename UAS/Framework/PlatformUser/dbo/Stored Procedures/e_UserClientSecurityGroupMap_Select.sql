CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Select]
AS
	select * 
	from UserClientSecurityGroupMap with(nolock)
