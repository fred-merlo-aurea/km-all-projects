CREATE PROCEDURE [dbo].[e_Rule_Select_FormID]
	@FormID int
AS
	Select * from [Rule] with(nolock)
	where Form_Seq_ID = @FormID
	UNION
	Select r.* from [Rule] r with(nolock)
	join Control c with(nolock) on r.Control_ID = c.Control_ID
	where c.Form_Seq_ID = @FormID