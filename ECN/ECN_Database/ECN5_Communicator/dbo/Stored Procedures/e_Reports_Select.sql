CREATE  PROC dbo.e_Reports_Select 
AS 
BEGIN
	select * from Reports with (NOLOCK)
END