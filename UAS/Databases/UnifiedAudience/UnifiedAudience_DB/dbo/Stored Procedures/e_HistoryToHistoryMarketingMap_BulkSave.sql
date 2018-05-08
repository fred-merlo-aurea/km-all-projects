CREATE PROCEDURE [dbo].[e_HistoryToHistoryMarketingMap_BulkSave]
@xml xml
AS
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		HistoryID int,
		HistoryMarketingMapID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 HistoryID,HistoryMarketingMapID
	)  
	
	SELECT HistoryID,HistoryMarketingMapID
	FROM OPENXML(@docHandle,N'/XML/History')
	WITH
	(
		HistoryID int 'HistoryID' ,
		HistoryMarketingMapID int 'HistoryMarketingMapID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	INSERT INTO HistoryToHistoryMarketingMap (HistoryID,HistoryMarketingMapID)
	SELECT HistoryID,HistoryMarketingMapID FROM @import

END