-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamSettings_Update]
	@LTPSID int,
	@LTPID int,
	@CustomerID int,
	@BaseChannelID int,
	@DisplayName varchar(50),
	@AllowCustom bit,
	@IsRequired bit,
	@UpdatedUserID int,
	@UpdatedDate datetime
AS
BEGIN
	UPDATE LinkTrackingParamSettings
	SET LTPID = @LTPID, CustomerID = @CustomerID, BaseChannelID = @BaseChannelID, DisplayName = @DisplayName, AllowCustom = @AllowCustom, IsRequired = @IsRequired,
	UpdatedUserID = @UpdatedUserID, UpdatedDate = @UpdatedDate
	WHERE LTPSID = @LTPSID
END