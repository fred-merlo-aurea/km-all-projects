CREATE PROCEDURE [dbo].[e_LinkAlias_GetByBlastLink]  
(
	@BlastID int,
	@Link varchar(300)
) AS 
BEGIN
	SELECT 
		la.*, b.CustomerID
	FROM 
		[Blast] b WITH (NOLOCK) 
		JOIN [Layout] l WITH (NOLOCK) ON b.layoutID = l.layoutID and l.IsDeleted = 0
		JOIN [Content] c WITH (NOLOCK) ON (l.ContentSlot1 = c.contentID OR l.ContentSlot2 = c.contentID OR l.ContentSlot3 = c.contentID OR l.ContentSlot4 = c.contentID OR l.ContentSlot5 = c.contentID OR l.ContentSlot6 = c.contentID ) and c.IsDeleted = 0
		JOIN [linkAlias] la WITH (NOLOCK) ON  la.ContentID = c.ContentID AND la.IsDeleted = 0
	WHERE 
		b.blastID = @BlastID AND 
		la.Link =@link AND
		b.StatusCode <> 'Deleted'

END