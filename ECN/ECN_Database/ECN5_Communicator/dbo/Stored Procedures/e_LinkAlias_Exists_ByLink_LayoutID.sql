CREATE PROCEDURE [dbo].[e_LinkAlias_Exists_ByLink_LayoutID]
	@LayoutID int = 0,
	@Link varchar(2000)
AS
	IF EXISTS	(
					SELECT TOP 1 la.AliasID 
					FROM LinkAlias la with (nolock) 
					join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
					join Layout l with(nolock) on c.ContentID = l.ContentSlot1 or 
											      c.ContentID = l.ContentSlot2 or 
												  c.ContentID = l.ContentSlot3 or 
												  c.ContentID = l.ContentSlot4 or 
												  c.ContentID = l.ContentSlot5 or 
												  c.ContentID = l.ContentSlot6 or 
												  c.ContentID = l.ContentSlot7 or 
												  c.ContentID = l.ContentSlot8 or 
												  c.ContentID = l.ContentSlot9	
					WHERE l.LayoutID= @LayoutID and la.Link = @Link and la.IsDeleted = 0 and c.IsDeleted = 0
				) 
	SELECT 1 ELSE SELECT 0
