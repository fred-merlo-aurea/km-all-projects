CREATE proc [dbo].[v_Layout_GetLayoutDR] 
(
	@CustomerID int,
	@UserID int
)
as
Begin
	Set nocount on
			
	SELECT DISTINCT LayoutID, (LayoutName + ' - ' + convert(varchar(10), IsNull(l.UpdatedDate, IsNull(l.CreatedDate, '')), 101)) AS 'LayoutName' 
	FROM 
		Layout l WITH (NOLOCK)
		left outer join Content c with(nolock) on l.ContentSlot1 = c.ContentID 
                            or l.ContentSlot2 = c.ContentID 
                            or l.ContentSlot3 = c.ContentID 
                            or l.ContentSlot4 = c.ContentID
                            or l.ContentSlot5 = c.ContentID 
                            or l.ContentSlot6 = c.ContentID
                            or l.ContentSlot7 = c.ContentID
                            or l.ContentSlot8 = c.ContentID
                            or l.ContentSlot9 = c.ContentID

	WHERE 
		l.CustomerID= @CustomerID AND 
		l.IsDeleted = 0
		and ISNULL(l.ContentSlot1,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot1,0) ELSE null END   
		and ISNULL(l.ContentSlot2,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot2,0) ELSE null END 
		and ISNULL(l.ContentSlot3,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot3,0) ELSE null END
	    and ISNULL(l.ContentSlot4,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot4,0) ELSE null END 
		and ISNULL(l.ContentSlot5,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot5,0) ELSE null END 
		and ISNULL(l.ContentSlot6,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot6,0) ELSE null END
	    and ISNULL(l.ContentSlot7,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot7,0) ELSE null END 
		and ISNULL(l.ContentSlot8,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot8,0) ELSE null END
	    and ISNULL(l.ContentSlot9,0) = Case when ISNULL(c.IsValidated, 0) = 1 THEN ISNULL(l.ContentSlot9,0) ELSE null END 
		   
	ORDER BY LayoutName 

 
End