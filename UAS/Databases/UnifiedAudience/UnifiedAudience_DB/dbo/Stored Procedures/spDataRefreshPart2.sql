CREATE proc [dbo].[spDataRefreshPart2]
as
BEGIN

	SET NOCOUNT ON


	SELECT 'case when PubID	in (' + STUFF((SELECT ',' + convert(varchar(100),PubID) FROM dbo.codesheet c1
											WHERE c1.responsegroup = c.ResponseGroup
											group by PubID ORDER BY PubID FOR xml PATH ('')) , 1, 1, '') + 
				 ')	then [' + Upper(ResponseGroup)+ '] ELSE '''' end as ''' + Upper(ResponseGroup) + ''',',
		'@' + Upper(ResponseGroup), 
		'isnull(ltrim(rtrim(replace(iData.[' + Upper(ResponseGroup)+ '],'' '' ,''''))), '''') as ''' + Upper(ResponseGroup)+ ''',',
		Upper(ResponseGroup), 
		STUFF((SELECT ',' + convert(varchar(100),PubID) FROM dbo.codesheet c1
				WHERE c1.responsegroup = c.ResponseGroup
				group by PubID ORDER BY PubID FOR xml PATH ('')) , 1, 1, '') as Pubids
	FROM codesheet c
	group by responsegroup
	order by responsegroup



	--declare @GROUP VARCHAR(200),
	--	@PUBid INT,
	--	@i int

	--set @i = 1
	--set nocount on

	--DECLARE @X TABLE (RESPONSEGROUP VARCHAR(100), PUBids varchar(8000))

	--DECLARE c_Dups CURSOR FOR select distinct upper(responsegroup), PubID from codesheet order by 1, 2

	--OPEN c_Dups  
	--FETCH NEXT FROM c_Dups INTO @GROUP, @PUBid

	--WHILE @@FETCH_STATUS = 0  
	--BEGIN  

	--	if exists (select top 1 * from @X where RESPONSEGROUP = @GROUP)
	--		update @X set PUBids = ISNULL(pubids,'')  + ',' + convert(varchar(10),@PUBid) where RESPONSEGROUP =  @GROUP
	--	else 
	--		insert into @X values (@GROUP, @PUBid )
		
	--	FETCH NEXT FROM c_Dups INTO @GROUP, @PUBid
	--END

	--CLOSE c_Dups  
	--DEALLOCATE c_Dups  

	--select 'case when PubID	in (' + PUBids + ')	then [' + RESPONSEGROUP+ '] ELSE '''' end as ''' + RESPONSEGROUP + ''',', '@' + RESPONSEGROUP, 
	--'isnull(ltrim(rtrim(replace(iData.[' + RESPONSEGROUP+ '],'' '' ,''''))), '''') as ''' + RESPONSEGROUP+ ''',' ,  * from @x	
	
	-----------------------------------------------------------------------------------------------------------------
END