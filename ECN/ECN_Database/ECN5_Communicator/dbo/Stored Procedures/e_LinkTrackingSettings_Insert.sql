-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingSettings_Insert]
	@LTID int,
	@CustomerID int,
	@BaseChannelID int,
	@XMLConfig varchar(500),
	@CreatedUserID int,
	@CreatedDate datetime
AS
BEGIN
	INSERT INTO LinkTrackingSettings(LTID, CustomerID, BaseChannelID, XMLConfig, CreatedUserID, CreatedDate, IsDeleted)
	Values(@LTID, @CustomerID, @BaseChannelID, @XMLConfig, @CreatedUserID, @CreatedDate, 0)
	SELECT @@IDENTITY
END