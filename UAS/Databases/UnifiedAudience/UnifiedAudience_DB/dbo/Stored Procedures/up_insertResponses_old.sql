create proc [dbo].[up_insertResponses_old]
(    
 	@PUBID Int,    
 	@SUBSCRIPTIONID int,    
  	@FIELDNAME varchar(255),    
 	@RESPONSEVALUE varchar(1000)    
)     
as     
BEGIN
	
	SET NOCOUNT ON
    
	if CHARINDEX(',', @RESPONSEVALUE) > 0      
		BEGIN    
				INSERT INTO SUBSCRIPTIONDETAILS    
				select @SUBSCRIPTIONID, MasterID 
				from
				(
					select distinct  MasterID 
					from CODESHEET R 
					join (select items from dbo.fn_Split(@RESPONSEVALUE, ',') WHERE isnull(rtrim(ltrim(ITEMS)),'') <> '') inn1 
						on R.RESPONSEVALUE = inn1.items and PUBID = @PUBID AND ResponseGroup = @FIELDNAME
				) inn2
				where not exists (select top 1 * from subscriptiondetails sd where sd.subscriptionID = @SUBSCRIPTIONID and sd.masterID = inn2.masterID)

				if (@@ROWCOUNT = 0)
					Begin
						Print (' Pub ID : "' + Convert(varchar,@PUBID) + '" - "' + @FIELDNAME + '" - "' + @RESPONSEVALUE + '" Not Found');
					End
			end    
		ELSE    
	BEGIN    
  		INSERT INTO SUBSCRIPTIONDETAILS    
		select @SUBSCRIPTIONID, MasterID 
		from
  		(
			select MasterID  
			from CODESHEET R 
			where RESPONSEVALUE = @RESPONSEVALUE and 
				PUBID = @PUBID AND 
				ResponseGroup = @FIELDNAME
		) inn2
		where not exists (select top 1 * from subscriptiondetails sd where sd.subscriptionID = @SUBSCRIPTIONID and sd.masterID = inn2.masterID)

			if (@@ROWCOUNT = 0)
				Begin
					Print (' Pub ID : ' + Convert(varchar,@PUBID) + ' - ' + @FIELDNAME + ' - ' + @RESPONSEVALUE + ' Not Found');
				End
	END 

End