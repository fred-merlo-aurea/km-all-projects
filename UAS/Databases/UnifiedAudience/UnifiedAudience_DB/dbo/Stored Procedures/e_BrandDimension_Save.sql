CREATE PROCEDURE [dbo].[e_BrandDimension_Save]
@BrandDimensionID int,
@BrandID int,
@MasterGroupID int,
@EnableSearching bit, 
@IsRequired bit, 
@IsMultipleValue bit,
@SortOrder int
AS
BEGIN

	SET NOCOUNT ON;

	if (@BrandDimensionID > 0)
	begin	
		UPDATE [BrandDimension] SET 
		[EnableSearching] = @EnableSearching, 
		[IsRequired] = @IsRequired, 
		[IsMultipleValue] = @IsMultipleValue, 
		[SortOrder] = @SortOrder 
		WHERE [BrandDimensionID] = @BrandDimensionID
		select @BrandDimensionID;
	end
	else
	begin
		INSERT INTO [BrandDimension] 
			([BrandID], [MasterGroupID], [EnableSearching], [IsRequired], [IsMultipleValue], [SortOrder]) 
		VALUES 
			(@BrandID, @MasterGroupID, @EnableSearching, @IsRequired, @IsMultipleValue, @SortOrder)
		select @@IDENTITY;
    end		

END