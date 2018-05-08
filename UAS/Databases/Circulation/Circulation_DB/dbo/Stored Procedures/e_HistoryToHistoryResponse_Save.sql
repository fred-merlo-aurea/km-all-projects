CREATE PROCEDURE [dbo].[e_HistoryToHistoryResponse_Save]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		HistoryID int,
		HistoryResponseID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 HistoryID,HistoryResponseID
	)  
	
	SELECT HistoryID,HistoryResponseID
	FROM OPENXML(@docHandle,N'/XML/History')
	WITH
	(
		HistoryID int 'HistoryID' ,
		HistoryResponseID int 'HistoryIDResponseID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	INSERT INTO HistoryToHistoryResponse (HistoryID,HistoryResponseID)
	SELECT HistoryID,HistoryResponseID FROM @import
