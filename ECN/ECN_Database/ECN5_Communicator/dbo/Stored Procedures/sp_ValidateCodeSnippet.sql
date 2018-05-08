CREATE PROCEDURE [dbo].[sp_ValidateCodeSnippet]            
(            
 @layoutID varchar(100),            
 @groupID varchar(100)            
)                  
AS                  
Begin              
            
	set nocount on   
	         
	if CHARINDEX(',', @layoutID, 1) = 0
	Begin            
		if len(@layoutID) > 0 and len(@groupID) > 0            
		Begin   
		        
		 declare @contentlength int  
		 set @contentlength = 0  
		  
		 select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join [LAYOUT] l on c.contentID = l.ContentSlot1 where layoutID = @layoutID  
		  
		 if @contentlength < 8000  
		 Begin  
			  -- test1    
			  create table #cs (colname varchar(100))            
			  create table #ly (contentsource varchar(8000))            
			  create table #acs (colname varchar(100))            
			          
			  insert into #cs values ('BlastID')            
			  insert into #cs values ('GroupID')            
			  insert into #cs values ('GroupName')  
			  insert into #cs values ('emailtofriend')            
			  insert into #cs values ('ConversionTrkCDE')          
			  insert into #cs values ('unsubscribelink')       
			  insert into #cs values ('LastChanged')      
			  insert into #cs values ('CreatedOn')      
			  insert into #cs values ('publicview')         
			  insert into #cs values ('company_address')       
			  insert into #cs values ('SurveyTitle')   
			  insert into #cs values ('SurveyLink')          
			  insert into #cs values ('CurrDate')  
			      
			  insert into #cs values ('customer_name')    
			  insert into #cs values ('customer_address')    
			  insert into #cs values ('customer_webaddress')    
			  
			  insert into #cs values ('customer_udf1')    
			  insert into #cs values ('customer_udf2')    
			  insert into #cs values ('customer_udf3')    
			  insert into #cs values ('customer_udf4')    
			  insert into #cs values ('customer_udf5')    
			     
			  declare @contentsource varchar(8000),            
				@contenttext varchar(8000)            
			             
			  exec (' insert into #cs select lower(c.name) from sysobjects o join syscolumns c on o.id = c.id where o.name = ''Emails''            
			  union             
			  select shortname from groupdatafields where groupID in ('+ @groupID +')')            
			             
			  exec(' insert into #ly select ContentSource  from            
			  [LAYOUT] join                
			  Content on  Content.ContentID = [LAYOUT].ContentSlot1 or Content.ContentID = [LAYOUT].ContentSlot2 or  Content.ContentID = [LAYOUT].ContentSlot3 or                 
				Content.ContentID = [LAYOUT].ContentSlot4 or Content.ContentID = [LAYOUT].ContentSlot5 or Content.ContentID = [LAYOUT].ContentSlot6 or                 
				Content.ContentID = [LAYOUT].ContentSlot7 or Content.ContentID = [LAYOUT].ContentSlot8 or Content.ContentID = [LAYOUT].ContentSlot9             
			  where [LAYOUT].layoutID in (' + @layoutID + ') union all            
			  select ContentText from            
			  [LAYOUT] join                
			  Content on  Content.ContentID = [LAYOUT].ContentSlot1 or Content.ContentID = [LAYOUT].ContentSlot2 or  Content.ContentID = [LAYOUT].ContentSlot3 or                 
				Content.ContentID = [LAYOUT].ContentSlot4 or Content.ContentID = [LAYOUT].ContentSlot5 or Content.ContentID = [LAYOUT].ContentSlot6 or                 
				Content.ContentID = [LAYOUT].ContentSlot7 or Content.ContentID = [LAYOUT].ContentSlot8 or Content.ContentID = [LAYOUT].ContentSlot9             
			  where [LAYOUT].layoutID in (' + @layoutID + ')')                
			             
			             
			  DECLARE cursor_cs CURSOR FOR             
			  select * from #ly            
			             
			  OPEN cursor_cs            
			          
			  FETCH NEXT FROM cursor_cs INTO @contentsource            
			          
			  WHILE @@FETCH_STATUS = 0            
			  BEGIN            
			   if len(@contentsource) > 0         
				insert into #acs            
				select items from (select Items from dbo.fn_splitcodesnippet(@contentsource,'%%')) inn left outer join #acs a on inn.items = a.colname where a.colname is null            
			             
			   FETCH NEXT FROM cursor_cs INTO @contentsource            
			  end            
			             
			  CLOSE cursor_cs    
			  DEALLOCATE cursor_cs            
			             
			  declare @csnotexists varchar(4000)            
			  set @csnotexists= ''            
			  select @csnotexists = @csnotexists + '%%' + a.colname + '%%, ' from #acs a left outer join #cs b on a.colname = b.colname where b.colname is null            
			             
			  if len(@csnotexists) > 0   
			  Begin            
			   set @csnotexists = left(@csnotexists, len(@csnotexists)-1)            
			   RAISERROR ('CodeSnippet "%s" not exist in the list/group.<BR>Please review the content.',16,1,@csnotexists)            
			  end            
			          
			  drop table #cs            
			  drop table #ly            
			  drop table #acs            
			end  
		end   
	end	         
End
