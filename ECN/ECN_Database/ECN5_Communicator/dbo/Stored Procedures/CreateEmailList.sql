CREATE  Proc [dbo].[CreateEmailList] (
@startIndex as integer,
@count as integer)
as 
Begin
declare @i as integer
set @i = 1
	while (@i <= @count) 
	   Begin
		print 'fakemail_' + cast(@startIndex as varchar(10)) + '@teckman.com'
		set @startIndex = @startIndex + 1
		set @i = @i +1
	   End
End
