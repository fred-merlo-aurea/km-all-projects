CREATE  PROC [dbo].[e_Layout_Exists_ContentID] 
(
	@ContentID int = NULL
)
AS 
BEGIN
	IF	exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot1=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot2=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot3=@ContentID and IsDeleted = 0) OR 
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot4=@ContentID and IsDeleted = 0) OR 
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot5=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot6=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot7=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot8=@ContentID and IsDeleted = 0) OR
		exists (select top 1 LayoutID from Layout WITH (NOLOCK) where ContentSlot9=@ContentID and IsDeleted = 0) 
	BEGIN
		select 1 
	END
	ELSE 
	BEGIN
		select 0
	END
END
