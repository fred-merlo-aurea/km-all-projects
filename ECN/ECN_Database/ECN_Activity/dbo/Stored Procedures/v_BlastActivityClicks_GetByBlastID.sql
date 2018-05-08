CREATE PROCEDURE [dbo].[v_BlastActivityClicks_GetByBlastID]  
(
	@BlastID int,
	@CustomerID int
) AS 
BEGIN
	SELECT 
		bacl.ClickTime, e.EmailAddress as EmailAddress, bacl.URL AS FullLink  
	FROM 
		[BlastActivityClicks] bacl WITH (NOLOCK)
		JOIN ecn5_communicator..[Emails] e WITH (NOLOCK) ON bacl.EMailID = e.EMailID 
	WHERE 
		BlastID = @BlastID AND
		e.CustomerID = @CustomerID
	ORDER BY 
		ClickTime DESC

END