CREATE FUNCTION dbo.getregions(@SuperRegionID int)     
RETURNS  varchar(8000)    
AS         
    
BEGIN         
	declare @string varchar(8000)  
  
	set @string = ''  
  
	select  @string = @string + coalesce(Convert(varchar,regionID) + ',','') FROM NEBOOK_Region_SuperRegion where SuperRegionID = @SuperRegionID  

	if @string <> ''   
		set @string = substring(@string,1,len(@string)-1)  

	return @string
END     
  
