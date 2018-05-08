CREATE proc spCDSRemoveDuplicates (@pubcode varchar(25), @inputdata text)
as
Begin
set nocount on

	declare @docHandle int

	create table #tmpInput (eml varchar(255), sub int, crd varchar(100))

	CREATE INDEX EA_1 on #tmpInput (eml)

	create table #dupes (eml varchar(255), cdsID int)

	CREATE INDEX EA_2 on #dupes (eml)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @inputdata 


	insert into #tmpInput 
		(
			eml, sub, crd
		)  
		SELECT 
			LTRIM(RTRIM(eml)), sub, crd
		FROM OPENXML(@docHandle, N'/xml/record')   
		WITH   
		(  
			eml varchar(255) 'eml', sub int 'sub', crd varchar(100) 'crd'
		) 

	EXEC sp_xml_removedocument @docHandle   

	INSERT INTO #dupes 
	select EML, MIN(sub) as cdsID 
	from #tmpInput
	where ISNULL(eml, '') <> '' 
	group by EML
	having COUNT(*) > 1 

	update #tmpInput
	set EML = ''
	where SUB in (select t1.sub from #tmpInput t1 join #dupes d on t1.EML = d.eml and t1.SUB <> d.cdsID)

	drop table #dupes

	--select COUNT(*) as Total_Blanks from #tmpInput where ISNULL(eml, '') = '' 

	update #tmpInput set EML =  CRD + '-' + convert(varchar(25),SUB) + '@' + @pubcode +'.KMPSGROUP.COM'  where ISNULL(eml, '') = '' 

	select * from #tmpInput


	drop table #tmpInput

End
