CREATE proc [dbo].[rpt_Blast_ByStatus] 
(
	@Status varchar(50),
	@BaseChannelID int = NULL
)
as
Begin
	Set nocount on
	
	IF @BaseChannelID IS NULL
	BEGIN	
		SELECT 
			b.BlastID, b.EmailSubject, b.SendTime, b.BlastType, c.CustomerName, bc.BaseChannelName, b.StatusCode, g.GroupName  
		FROM 
			Blast b  WITH (NOLOCK)
			JOIN ecn5_accounts..Customer c WITH (NOLOCK) ON b.CustomerID = c.CustomerID 
			JOIN ecn5_accounts..BaseChannel bc WITH (NOLOCK) ON c.BaseChannelID = bc.BaseChannelID
			JOIN Groups g WITH (NOLOCK) on b.GroupID = g.GroupID 
		WHERE  
			b.StatusCode = @Status 
		ORDER BY 
			bc.baseChannelName, b.SendTime
	END
	ELSE
	BEGIN
		SELECT 
			b.BlastID, b.EmailSubject, b.SendTime, b.BlastType, c.CustomerName, bc.BaseChannelName, b.StatusCode, g.GroupName  
		FROM 
			Blast b WITH (NOLOCK) 
			JOIN ecn5_accounts..Customer c WITH (NOLOCK) ON b.CustomerID = c.CustomerID 
			JOIN ecn5_accounts..BaseChannel bc WITH (NOLOCK) ON c.BaseChannelID = bc.BaseChannelID
			JOIN Groups g WITH (NOLOCK) on b.GroupID = g.GroupID 
		WHERE  
			b.StatusCode = @Status  AND 
			c.BaseChannelID = @BaseChannelID
		ORDER BY 
			bc.baseChannelName, b.SendTime
	END

 
End