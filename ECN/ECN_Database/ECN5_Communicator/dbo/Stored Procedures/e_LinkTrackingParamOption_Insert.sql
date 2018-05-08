-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Insert]
	@LTPID int,
	@DisplayName varchar(50),
	@ColumnName varchar(50) = NULL,
	@Value varchar(50),
	@IsActive bit,
	@CustomerID int,
	@BaseChannelID int,
	@IsDynamic bit,
	@IsDefault bit,
	@CreatedUserID int,
	@CreatedDate datetime
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--Have to change IsDefault if there already is a default option
	if(@IsDefault = 1 and exists(Select top 1 * from LinkTrackingParamOption where CustomerID = @CustomerID and BaseChannelID = @BaseChannelID and LTPID = @LTPID and IsDefault = 1 and IsDeleted = 0))
	begin
		update LinkTrackingParamOption
		set IsDefault = 0
		where CustomerID = @CustomerID and BaseChannelID = @BaseChannelID and IsDefault = 1 and LTPID = @LTPID and IsDeleted = 0
	end

   INSERT INTO LinkTrackingParamOption(LTPID, DisplayName, ColumnName, Value, IsActive, CustomerID, BaseChannelID, IsDynamic, IsDefault, CreatedUserID, CreatedDate, IsDeleted)
   Values(@LTPID, @DisplayName, @ColumnName, @Value, @IsActive,@CustomerID, @BaseChannelID, @IsDynamic, @IsDefault, @CreatedUserID, @CreatedDate, 0)
   SELECT @@IDENTITY
END