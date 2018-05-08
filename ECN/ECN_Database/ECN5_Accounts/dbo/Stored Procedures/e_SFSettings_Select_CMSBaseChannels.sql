CREATE PROCEDURE [dbo].[e_SFSettings_Select_CMSBaseChannels]
AS
BEGIN
	select * from SFSettings 
	where PushChannelMasterSuppression=1
END