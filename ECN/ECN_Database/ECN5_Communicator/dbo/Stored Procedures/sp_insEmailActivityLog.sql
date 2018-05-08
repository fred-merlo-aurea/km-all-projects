CREATE  PROCEDURE [dbo].[sp_insEmailActivityLog]
(
 @xmldoc Text   
)
AS  
  
BEGIN   
  
 DECLARE @xmlHandle INT  
  
 EXEC sp_xml_preparedocument @xmlHandle OUTPUT, @xmlDoc  
  
Insert into EmailActivityLog (EmailID, BlastID, ActionTypeCode, ActionValue, ActionDate)  
 SELECT * FROM OPENXML (@xmlHandle, '/EmailActivityLog/Email', 2) WITH  
  (  
   EmailID  int '@id',  
   BlastID  int '@BlastID',  
   ActionTypeCode varchar(25) '@ActionTypeCode',  
   ActionValue  varchar(25) '@ActionValue' ,
   ActionDate  datetime	 '@ActionDate'
  )  
  
 -- Important to specify this  
 EXEC sp_xml_removedocument @xmlHandle  
END
