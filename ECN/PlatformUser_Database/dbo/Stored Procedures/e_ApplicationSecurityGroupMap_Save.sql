CREATE PROCEDURE [dbo].[e_ApplicationSecurityGroupMap_Save]
@ApplicationSecurityGroupMapID int,
@SecurityGroupID int,
@ApplicationID int,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
/*
DECLARE @ApplicationSecurityGroupMapID int;
DECLARE @SecurityGroupID int = 8;
DECLARE @ApplicationID int = 8;
DECLARE @HasAccess bit = 0;
DECLARE @DateCreated datetime;
DECLARE @DateUpdated datetime;
DECLARE @CreatedByUserID int;
DECLARE @UpdatedByUserID int;
*/
IF @ApplicationSecurityGroupMapID IS NULL OR @ApplicationSecurityGroupMapID < 1
	IF 0 < (SELECT COUNT(*) FROM ApplicationSecurityGroupMap m WHERE m.SecurityGroupID = @SecurityGroupID  AND m.ApplicationID = @ApplicationID)
	BEGIN
		DECLARE @UpdateRowID int = (SELECT ApplicationSecurityGroupMapID FROM ApplicationSecurityGroupMap m WHERE m.SecurityGroupID = @SecurityGroupID  AND m.ApplicationID = @ApplicationID);
		SET @DateUpdated = GETDATE();
		UPDATE ApplicationSecurityGroupMap
		SET 
			HasAccess = @HasAccess,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ApplicationSecurityGroupMapID = @UpdateRowID;
		SELECT @UpdateRowID;
	END
ELSE 
	BEGIN
		SET @DateCreated = GETDATE();
		INSERT INTO ApplicationSecurityGroupMap (SecurityGroupID,ApplicationID,HasAccess,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupID,@ApplicationID,@HasAccess,@DateCreated,@CreatedByUserID);
		SELECT @@IDENTITY;
	END

