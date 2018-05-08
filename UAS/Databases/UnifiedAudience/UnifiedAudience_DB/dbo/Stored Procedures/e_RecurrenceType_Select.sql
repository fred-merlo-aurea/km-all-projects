CREATE  PROCEDURE [dbo].[e_RecurrenceType_Select]   
AS  
BEGIN

	SET NOCOUNT ON

	Select * 
	from RecurrenceType with(nolock)  

End