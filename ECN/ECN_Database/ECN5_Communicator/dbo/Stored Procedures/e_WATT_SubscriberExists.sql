
CREATE PROCEDURE [dbo].[e_WATT_SubscriberExists] 
	@Token varchar(100),
	@IssueID int
AS
BEGIN
	if exists (SELECT top 1 * from EmailDataValues edv where edv.DataValue = @Token and 
		edv.GroupDatafieldsID in (SELECT GroupDatafieldsID from GroupDatafields gdf with(nolock) where gdf.ShortName = 'Mozaic_Issue_' + CONVERT(varchar(50), @IssueID)))
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END