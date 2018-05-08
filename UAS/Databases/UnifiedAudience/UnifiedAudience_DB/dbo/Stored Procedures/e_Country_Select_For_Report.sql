CREATE PROCEDURE [dbo].[e_Country_Select_For_Report]
AS
BEGIN

	SET NOCOUNT ON

	SELECT ShortName, CountryID 
	FROM UAD_Lookup..Country With(NoLock)
	WHERE FullName is not null
	UNION ALL
	SELECT 'No Response', 0

END