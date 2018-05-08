CREATE PROCEDURE [dbo].[e_Form_Select_FormID]
	@FormSeqID int
AS
	Select * from Form f with(nolock) 
		join [ecn5_accounts].[dbo].[Customer] c WITH (NOLOCK) on f.CustomerID = c.CustomerID
        WHERE  f.ParentForm_ID is null
	    and f.Form_Seq_ID = @FormSeqID and (f.Active = 0 OR (Active = 2 and GetDate() between ActivationDateFrom and ActivationDateTo))