CREATE PROCEDURE [dbo].[sp_Import_Product]
	@importXML TEXT,
	@userID int
AS
BEGIN	
		
	SET NOCOUNT ON
	
 --	exec sp_Import_Product 
--'<productlist>
--  <product>
--    <pubname>Phar1</pubname>
--    <pubcode>PHAR1</pubcode>
--    <istradeshow></istradeshow>
--    <pubtype>PUB</pubtype>
--    <enablesearching>1</enablesearching>
--    <score>1</score>
--    <yearstartdate></yearstartdate>
--    <yearenddate></yearenddate>
--    <issuedate></issuedate>
--    <isimported></isimported>
--    <isactive>1</isactive>
--    <allowdataentry></allowdataentry>
--    <kmimportallowed></kmimportallowed>
--    <clientimportallowed></clientimportallowed>
--    <addremoveallowed></addremoveallowed>
--    <acsmailerinfo></acsmailerinfo>
--    <isuad></isuad>
--    <iscirc></iscirc>
--    <isopencloselocked></isopencloselocked>
--    <haspaidrecords></haspaidrecords>
--    <usesubgen></usesubgen>
--	  <brand></brand>
--  </product>
--</productlist>',521

 	DECLARE @docHandle INT
	DECLARE @dt datetime = getdate() 

	CREATE TABLE #tmpData (
	PubName varchar(100) NOT NULL,
	istradeshow bit NULL,
	PubCode varchar(50) NULL,
	PubTypeID int NULL,
	PubType varchar(50) NULL,
	EnableSearching bit NULL,
	Score int NULL,
	YearStartDate varchar(5) NULL,
	YearEndDate varchar(5) NULL,
	IssueDate datetime NULL,
	IsImported bit NULL,
	IsActive bit NULL,
	AllowDataEntry bit NULL,
	KMImportAllowed bit NULL,
	ClientImportAllowed bit NULL,
	AddRemoveAllowed bit NULL,
	IsUAD bit NULL,
	IsCirc bit NULL,
	IsOpenCloseLocked bit NULL,
	HasPaidRecords bit NULL,
	UseSubGen bit NULL,
	Brand varchar(50) NULL
	)
	
	CREATE TABLE #tmpBrand(
	PubCode varchar(50),
	PubID int,
	BrandID int	)


	EXEC sp_xml_preparedocument @docHandle OUTPUT, @importXML  

	INSERT INTO #tmpData (
		PubName,
		istradeshow,
		PubCode,
		PubType,
		EnableSearching,
		Score,
		YearStartDate,
		YearEndDate,
		IssueDate,
		IsImported,
		IsActive,
		AllowDataEntry,
		KMImportAllowed,
		ClientImportAllowed,
		AddRemoveAllowed,
		IsUAD,
		IsCirc,
		IsOpenCloseLocked,
		HasPaidRecords,
		UseSubGen,
		Brand
		)  
	SELECT 	DISTINCT 
		PubName,
		istradeshow,
		PubCode,
		PubType,
		EnableSearching,
		Score,
		YearStartDate,
		YearEndDate,
		IssueDate,
		IsImported,
		IsActive,
		AllowDataEntry,
		KMImportAllowed,
		ClientImportAllowed,
		AddRemoveAllowed,
		IsUAD,
		IsCirc,
		IsOpenCloseLocked,
		HasPaidRecords,
		UseSubGen,
		Brand
	FROM 
		OPENXML(@docHandle, N'/productlist/product')   
	WITH   
		(  
			PubName varchar(100) 'pubname',
			istradeshow bit 'istradeshow',
			PubCode varchar(50) 'pubcode',
			PubType varchar(50) 'pubtype',
			EnableSearching bit 'enablesearching',
			Score int 'score',
			YearStartDate varchar(5) 'yearstartdate',
			YearEndDate varchar(5) 'yearenddate',
			IssueDate datetime 'issuedate',
			IsImported bit 'isimported',
			IsActive bit 'isactive',
			AllowDataEntry bit 'allowdataentry',
			KMImportAllowed bit 'kmimportallowed',
			ClientImportAllowed bit 'clientimportallowed',
			AddRemoveAllowed bit 'addremoveallowed',
			IsUAD bit 'isuad',
			IsCirc bit 'iscirc',
			IsOpenCloseLocked bit 'isopencloselocked',
			HasPaidRecords bit 'haspaidrecords',
			UseSubGen bit 'usesubgen',
			Brand varchar(50) 'brand'
		) 

	EXEC sp_xml_removedocument @docHandle  
	
	update t
	set t.PubTypeID = pt.PubTypeID
	from #tmpData t join PubTypes pt on t.PubType = pt.PubTypeDisplayName
	
	if exists (select top 1 1 from #tmpData where PubTypeID is null)
	Begin
		RAISERROR (N'Invalid pubtypes in file', -- Message text.
           16, -- Severity,
           1); 
	End
	Else
	Begin
	
		Insert into #tmpBrand	
		select PubCode, null as pubID, b.BrandID
		from #tmpData t
		cross apply fn_Split (t.brand, ',') bd 
		left outer join Brand b on b.BrandName = bd.Items

		if exists (select top 1 1 from #tmpBrand where BrandID is null)
		Begin
			RAISERROR (N'Invalid Brand Name in file', -- Message text.
			   16, -- Severity,
			   1); 
		End
		Else
		Begin 
			Insert into Pubs 
			(
				PubName,
				istradeshow,
				PubCode,
				PubTypeID,
				EnableSearching,
				Score,
				YearStartDate,
				YearEndDate,
				IssueDate,
				IsImported,
				IsActive,
				AllowDataEntry,
				KMImportAllowed,
				ClientImportAllowed,
				AddRemoveAllowed,
				IsUAD,
				IsCirc,
				IsOpenCloseLocked,
				HasPaidRecords,
				UseSubGen,
				SortOrder,
				DateCreated,
				CreatedByUserID		
			)
			select distinct 
				t.PubName, 
				t.istradeshow, 
				t.PubCode, 
				t.PubTypeID,
				t.EnableSearching,
				t.Score,
				t.YearStartDate,
				t.YearEndDate,
				t.IssueDate,
				t.IsImported,
				t.IsActive,
				t.AllowDataEntry,
				t.KMImportAllowed,
				t.ClientImportAllowed,
				t.AddRemoveAllowed,
				t.IsUAD,
				t.IsCirc,
				t.IsOpenCloseLocked,
				t.HasPaidRecords,
				t.UseSubGen, 
				null, 
				@dt,  
				@userID
			from 
					#tmpData t  left outer join 
					pubs p on t.PubCode = p.PubCode and t.PubName = p.PubName
			where 
					p.PubID is null
					
			Update t
			set t.pubID  = p.pubID
			from #tmpBrand t join 
					pubs p on t.PubCode = p.PubCode
					
			insert into BrandDetails
			(
				BrandID,
				PubID
			)	
			select 
				tb.BrandID, 
				tb.PubID 
			from
				#tmpBrand tb left outer join 
				BrandDetails bd  on tb.BrandID = bd.BrandID and tb.PubID = bd.PubID
			where
				bd.BrandDetailsID is null
		End
	End
		
	Drop table #tmpData
	Drop table #tmpBrand

END