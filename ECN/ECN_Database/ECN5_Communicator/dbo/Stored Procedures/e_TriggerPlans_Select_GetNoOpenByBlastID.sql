CREATE PROCEDURE [dbo].[e_TriggerPlans_Select_GetNoOpenByBlastID]   
@BlastID int = NULL
AS
	SELECT 
		tp.* 
	FROM 
		TriggerPlans tp WITH (NOLOCK)
		JOIN Blast b WITH (NOLOCK) on tp.RefTriggerID = b.BlastID
	WHERE 
		tp.RefTriggerID = @BlastID AND 
		tp.EventType = 'NoOpen' AND
		Isnull(tp.[Status],'Y') = 'Y' AND
		tp.IsDeleted = 0 AND
		b.StatusCode <> 'Deleted'
