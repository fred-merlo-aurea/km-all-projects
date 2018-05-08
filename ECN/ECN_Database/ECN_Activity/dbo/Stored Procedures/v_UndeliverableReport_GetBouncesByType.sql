CREATE PROCEDURE [dbo].[v_UndeliverableReport_GetBouncesByType]  
(
	@StartDate Date,
	@EndDate Date,
	@CustomerID int,
	@BounceCodeID int
) AS 
BEGIN

	select e.EmailID, e.EmailAddress, x.*, b.BounceMessage AS [Message]
	from BlastActivityBounces b with (nolock)
	join ecn5_communicator..Emails e with (nolock)
		on b.EmailID = e.EmailID
	join (	select BlastID, EmailSubject, Sendtime, LayoutName 
			from ecn5_communicator..Blast b with (nolock)
			join ecn5_communicator..Layout l with (nolock)
				on b.LayoutID = l.LayoutID
			where b.CustomerID = @CustomerID 
			and TestBlast = 'n' 
			and convert(date,SendTime) between @StartDate and @EndDate
			and StatusCode = 'sent'
		 ) x on x.BlastID = b.BlastID 
	and b.BounceCodeID = @BounceCodeID

END