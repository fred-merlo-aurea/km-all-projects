create procedure e_DataCompareRecordPriceRange_Select
as
	begin
		set nocount on
		select * from DataCompareRecordPriceRange with(nolock)
	end
go
