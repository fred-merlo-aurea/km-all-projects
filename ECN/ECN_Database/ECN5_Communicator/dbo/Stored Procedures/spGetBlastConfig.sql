CREATE PROCEDURE [dbo].[spGetBlastConfig] (
	@CustomerID int
) AS
BEGIN
	SET NOCOUNT ON
	
	IF EXISTS(SELECT TOP 1 bc.BlastConfigID
		FROM MTACustomer mtac WITH(NOLOCK) 
			JOIN MTA m WITH(NOLOCK) ON mtac.MTAID = m.MTAID
			JOIN BlastConfig bc WITH(NOLOCK) ON m.BlastConfigID = bc.BlastConfigID
		WHERE mtac.CustomerID = @CustomerID)
	BEGIN
		SELECT TOP 1 bc.*
		FROM MTACustomer mtac WITH(NOLOCK) 
			JOIN MTA m WITH(NOLOCK) ON mtac.MTAID = m.MTAID
			JOIN BlastConfig bc WITH(NOLOCK) ON m.BlastConfigID = bc.BlastConfigID
		WHERE mtac.CustomerID = @CustomerID
		ORDER BY IsDefault DESC
	END
	ELSE
	BEGIN
		SELECT TOP 1 bc.*
		FROM ecn5_communicator..BlastConfig bc WITH(NOLOCK) 
			JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c WITH(NOLOCK) ON bc.BlastConfigID = c.BlastConfigID
		WHERE c.CustomerID = @CustomerID
	END
END
