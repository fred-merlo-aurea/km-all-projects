CREATE PROC [dbo].[sp_getEmailsListFromGroup]
	@groupID int
AS 
BEGIN 
	SELECT e.EmailID, e.EmailAddress, e.FirstName, e.LastName, e.Voice, eg.GroupID, eg.FormatTypeCode, eg.SubscribeTypeCode, eg.CreatedOn, eg.LastChanged, 
		(isnull(Convert(varchar,eg.CreatedOn),'')+'<br>'+ isnull(Convert(varchar,eg.LastChanged),'n/a')) as 'Dates'
	FROM Emails e with(nolock) join EmailGroups eg with(nolock) ON e.EmailID = eg.EmailID AND eg.groupID = @groupID
	ORDER BY EmailAddress
END
