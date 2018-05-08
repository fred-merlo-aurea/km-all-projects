
CREATE PROCEDURE [dbo].[e_WATT_GetNextTokenForSubscriber] 
	@Token varchar(100),
	@IssueID int
AS
BEGIN
	DECLARE @EmailID int
	
	SELECT @EmailID = EmailID 
	FROM EmailDataValues edv with(nolock)
	WHERE edv.DataValue = @Token and edv.GroupDatafieldsID in (SELECT GroupDatafieldsID from GroupDatafields with(nolock) where ShortName like 'Mozaic_Issue%')
	
	SELECT edv.DataValue 
	FROM EmailDataValues edv with(nolock)
	JOIN GroupDatafields gdf with(nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
	where edv.EmailID = @EmailID and gdf.ShortName = 'Mozaic_Issue_' + CONVERT(varchar(50), @IssueID)
	
END