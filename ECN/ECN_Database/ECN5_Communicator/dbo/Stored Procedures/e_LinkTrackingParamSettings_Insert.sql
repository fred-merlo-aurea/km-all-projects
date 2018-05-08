-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamSettings_Insert]
	@LTPID int,
	@CustomerID int,
	@BaseChannelID int,
	@DisplayName varchar(50),
	@AllowCustom bit,
	@IsRequired bit,
	@CreatedDate datetime,
	@CreatedUserID int
AS
BEGIN
    INSERT INTO LinkTrackingParamSettings(LTPID, CustomerID, BaseChannelID, DisplayName, AllowCustom, IsRequired, IsDeleted, CreatedDate, CreatedUserID)
    Values(@LTPID, @CustomerID, @BaseChannelID, @DisplayName, @AllowCustom, @IsRequired, 0, @CreatedDate, @CreatedUserID)
    SELECT @@IDENTITY
END