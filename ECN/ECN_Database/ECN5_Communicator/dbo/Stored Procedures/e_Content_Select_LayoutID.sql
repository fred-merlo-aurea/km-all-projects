CREATE PROCEDURE [dbo].[e_Content_Select_LayoutID]   
@LayoutID int
AS
	SELECT 
		Content.* 
	FROM 
		Layout WITH (NOLOCK)
		LEFT OUTER JOIN Content WITH (NOLOCK) on	Content.ContentID = Layout.ContentSlot1 or     
													Content.ContentID = IsNull(Layout.ContentSlot2, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot3, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot4, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot5, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot6, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot7, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot8, 0) or     
													Content.ContentID = IsNull(Layout.ContentSlot9, 0)
	WHERE 
		LayoutID = @LayoutID and Layout.IsDeleted = 0
