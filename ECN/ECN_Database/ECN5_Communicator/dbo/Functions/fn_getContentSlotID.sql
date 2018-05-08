CREATE FUNCTION [dbo].[fn_getContentSlotID](@layoutID int, @slotnumber int)   
RETURNS varchar(10)
AS       
  
BEGIN       
	declare @contentID int

	if @slotnumber = 1
		select @contentID = isnull(ContentSlot1,0) from Layout where layoutID = @layoutID
	if @slotnumber = 2
		select @contentID = isnull(ContentSlot2,0) from Layout where layoutID = @layoutID
	if @slotnumber = 3
		select @contentID = isnull(ContentSlot3,0) from Layout where layoutID = @layoutID
	if @slotnumber = 4
		select @contentID = isnull(ContentSlot4,0) from Layout where layoutID = @layoutID
	if @slotnumber = 5
		select @contentID = isnull(ContentSlot5,0) from Layout where layoutID = @layoutID
	if @slotnumber = 6
		select @contentID = isnull(ContentSlot6,0) from Layout where layoutID = @layoutID
	if @slotnumber = 7
		select @contentID = isnull(ContentSlot7,0) from Layout where layoutID = @layoutID
	if @slotnumber = 8
		select @contentID = isnull(ContentSlot8,0) from Layout where layoutID = @layoutID
	if @slotnumber = 9
		select @contentID = isnull(ContentSlot9,0) from Layout where layoutID = @layoutID

	
	return convert(varchar(10),@contentID)

END
