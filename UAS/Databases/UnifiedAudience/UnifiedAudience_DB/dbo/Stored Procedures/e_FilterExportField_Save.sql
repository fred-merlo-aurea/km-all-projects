CREATE proc [dbo].[e_FilterExportField_Save]
@FilterScheduleID int, 
@ExportColumn varchar(100),
@MappingField varchar(100),
@IsCustomValue bit,
@CustomValue varchar(100),
@IsDescription bit,
@FieldCase varchar(100)
AS
BEGIN

	SET NOCOUNT ON

	insert into FilterExportField (FilterScheduleID, ExportColumn, MappingField, IsCustomValue, CustomValue, IsDescription, FieldCase) 
	values (@FilterScheduleID, @ExportColumn, @MappingField, @IsCustomValue, @CustomValue, @IsDescription, @FieldCase)
	Select @@IDENTITY;

END