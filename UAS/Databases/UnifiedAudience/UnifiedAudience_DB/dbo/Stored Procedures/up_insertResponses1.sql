CREATE proc [dbo].[up_insertResponses1]
(    
 	@PUBID Int,    
 	@SUBSCRIPTIONID int,    
  	@FIELDNAME varchar(255),    
 	@RESPONSEVALUE varchar(5000)    
)     
as     
BEGIN
	
	SET NOCOUNT ON

--	if len(rtrim(ltrim(@responsevalue))) <> 0
--	Begin
		--print(@FIELDNAME + ',' + @RESPONSEVALUE)
		if CHARINDEX(',', @RESPONSEVALUE) > 0
			insert into Tempincomingdatadetails --(pubid, subscriptionID, responsegroup, responsevalue)
			select @pubId,@SUBSCRIPTIONID,@fieldname, items, 1  from dbo.fn_split(@RESPONSEVALUE, ',')
		else
			insert into Tempincomingdatadetails --(pubid, subscriptionID, responsegroup, responsevalue)
			select @pubId,@SUBSCRIPTIONID,@fieldname, (case when ISNUMERIC(@RESPONSEVALUE) = 1 then CONVERT(varchar(100),CONVERT(int,@RESPONSEVALUE)) else @RESPONSEVALUE end), 1
			
		
		
--		set @responsevalue = '''' + Replace(@RESPONSEVALUE,',',''',''')  + ''''
--	    
--  		exec (' INSERT INTO SUBSCRIPTIONDETAILS ' + 
--			' 	select ' + @SUBSCRIPTIONID + ', MasterID  from CODESHEET R  ' +
--			' 	where 	RESPONSEVALUE in (' + @RESPONSEVALUE + ') and  ' +
--			' 			PUBID = ' + @PUBID + ' AND  ' +
--			' 			ResponseGroup = ''' + @FIELDNAME + '''')
--			--' where not exists (select top 1 * from subscriptiondetails sd where sd.subscriptionID = ' + @SUBSCRIPTIONID + ' and sd.masterID = inn2.masterID) ')
--
--			if @@ROWCOUNT = 0 
--				Print (' PubID=' + Convert(varchar,@PUBID)  + ',SubID=' + convert(varchar,@SUBSCRIPTIONID)  + ' / ' + @FIELDNAME + ' / ' + @RESPONSEVALUE) 
--	End

END