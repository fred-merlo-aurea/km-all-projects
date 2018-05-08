CREATE PROCEDURE [dbo].[e_Country_Select_For_Report]
AS
BEGIN

	set nocount on

	SELECT ShortName, CountryID FROM Country With(NoLock)
	WHERE FullName is not null
	UNION ALL
	SELECT 'No Response', 0

END