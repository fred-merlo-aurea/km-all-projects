-- ======================================
-- Description: Returns as a record set .
-- The input can be a string which is separated by any delimiter
-- ======================================
CREATE FUNCTION [dbo].[SplitStr]
(
@InpString varchar(2500),
@Delimiter char(1)
)
RETURNS @tmptable TABLE (items varchar(2500))
AS
BEGIN
Declare @indx int
Declare @StrPart varchar(2500)

Select @indx = 1
if len(@InpString)<1 or @InpString is null RETURN

While @indx!= 0
Begin
set @indx = charindex(@Delimiter,@InpString)
if @indx!=0
set @StrPart = left(@InpString,@indx - 1)
else
set @StrPart = @InpString

if(len(@StrPart)>0)
insert into @tmptable(Items) values(@StrPart)

set @InpString = right(@InpString,len(@InpString) - @indx)
if len(@InpString) = 0 break
End
return

END