CREATE PROCEDURE [dbo].[e_LandingPageAssignContent_GetSelectedReasons]
	@CustomerID int,
	@FromDate datetime, 
	@ToDate datetime
AS
	SET @FromDate = @FromDate + ' 00:00:00.000'
	SET @ToDate = @ToDate + ' 23:59:59.999'
	SELECT distinct bau.Comments
	FROM ECN_Activity..BlastActivityUnsubscribes bau with(nolock)
	join ECN5_Communicator..Blast b with(nolock) on bau.BlastID = b.BlastID
	where b.CustomerID = @CustomerID and bau.UnsubscribeTime between @FromDate and @ToDate and bau.Comments like '%Reason:%'
