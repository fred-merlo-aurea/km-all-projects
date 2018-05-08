CREATE PROCEDURE [dbo].[e_SecurityGroup_CreateFromTemplateForClient]
@SecurityGroupTemplateName varchar(50),
@ClientID int,
@AdministrativeLevel varchar(50),
@DateCreated datetime,
@CreatedByUserID int
AS
BEGIN
   DECLARE @SecurityGroupID int = 0;
   DECLARE @TEMPLATE_ID int;
   SELECT @TEMPLATE_ID = SecurityGroupTemplateID FROM SecurityGroupTemplate where SecurityGroupName = @SecurityGroupTemplateName;
	INSERT 
	  INTO SecurityGroup 
	      ( SecurityGroupName,ClientGroupID,ClientID,IsActive,AdministrativeLevel,DateCreated,CreatedByUserID )
	VALUES(@SecurityGroupTemplateName,NULL,@ClientID,1, @AdministrativeLevel,@DateCreated,@CreatedByUserID);
	SELECT *
	  FROM SecurityGroupTemplate t
	 WHERE t.SecurityGroupName = @SecurityGroupTemplateName;
	
	SELECT @SecurityGroupID = @@IDENTITY;
	INSERT 
	  INTO SecurityGroupPermission
	      (SecurityGroupID, ServiceFeatureAccessMapID, IsActive, DateCreated, CreatedByUserID )
	SELECT @SecurityGroupID, t.ServiceFeatureAccessMapID, 1, @DateCreated, @CreatedByUserID
	  FROM SecurityGroupTemplatePermission t
	 WHERE t.SecurityGroupTemplateID = @TEMPLATE_ID
END;