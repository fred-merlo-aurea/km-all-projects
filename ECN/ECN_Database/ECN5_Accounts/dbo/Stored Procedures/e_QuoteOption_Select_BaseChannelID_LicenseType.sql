create PROCEDURE [dbo].[e_QuoteOption_Select_BaseChannelID_LicenseType]   
@BaseChannelID int,
@LicenseType  int
AS
	SELECT * FROM QuoteOption where BaseChannelID = @BaseChannelID and LicenseType = @LicenseType and IsDeleted = 0
