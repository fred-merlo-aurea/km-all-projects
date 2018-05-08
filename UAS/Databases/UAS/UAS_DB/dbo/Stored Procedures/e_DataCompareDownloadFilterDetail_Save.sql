CREATE PROCEDURE [dbo].[e_DataCompareDownloadFilterDetail_Save]
@DcFilterGroupId int,
@FilterType int,
@Group varchar(100) = null,
@Name varchar(100) = null,
@Values varchar(max) = null,
@SearchCondition varchar(100) = null
AS
	begin
		set nocount on
		
		insert into DataCompareDownloadFilterDetail (DcFilterGroupId,FilterType,[Group],Name,[Values],SearchCondition)
		values(@DcFilterGroupId,@FilterType,@Group,@Name,@Values,@SearchCondition);
		
		select @@IDENTITY;
	end

