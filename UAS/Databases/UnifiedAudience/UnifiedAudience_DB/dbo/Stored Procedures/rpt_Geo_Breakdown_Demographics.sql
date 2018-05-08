CREATE proc [dbo].[rpt_Geo_Breakdown_Demographics]
(       
	@ReportID int ,    
	@ProductID int, 
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@WaveMail varchar(100) = ''
)
AS
BEGIN 
	
	SET NOCOUNT ON
 
	DECLARE @magazineID int, @ResponseGroup varchar(50)     
	SELECT @MagazineID = ProductID, @ResponseGroup=Row 
	FROM reports 
	WHERE reportID = @reportID    

	DECLARE @magazineCode varchar(20)
	SELECT @magazineCode  = PubCode 
	from Pubs 
	where PubID = @ProductID

	CREATE TABLE #SubscriptionID (SubscriptionID int, copies int)   

	INSERT INTO #SubscriptionID   
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter 
		@ProductID, 
		@CategoryIDs,
		@CategoryCodes,
		@TransactionIDs,
		@TransactionCodes,
		@QsourceIDs,
		@StateIDs,
		@CountryIDs,
		@Email,
		@Phone,
		@Mobile,
		@Fax,
		@ResponseIDs,
		@Demo7,		
		@Year,
		@startDate,		
		@endDate,
		@AdHocXML 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	CREATE TABLE #responseID (responseID int)   

	INSERT INTO #responseID 
	select p.CodesheetID 
	from PubSubscriptionDetail p with(nolock) 
		JOIN CodeSheet c with(nolock) ON c.CodeSheetID = p.CodesheetID 
	where c.PubID = @MagazineID and c.ResponseGroup = @ResponseGroup  

	BEGIN  
		SELECT isnull(st.ZipCodeRange,'') + ' ' + st.RegionCode as state,
			st.RegionName,  
			st.sort_order,   
			st.CountryID,   
			st.country_sort_order,	
			sd.CodesheetID,    
			c.Responsevalue + c.Responsedesc  AS 'DESCRIPTION',          
			sum(s.copies) as 'Totals'      
		From #SubscriptionID sf with(nolock) 
			join PubSubscriptions s with(nolock) on sf.SubscriptionID = s.SubscriptionID 
			join PubSubscriptionDetail sd with(nolock) on sf.SubscriptionID = sd.SubscriptionID 
			join CodeSheet c with(nolock) on sd.CodesheetID = c.CodeSheetID 
			join UAD_Lookup..Region st with(nolock) ON st.RegionCode = s.RegionCode    
		WHERE responsegroup = @ResponseGroup  and c.CodeSheetID in (select responseID from #responseID)  
		group by isnull(st.ZipCodeRange,'') + ' ' + st.RegionCode,  
			st.RegionName,  
			st.sort_order,   
			st.CountryID,   
			st.country_sort_order,	        
			sd.CodesheetID,    
			c.Responsevalue + c.Responsedesc  
	END  

	DROP TABLE #SubscriptionID  
	DROP TABLE #responseID
	
END