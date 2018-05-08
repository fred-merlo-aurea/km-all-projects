
CREATE PROCEDURE [dbo].[e_Service_Select_ServiceCode]  
@ServiceCode varchar(100)  
AS  
 SELECT *  
 FROM Service With(NoLock)  
 WHERE ServiceCode = @ServiceCode