CREATE FUNCTION [dbo].[fn_getGCCouponData](@coupondata varchar(200), @type varchar(10))   
RETURNS  varchar(50)  
as
Begin

declare @coupontype varchar(50),
		@couponvalue varchar(50),
		@couponexpiration varchar(50),
		@returnvalue varchar(50)

set @returnvalue = ''

set @coupontype = SUBSTRING(@coupondata, 1, charindex('_',@coupondata)-1)
set @coupondata = REPLACE(@coupondata, @coupontype + '_', '')

set @couponvalue = SUBSTRING(@coupondata, 1, charindex('_',@coupondata)-1)
set @couponexpiration = REPLACE(@coupondata, @couponvalue + '_', '')

if @type = 'type'
	set @returnvalue = @coupontype
	
if @type = 'value'
	set @returnvalue = @couponvalue

if @type='exp'
 	set @returnvalue = @couponexpiration

return @returnvalue
 	
End
