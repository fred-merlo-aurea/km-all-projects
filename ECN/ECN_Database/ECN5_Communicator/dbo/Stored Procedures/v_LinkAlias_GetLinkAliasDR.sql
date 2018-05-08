CREATE proc [dbo].[v_LinkAlias_GetLinkAliasDR] 
(
	@CustomerID int,
	@LayoutID int
)
as
Begin
	Set nocount on
	SELECT la.Link, la.Alias
	FROM 
		LinkAlias la WITH (NOLOCK)
		JOIN Layout lt  WITH (NOLOCK) ON ( 
											(la.ContentID =lt.ContentSlot1 and ISNULL(lt.ContentSlot1,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot2 and ISNULL(lt.ContentSlot2,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot3 and ISNULL(lt.ContentSlot3,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot4 and ISNULL(lt.ContentSlot4,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot5 and ISNULL(lt.ContentSlot5,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot6 and ISNULL(lt.ContentSlot6,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot7 and ISNULL(lt.ContentSlot7,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot8 and ISNULL(lt.ContentSlot8,0) <> 0) OR 
											(la.ContentID = lt.ContentSlot9 and ISNULL(lt.ContentSlot9,0) <> 0) )
	WHERE 
		lt.CustomerID= @CustomerID AND 
		lt.LayoutID = @LayoutID AND
		la.IsDeleted = 0 AND
		lt.IsDeleted = 0
	ORDER BY lt.LayoutName 

 
End
