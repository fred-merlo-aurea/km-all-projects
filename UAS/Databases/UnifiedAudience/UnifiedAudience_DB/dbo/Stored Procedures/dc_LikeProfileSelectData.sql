CREATE PROCEDURE dc_LikeProfileSelectData
@TableName varchar(250)
AS
BEGIN

	set nocount on

	IF (EXISTS (SELECT * 
					 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME in ('tmpStdProf_'+ @TableName, 'tmpPremProf_'+ @TableName)))
		BEGIN
			declare @return varchar(2000) = 'select * from UAD_TEMP.dbo.tmpStdProf_'+ @TableName + ' s full join UAD_TEMP.dbo.tmpPremProf_'+ @TableName + ' p on s.SubscriptionID = p.SubscriptionID'
			exec(@return);
    
			declare @stdDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpStdProf_'+ @TableName
			exec(@stdDrop)
			declare @premDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpPremProf_'+ @TableName
			exec(@premDrop)	
		END
	ELSE IF (EXISTS (SELECT * 
					 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'UAD_TEMP.dbo.tmpStdProf_'+ @TableName))
		BEGIN
			declare @returnStd varchar(2000) = 'select * from UAD_TEMP.dbo.tmpStdProf_'+ @TableName 
			exec(@returnStd);
    
			declare @stdOnlyDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpStdProf_'+ @TableName
			exec(@stdOnlyDrop)
		END
	ELSE IF (EXISTS (SELECT * 
					 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'UAD_TEMP.dbo.tmpPremProf_'+ @TableName))
		BEGIN
			declare @returnPrem varchar(2000) = 'select * from UAD_TEMP.dbo.tmpPremProf_'+ @TableName 
			exec(@returnPrem);
    
			declare @premOnlyDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpPremProf_'+ @TableName
			exec(@premOnlyDrop)	
		END

END