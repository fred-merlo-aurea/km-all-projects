-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingSettings_Update]
	@LTSID int,
	@LTID int,
	@CustomerID int,
	@BaseChannelID int,
	@XMLConfig varchar(500),
	@UpdatedUserID int,
	@UpdatedDate datetime
	
AS
BEGIN
	UPDATE LinkTrackingSettings
	SET LTID = @LTID, CustomerID = @CustomerID, BaseChannelID = @BaseChannelID, XMLConfig = @XMLConfig, UpdatedUserID = @UpdatedUserID, UpdatedDate = @UpdatedDate
	WHERE LTSID = @LTSID
END