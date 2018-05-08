CREATE PROCEDURE [dbo].[rptGetMasterSuppressionSourceReport] 
	@CustomerID int,
	@FromDate date,
	@ToDate date
AS
--DECLARE
--	@CustomerID int = 1,
--	@FromDate varchar(20) = '1/27/2015',
--	@ToDate varchar(20) = '4/27/2015'


BEGIN

	SET NOCOUNT ON
		
		DECLARE @mastersuppressiongroupID INT 
		
		SELECT 
			@mastersuppressiongroupID = groupID 
		FROM 
			ECN5_COMMUNICATOR..Groups 
		WHERE 
			CustomerID = @customerID 
			AND MasterSupression = 1 

		SELECT 
			groupID, 
			isnull(uc.UnsubscribeCodeID, 0) as UnsubscribeCodeID,
			ISNULL(UnsubscribeCode,'') as UnsubscribeCode, 
			COUNT(DISTINCT eg.emailID) AS [Count]
		FROM 
			ECN5_COMMUNICATOR..emailgroups eg 
			LEFT OUTER JOIN BlastActivityUnSubscribes bs ON eg.EmailID = bs.EmailID 
			LEFT OUTER JOIN UnsubscribeCodes uc ON uc.UnsubscribeCodeID = bs.UnsubscribeCodeID 
		WHERE 
			GroupID = @mastersuppressiongroupID 
			AND ISNULL(UnsubscribeCode,'') IN ('ABUSERPT_UNSUB', 'MASTSUP_UNSUB','') 
			AND cast (ISNULL(bs.UnsubscribeTime, ISNULL(eg.LastChanged, eg.CreatedOn)) as date) BETWEEN @FromDate AND @ToDate
		GROUP BY 
			groupID, 
			uc.UnsubscribeCode,
			uc.UnsubscribeCodeID

	SET NOCOUNT OFF
END

GO


