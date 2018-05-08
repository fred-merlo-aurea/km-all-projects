CREATE PROCEDURE [dbo].[e_Pricing_Save]
	@BrandID int,
	@RangeStart int,
	@RangeEnd int,
	@Price decimal(10,2),
	@BGColor varchar(10),
	@FGColor varchar(10),
	@UpdatedBy uniqueidentifier
AS
BEGIN

	set nocount on

		INSERT INTO [Pricing]
			([BrandID]
			,[RangeStart]
			,[RangeEnd]
			,[Price]
			,[BGColor]
			,[FGColor]
			,[UpdatedBy]
			,[UpdatedDate])	      
		VALUES
		(@BrandID, @RangeStart, @RangeEnd, @Price, @BGColor, @FGColor, @UpdatedBy,  GETDATE())
		SELECT @@IDENTITY

END