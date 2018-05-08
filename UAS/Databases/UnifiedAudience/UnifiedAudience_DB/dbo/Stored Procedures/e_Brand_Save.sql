CREATE proc [dbo].[e_Brand_Save](
@BrandID int,
@BrandName varchar(50),
@Logo varchar(100),
@IsBrandGroup bit,
@IsDeleted bit,
@CreatedUserID int,
@CreatedDate datetime,
@UpdatedUserID int,
@UpdatedDate datetime
)
as
BEGIN

	set nocount on

	if (@BrandID > 0)
	begin
		update Brand 
			set BrandName = @BrandName, 
				Logo = @Logo, 
				IsBrandGroup = @IsBrandGroup, 
				IsDeleted = @IsDeleted,
				UpdatedUserID = @UpdatedUserID, 
				UpdatedDate = GETDATE()  
			where BrandID = @BrandID
		select @BrandID;
	end
	else
	begin
		insert into Brand (BrandName, Logo, IsBrandGroup, IsDeleted, CreatedUserID, CreatedDate) values (@BrandName, @Logo, @IsBrandGroup, @IsDeleted, @CreatedUserID, @CreatedDate)
		
		select @@IDENTITY;
    end
End