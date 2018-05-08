CREATE PROCEDURE [dbo].[e_SecurityGroupTemplate_GetNonAdmin]
	
AS
	select * from SecurityGroupTemplate sgt with(nolock) where sgt.SecurityGroupTemplateID not in (1,2)
