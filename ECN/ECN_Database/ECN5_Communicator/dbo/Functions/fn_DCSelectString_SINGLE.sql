CREATE FUNCTION [dbo].[fn_DCSelectString_SINGLE](@layoutID int)   
RETURNS  varchar(8000)  
AS       
  
BEGIN       
	declare @slotnumber int,
	@ContentID int,
	@whereclause varchar(500),
	@DynamicSlotstring varchar(8000),
	@oldSlotNumber int,
	@selectslotstr varchar(8000)


	set @selectslotstr = 'select %slot1% as slot1, %slot2% as slot2,%slot3% as slot3, %slot4% as slot4, %slot5% as slot5, %slot6% as slot6, %slot7% as slot7, %slot8% as slot8, %slot9% as slot9  ' 

	set @dynamicSlotString = ''
	set @oldSlotNumber = 0

	DECLARE ecursor CURSOR FOR 
	select SlotNumber, ContentID, WhereClause  from contentFilter where layoutID = @layoutID and IsNull(IsDeleted, 0) = 0 order by SlotNumber



	OPEN ecursor

	FETCH NEXT FROM ecursor INTO @slotnumber, @ContentID, @whereclause

	WHILE @@FETCH_STATUS = 0
	BEGIN

		if @oldSlotNumber <> @slotNumber
		Begin
		if @oldSlotNumber = 0 
			set @dynamicSlotString = ' case'
		else
		Begin
			set @dynamicSlotString = @dynamicSlotString + ' end '
			
			select @selectslotstr = replace(@selectslotstr, '%slot' + convert(varchar,@oldSlotNumber) + '%', @dynamicSlotString)
				
			set @dynamicSlotString = ' case'
		end

		while @oldSlotNumber+1 < @SlotNumber
		Begin
			select @selectslotstr = replace(@selectslotstr, '%slot' + convert(varchar,@oldSlotNumber+1) + '%', dbo.fn_getContentSlotID (@layoutID, @oldSlotNumber+1))
			set @oldSlotNumber = @oldSlotNumber + 1 
		End
		end

		set @dynamicSlotString = @dynamicSlotString + ' when ' + @whereclause + ' then ' + convert(varchar,@ContentID) 

		set @oldSlotNumber = @slotnumber

		FETCH NEXT FROM ecursor INTO @slotnumber, @ContentID, @whereclause

	end

	if @oldSlotNumber > 0
	Begin
		set @dynamicSlotString = @dynamicSlotString + ' end '
		select @selectslotstr = replace(@selectslotstr, '%slot' + convert(varchar,@oldSlotNumber) + '%', @dynamicSlotString)

		while @oldSlotNumber+1 < 10
		Begin
			select @selectslotstr = replace(@selectslotstr, '%slot' + convert(varchar,@oldSlotNumber+1) + '%', dbo.fn_getContentSlotID (@layoutID, @oldSlotNumber+1))
			set @oldSlotNumber = @oldSlotNumber + 1 
		End
	end
	CLOSE ecursor
	DEALLOCATE ecursor
	
	return @selectslotstr

END