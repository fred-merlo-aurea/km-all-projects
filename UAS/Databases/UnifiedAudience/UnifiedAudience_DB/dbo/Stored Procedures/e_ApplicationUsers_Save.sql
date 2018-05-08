CREATE proc [dbo].[e_ApplicationUsers_Save]
@UserID uniqueidentifier,
@UserName varchar(100),
@Password varchar(50),
@Email varchar(50), 
@IsApproved bit, 
@FullName varchar(200),
@SalesRepID uniqueidentifier,
@CompanyName varchar(100),
@SalesForceID varchar(50),
@FreeContactDownload int,
@FreeOpportunityDownload int,
@TrialExpireDate datetime,
@PackageLevel varchar(50),
@BrandID int,
@RoleID int
AS
BEGIN
	SET NOCOUNT ON;

	if (@UserID = cast(cast(0 as binary) as uniqueidentifier))
		begin
		
			Declare @id  uniqueidentifier
			set @id = NEWID();
		
			INSERT INTO [ApplicationUsers] 
				([UserID], [UserName], [Password], [Email], [IsApproved], [FullName], [SalesRepID], [CompanyName],
				[SalesForceID], [FreeContactDownload], [FreeOpportunityDownload], [TrialExpireDate],
				[PackageLevel], [BrandID], [RoleID], CreatedDate) 
			VALUES 
				(@id, @UserName, @Password, @Email, @IsApproved, @FullName, @SalesRepID, @CompanyName,
				@SalesForceID, @FreeContactDownload, @FreeOpportunityDownload, @TrialExpireDate,
				@PackageLevel, @BrandID, @RoleID, GETDATE())
			select @id;
		end
	else
		begin
			UPDATE [ApplicationUsers] SET 
			[UserName] = @UserName, 
			[Email] = @Email, 
			[IsApproved] = @IsApproved, 
			[FullName] = @FullName, 
			[SalesRepID] = @SalesRepID,
			[CompanyName] = @CompanyName, 
			[SalesForceID] = @SalesForceID, 
			[FreeContactDownload] = @FreeContactDownload,
			[FreeOpportunityDownload] = @FreeOpportunityDownload, 
			[TrialExpireDate] = @TrialExpireDate, 
			[PackageLevel] = @PackageLevel, 
			[BrandID] = @BrandID, 
			[RoleID] = @RoleID,
			[UpdatedDate] = GETDATE()
			WHERE [UserID] = @UserID
			select @UserID;
		end

END