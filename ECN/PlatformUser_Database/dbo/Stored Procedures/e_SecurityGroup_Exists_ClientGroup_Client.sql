
CREATE PROCEDURE [dbo].[e_SecurityGroup_Exists_ClientGroup_Client]
	@SecurityGroupName varchar(100),
	@ClientGroupID int,
	@ClientID int,
	@SecurityGroupID int
AS
declare @Exists bit = 0
	if(@ClientGroupID > 0)
	BEGIN
		if exists (Select top 1 * from SecurityGroup sg with(nolock) where sg.SecurityGroupName = @SecurityGroupName and sg.ClientGroupID = @ClientGroupID and sg.SecurityGroupID != @SecurityGroupID)
		BEGIN
			SET @Exists = 1
		END
	END

	IF @ClientID > 0
	BEGIN
		if exists (Select top 1 * from SecurityGroup sg with(nolock) where sg.SecurityGroupName = @SecurityGroupName and sg.ClientID = @ClientID and sg.SecurityGroupID != @SecurityGroupID)
		BEGIN
			SET @Exists = 1
		END	
		
	END

	if @Exists = 1
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END