CREATE PROCEDURE [dbo].[e_EmailDirect_GetNextToSend]
	
AS
	Select top 1 * 
	FROM EmailDirect ed with(nolock)
	where ed.Status = 'Pending' and ed.SendTime < GetDate() 
	Order by ed.SendTime asc
