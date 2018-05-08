CREATE PROCEDURE [dbo].[e_GatewayValue_Save]
	@GatewayValueID int = null,
	@UpdatedUserID int = null,
	@CreatedUserID int = null,
	@GatewayID int,
	@IsDeleted bit,
	@IsCaptureValue bit,
	@IsLoginValidator bit,
	@Field varchar(100),
	@FieldType varchar(100) = null,
	@Comparator varchar(50) = null,
	@DatePart varchar(50) = null,
	@NOT bit = null,
	@Value varchar(255),
	@IsStatic bit,
	@Label varchar(MAX)
AS
	if(@GatewayValueID is not null)
	BEGIN
		Update GatewayValue
		set UpdatedUserID = @UpdatedUserID, UpdatedDate = GETDATE(), IsDeleted = @IsDeleted, IsCaptureValue = @IsCaptureValue, IsLoginValidator = @IsLoginValidator, Field = @Field, FieldType = @FieldType, Comparator = @Comparator, DatePart = @DatePart, [NOT] = @NOT, Value = @Value,IsStatic = @IsStatic, Label = @Label
		WHERE GatewayValueID = @GatewayValueID
		SELECT @GatewayValueID
	END
	ELSE
	BEGIN
		INSERT INTO GatewayValue(GatewayID, Field, IsLoginValidator, IsCaptureValue,IsStatic,Label, Value, IsDeleted, Comparator, [NOT], FieldType, DatePart, CreatedUserID, CreatedDate)
		VALUES(@GatewayID, @Field, @IsLoginValidator, @IsCaptureValue,@IsStatic,@Label, @Value, @IsDeleted, @Comparator, @NOT, @FieldType, @DatePart, @CreatedUserID, GETDATE())
		SELECT @@IDENTITY;
	END