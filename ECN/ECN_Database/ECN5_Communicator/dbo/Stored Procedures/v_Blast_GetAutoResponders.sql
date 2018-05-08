CREATE PROCEDURE [dbo].[v_Blast_GetAutoResponders]  
	@CustomerID int	= NULL
AS
BEGIN
	SELECT b.*, g.GroupName
    FROM Blast b WITH (NOLOCK), [Groups] g WITH (NOLOCK)
    WHERE 
		b.CustomerID = @CustomerID
		AND b.BlastType = 'autoresponder'
		AND g.GroupID = b.GroupID 
		AND g.CustomerID = b.CustomerID
		AND b.StatusCode <> 'Deleted'
END