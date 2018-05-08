CREATE PROCEDURE [dbo].[e_Filters_Save]
	@FilterID int,
    @Name varchar(50),
    @FilterXML xml,
    @CreatedDate DateTime,
    @CreatedUserID int,
    @FilterType varchar(50),
    @PubID int, 
    @IsDeleted bit,
    @UpdatedDate DateTime,
    @UpdatedUserID int,
    @BrandID int,
    @AddtoSalesView bit,
    @FilterCategoryID int,
    @QuestionCategoryID int,
    @QuestionName varchar(200),
	@IsLocked bit,
	@Notes varchar(250)
AS
BEGIN

	SET NOCOUNT ON

	if (@brandID = 0)
		begin
			set @brandID = null;
		end
	if (@CreatedDate is null)
		begin
			set @CreatedDate = GETDATE()
		end
	if (@UpdatedDate is null)
		begin
			set @UpdatedDate = GETDATE()
		end
	if (@FilterID > 0)
		Begin
			Update Filters 
				set Name = @Name,
				FilterXML = @FilterXML,			
				FilterType = @FilterType,
				PubID = @PubID, 
				IsDeleted = @IsDeleted,
				UpdatedDate = @UpdatedDate,
				UpdatedUserID = @UpdatedUserID,
				BrandID = @BrandID,
				AddtoSalesView = @AddtoSalesView,
				FilterCategoryID = @FilterCategoryID,
				QuestionCategoryID = @QuestionCategoryID,
				QuestionName = @QuestionName,
				IsLocked = @IsLocked,
				Notes = @Notes
			where FilterID = @FilterID

			select @FilterID;
		End	
	else
		Begin
			INSERT INTO Filters (Name,FilterXML,CreatedDate,CreatedUserID,FilterType,PubID,IsDeleted,BrandID,AddtoSalesView,FilterCategoryID,QuestionCategoryID,QuestionName,IsLocked,Notes) 
			values (@Name,@FilterXML,@CreatedDate,@CreatedUserID,@FilterType,@PubID,@IsDeleted,@BrandID,@AddtoSalesView,@FilterCategoryID,@QuestionCategoryID,@QuestionName,@IsLocked,@Notes)

			select @@IDENTITY 
		End	
	
END