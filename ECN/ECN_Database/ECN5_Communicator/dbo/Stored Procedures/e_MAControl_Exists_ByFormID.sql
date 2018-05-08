CREATE PROCEDURE [dbo].[e_MAControl_Exists_ByFormID] 
	@FORMID int,
	@CONTROLTYPE varchar(50),
	@MASTATE varchar(50)
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 ECNID FROM [MAControl] MAC WITH (NOLOCK) JOIN [MarketingAutomation]  MA WITH (NOLOCK) ON MAC.MarketingAutomationID =MA.MarketingAutomationID
				WHERE ECNID = @FORMID AND (MA.State=@MASTATE AND MA.EndDate > GETDATE()) AND MAC.ControlType = @CONTROLTYPE) 
    SELECT 1 ELSE SELECT 0
END
