CREATE proc [dbo].[pharmalive_SaveAdjustments]
(
	@emailID int,
	@groupID int,
	@TransEntryID varchar(100),
	@AdjType varchar(100),
	@AdjAmount varchar(100),
	@AdjExpDate varchar(100),
	@AdjDesc varchar(255)
)
as
Begin
	declare @guid varchar(100),
			@deferredAmount decimal(10,2),
			@enddate datetime,
			@AmountPaid decimal(10,2),
			@AdjustedAmount decimal(10,2),
			@today datetime

	
	set @today = getdate()

	set @guid = newID()

	set @deferredAmount = 0
	set @AmountPaid = 0

	select @deferredAmount = isnull(datavalue,0) from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
	where emailID = @emailID and groupID = @groupID and shortname ='deferredamount' and entryID = @TransEntryID

	select @AmountPaid = isnull(datavalue,0) from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
		where emailID = @emailID and groupID = @groupID and shortname ='AmountPaid' and entryID = @TransEntryID

	select @enddate = convert(datetime, datavalue) from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
		where emailID = @emailID and groupID = @groupID and shortname ='EndDate' and entryID = @TransEntryID

	if @deferredAmount = 0
		set @deferredAmount = @AmountPaid	

	select @AmountPaid, @enddate, @deferredAmount 

	insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
	select @emailID, groupdatafieldsID, @TransEntryID, @today, -1, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'TransEntryID'
	
	insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
	select @emailID, groupdatafieldsID, convert(varchar(10),@today, 101), @today, -1, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'AdjDate'
	
	insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
	select @emailID, groupdatafieldsID, @AdjType, @today, -1, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'AdjType'
	insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
	select @emailID, groupdatafieldsID, @AdjDesc, @today, -1, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'AdjDesc'
	
	if (@adjType='EXPIRATION')
	Begin
		insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
		select @emailID, groupdatafieldsID, @AdjExpDate, @today, null, @guid, getdate() from groupdatafields where groupID = @groupID and shortname = 'AdjExpDate'

		update [EMAILDATAVALUES] set datavalue = @AdjExpDate
		where emaildatavaluesID = 
		(select emaildatavaluesID from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
		where emailID = @emailID and entryID = @TransEntryID and groupID = @groupID and shortname ='enddate')
	End
	
	if (@adjType='DISCOUNT')
	Begin
		insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
		select @emailID, groupdatafieldsID, @AdjAmount, @today, null, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'AdjAmount'
	End

	if (@adjType='CANCEL')
	Begin
		set @AdjustedAmount = 0

		select @AdjustedAmount = isnull(sum(convert(decimal,datavalue)),0) from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
		where emailID = @emailID and groupID = @groupID and shortname ='AdjAmount' and entryID in
		(select distinct entryID from [EMAILDATAVALUES] edv1 join groupdatafields gdf1 on edv1.groupdatafieldsID = gdf1.groupdatafieldsID
		where emailID = @emailID and groupID = @groupID and shortname ='TransEntryID' and datavalue  = @TransEntryID)

		insert into Emaildatavalues (EmailID,GroupDatafieldsID,DataValue,ModifiedDate,SurveyGridID,EntryID,CreatedDate)
		select @emailID, groupdatafieldsID, @deferredAmount - @AdjustedAmount, @today, null, @guid, @today from groupdatafields where groupID = @groupID and shortname = 'AdjAmount'

		update [EMAILDATAVALUES] set datavalue = 'FREE'
		where emaildatavaluesID = (select emaildatavaluesID from [EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID where emailID = @emailID and groupID = @groupID and shortname ='paidorFree')
	End
End