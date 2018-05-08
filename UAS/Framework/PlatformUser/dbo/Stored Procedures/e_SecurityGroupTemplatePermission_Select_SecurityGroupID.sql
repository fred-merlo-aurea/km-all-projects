CREATE PROCEDURE [dbo].[e_SecurityGroupTemplatePermission_Select_SecurityGroupID]
	@SecurityGroupTemplateID int,
	@IsActive bit = 1
AS
IF @IsActive IS NOT NULL 
	SELECT * FROM SecurityGroupTemplatePermission 
	 WHERE SecurityGroupTemplateID = @SecurityGroupTemplateID
	   AND IsActive = @IsActive
ELSE 
	SELECT * FROM SecurityGroupTemplatePermission
	 WHERE SecurityGroupTemplateID = @SecurityGroupTemplateID
