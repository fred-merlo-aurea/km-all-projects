CREATE PROCEDURE [dbo].[rptGetAllMasterSuppressionSourceReportDetails] (@CustomerID INT)
AS

Begin

	SET NOCOUNT ON
		
	SELECT emailaddress, isnull(UC.UnsubscribeCode, '') AS UnsubscribeCode, MAX(ISNULL(bs.UnsubscribeTime, ISNULL(eg.LastChanged, eg.CreatedOn))) AS UnsubscribeDateTime, isnull(bs.Comments, '') AS Reason
	FROM ECN5_COMMUNICATOR..emailgroups eg 
	JOIN ecn5_communicator..emails e 
		ON e.emailID = eg.emailID 
	LEFT OUTER JOIN BlastActivityUnSubscribes bs 
		ON eg.EmailID = bs.EmailID 
	LEFT OUTER JOIN UnsubscribeCodes uc 
		ON uc.UnsubscribeCodeID = bs.UnsubscribeCodeID 
		AND uc.UnsubscribeCode IN ('ABUSERPT_UNSUB', 'MASTSUP_UNSUB') 
	WHERE groupID = (SELECT groupID 
					FROM ECN5_COMMUNICATOR..Groups 
					WHERE CustomerID = @CustomerID 
					AND MasterSupression = 1) 
	GROUP BY groupID, emailaddress, uc.UnsubscribeCode, bs.Comments
	ORDER BY UnsubscribeDateTime
	
	SET NOCOUNT OFF
End