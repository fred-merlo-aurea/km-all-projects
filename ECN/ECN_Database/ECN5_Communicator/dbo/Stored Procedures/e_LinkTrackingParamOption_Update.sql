-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Update]
	@LTPOID int, 
	@LTPID int,
	@DisplayName varchar(50),
	@ColumnName varchar(50) = NULL,
	@Value varchar(50),
	@IsActive bit,
	@IsDeleted bit,
	@CustomerID int,
	@BaseChannelID int,
	@IsDynamic bit,
	@IsDefault bit,
	@UpdatedUserID int,
	@UpdatedDate datetime
AS
BEGIN
	if(@IsDeleted <> 1)
		begin
			if(@IsDefault = 1 and @CustomerID is null and exists(Select top 1 * from LinkTrackingParamOption where BaseChannelID = @BaseChannelID and LTPID = @LTPID and IsDefault = 1 and IsDeleted = 0))
				begin
					update LinkTrackingParamOption
					set IsDefault = 0
					where BaseChannelID = @BaseChannelID and IsDefault = 1 and LTPID = @LTPID and IsDeleted = 0
				end
			else if(@IsDefault = 1 and @BaseChannelID is null and exists(Select top 1 * from LinkTrackingParamOption where CustomerID = @CustomerID and LTPID = @LTPID and IsDefault = 1 and IsDeleted = 0))
				begin
				update LinkTrackingParamOption
					set IsDefault = 0
					where CustomerID = @CustomerID and IsDefault = 1 and LTPID = @LTPID and IsDeleted = 0
				end
			
			UPDATE LinkTrackingParamOption
			SET LTPID = @LTPID,DisplayName = @DisplayName, ColumnName = @ColumnName, Value = @Value, IsActive = @IsActive,
			 CustomerID = @CustomerID, BaseChannelID = @BaseChannelID, IsDynamic = @IsDynamic, IsDefault = @IsDefault, UpdatedDate = @UpdatedDate, UpdatedUserID = @UpdatedUserID
			WHERE LTPOID = @LTPOID 
		end
	else
	begin
		update LinkTrackingParamOption
		set IsDeleted = @IsDeleted
		where LTPOID = @LTPOID
	end
END