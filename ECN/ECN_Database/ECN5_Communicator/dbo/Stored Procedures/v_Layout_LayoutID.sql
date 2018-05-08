CREATE  PROC [dbo].[v_Layout_LayoutID] 
(
	@LayoutID int = NULL,
	@BaseChannelID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	select l.*, t.TemplateSource, t.TemplateText, t.SlotsTotal, 
		(convert(varchar(255),l.LayoutID) + '&chID=' + convert(varchar,@BaseChannelID) + '&cuID='  + convert(varchar,@CustomerID)) as LayoutIDPlus 
	from 
		Layout l with (nolock) 
		join Template t with (nolock) on l.TemplateID = t.TemplateID
	where 
		l.LayoutID = @LayoutID AND 
		l.IsDeleted = 0 AND
		t.IsDeleted = 0
END
