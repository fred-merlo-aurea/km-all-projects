CREATE PROCEDURE [dbo].[sp_deleteEmail]  
(  
 @EmailID int,  
 @GroupID int  
)        
AS        
Begin      
 set nocount on    
  
 DELETE FROM EmailGroups where EmailID=@EmailID AND GroupID = @GroupID  
 
 if exists (select GroupID from Groups where  GroupID = @GroupID and isnull(MasterSupression,0) = 1)
 Begin
	update EmailGroups
	set SubscribeTypeCode = 'S'
	Where EmailID = @EmailID and SubscribeTypeCode = 'M'
 End
  
/* 
    Commented the following sql's 'cos [CUSTOMER]  complain the delete emails take a 
    long time to delete just one record - ashok (12/12/06) 
    sp_Cleanup job will cleanup the orphan records in the other Tables 
*/

/*
 DELETE FROM EmailDataValues   
 WHERE emailID = @EmailID and  
   GroupDataFieldsID in (select GroupDataFieldsID from GroupDataFields where groupID = @GroupID)  
  
 if (select count(emailID) FROM EmailGroups where EmailID = @EmailID) = 0  
  DELETE FROM Emails WHERE EmailID = @EmailID  
*/
End
