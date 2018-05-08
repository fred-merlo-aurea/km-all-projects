
-- Procedure
CREATE PROC [dbo].[v_Blast_GetBlastEmailsListForDynamicContent_GetNoRecords] 
(
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID varchar(2000),
	@SupressionList varchar(2000),
	@OnlyCounts bit,
	@LogSuppressed bit = 1        
)
as

--declare @CustomerID int,        	
--	@BlastID int,
--	@GroupID int,        
--	@FilterID int,        
--	@BlastID_and_BounceDomain varchar(250),        
--	@ActionType varchar(10),        
--	@refBlastID varchar(2000),
--	@SupressionList varchar(2000),
--	@OnlyCounts bit,
--	@LogSuppressed bit
		
--set @CustomerID  =     	3053
--set @BlastID = 578456
--set @GroupID =    48310
--set @FilterID =        0
--set @BlastID_and_BounceDomain  = ''    
--set @ActionType  = ''      
--set @refBlastID  = ''
--set @SupressionList = ''
--set @OnlyCounts= 0
--set @LogSuppressed = 0

BEGIN        
	set NOCOUNT ON    

	select * from Emails where EmailID = -1

END
