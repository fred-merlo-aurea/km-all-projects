CREATE PROCEDURE [dbo].[e_UnicodeLookup_Select_UnicodeNum]
	@UnicodeNumber varchar(20)
AS
	SELECT TOP 1 * FROM UnicodeLookup ul with(nolock)
	WHERE ul.UnicodeNum = @UnicodeNumber and ul.IsEnabled = 1
