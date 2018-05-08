
CREATE Procedure [dbo].[sp_InsertIPDomain](  
 @xmlDocument Text   
)    
as   
BEGIN  
  
	set nocount on    
	  
	DECLARE @docHandle int    
	  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument    
	  
	INSERT INTO domainIPs(IP, domainName) 
	SELECT IP, Domain FROM OPENXML(@docHandle, N'/ROOT/ENTRY')
	WITH (
			IP varchar(50) '@IP',
			Domain varchar(100) '@Domain'
		) 
		  
	EXEC sp_xml_removedocument @docHandle  
    
END

--USE [ecn_Temp]
--GO
--/****** Object:  StoredProcedure [dbo].[sp_InsertIPDomain]    Script Date: 09/21/2011 11:44:02 ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO

--ALTER Procedure [dbo].[sp_InsertIPDomain](  
-- @xmlDocument Text   
--)    
--as   
--BEGIN  
  
--	set nocount on    
	  
--	DECLARE @docHandle int    
	  
--	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument    
	  
--	CREATE TABLE #templist
--	(
--		IP varchar(50),
--		Domain varchar(100)
--	)
	
--	INSERT INTO #templist(IP,Domain) 
--	SELECT IP, Domain FROM OPENXML(@docHandle, N'/ROOT/ENTRY')
--	WITH (
--			IP varchar(50) '@IP',
--			Domain varchar(100) '@Domain'
--		) 
		  
--	EXEC sp_xml_removedocument @docHandle 
	
--	INSERT INTO domainIPs(domainName, IP)
--	SELECT t.Domain, t.IP
--	FROM #templist t
--		JOIN domainIPs d on t.Domain = d.domainName AND t.IP = d.IP
--	WHERE d.domainName is null 
	
--	DROP TABLE #templist
    
--END




