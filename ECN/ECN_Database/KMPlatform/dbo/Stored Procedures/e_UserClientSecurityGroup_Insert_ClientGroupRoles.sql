CREATE PROCEDURE [dbo].[e_UserClientSecurityGroup_Insert_ClientGroupRoles]
	@ClientID int,
	@ClientGroupID int
AS
	

	
	--DO Userclientsecuritygroupmap records
	declare @UserID int,@TempClientID int,@SecurityGroupID int, @CreatedByUserID int, @DateCreated datetime, @IsActive bit, @InactiveReason varchar(50)
	
	
	declare myCursor CURSOR
	FOR 
	SELECT distinct ucsgm.UserID, @ClientID, sg.SecurityGroupID, ISNULL(ucsgm.CreatedByUserID,501), ucsgm.DateCreated, ucsgm.IsActive ,ucsgm.InactiveReason
		from SecurityGroup sg with(nolock)
		join UserClientSecurityGroupMap ucsgm with(nolocK) on sg.SecurityGroupID = ucsgm.SecurityGroupID
	where sg.ClientGroupID = @ClientGroupID and sg.IsActive = 1 and ucsgm.ClientID != @ClientID
	
	OPEN myCursor
	FETCH NEXT FROM myCursor
		INTO @UserID, @TempClientID, @SecurityGroupID, @CreatedByUserID, @DateCreated,@IsActive,@InactiveReason
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if not exists (Select top 1 * from UserClientSecurityGroupMap u with(nolock) where u.UserID = @UserID and u.ClientID = @ClientID and u.SecurityGroupID = @SecurityGroupID)
		BEGIN
			INSERT INTO UserClientSecurityGroupMap(UserID, ClientID, SecurityGroupID, CreatedByUserID, DateCreated, IsActive, InactiveReason)
			VALUES(@UserID, @ClientID, @SecurityGroupID, @CreatedByUserID, @DateCreated, @IsActive, @InactiveReason)
		END
		FETCH NEXT FROM myCursor
		INTO @UserID, @TempClientID, @SecurityGroupID, @CreatedByUserID, @DateCreated,@IsActive,@InactiveReason
	END
	CLOSE myCursor
	DEALLOCATE myCursor
	print 'done with user client security group map'
	--Do SecurityGroupOptIn records
	declare @SGOISGID int, @SGOIUserID int, @SGOISetID uniqueidentifier, @SGOIClientID int, @SGOIClientGroupID int, @SGOISendTime datetime, @SGOIHasAccepted bit, @SGOICreatedByUserID int, @SGOICreatedDate datetime
	
	declare cursor2 CURSOR
	FOR
	Select distinct sgoi.SecurityGroupID, sgoi.UserID, sgoi.SetID,sgoi.ClientID,sgoi.ClientGroupID,sgoi.SendTime, sgoi.HasAccepted, ISNULL(sgoi.CreatedByUserID,501), sgoi.CreatedDate
	FROM SecurityGroupOptIn sgoi with(nolocK)
	join UserClientSecurityGroupMap ucsgm with(nolock) on sgoi.UserID = ucsgm.UserID and sgoi.SecurityGroupID = ucsgm.SecurityGroupID
	where sgoi.HasAccepted = 0 and ISNULL(sgoi.IsDeleted, 0) = 0 and sgoi.ClientGroupID = @ClientGroupID
	
	OPEN cursor2
	FETCH NEXT FROM cursor2 INTO
		@SGOISGID, @SGOIUserID, @SGOISetID, @SGOIClientID, @SGOIClientGroupID, @SGOISendTime, @SGOIHasAccepted,@SGOICreatedByUserID, @SGOICreatedDate
		WHILE @@FETCH_STATUS = 0
		BEGIN
			if not exists(SELECT top 1 * from SecurityGroupOptIn sgoi with(nolock) where sgoi.SecurityGroupID = @SGOISGID and sgoi.UserID = @SGOIUserID and sgoi.ClientGroupID = @ClientGroupID and sgoi.ClientID = @ClientID)
			BEGIN
				INSERT INTO SecurityGroupOptIn(SecurityGroupID, UserID, SetID, ClientID, ClientGroupID, SendTime, HasAccepted, CreatedByUserID, CreatedDate)
				VALUES(@SGOISGID, @SGOIUserID, @SGOISetID, @ClientID, @ClientGroupID, @SGOISendTime, @SGOIHasAccepted,@SGOICreatedByUserID, @SGOICreatedDate)
			END
			FETCH NEXT FROM cursor2 INTO
		@SGOISGID, @SGOIUserID, @SGOISetID, @SGOIClientID, @SGOIClientGroupID, @SGOISendTime, @SGOIHasAccepted,@SGOICreatedByUserID, @SGOICreatedDate
		END
		CLOSE cursor2
		DEALLOCATE cursor2
	
	