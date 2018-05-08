
CREATE PROCEDURE [dbo].usp_ResetIsOpenCloseLocked
 @DatabaseName varchar(50),
 @Pubcode varchar(10)

with execute as owner
AS

-- execute usp_ResetIsOpenCloseLocked @DatabaseName = 'SourceMediaMasterDB_test', @Pubcode = 'ABM'

-- execute usp_ResetIsOpenCloseLocked @DatabaseName = 'AINPublicationsMasterDB', @Pubcode = 'AIN'
-- execute usp_ResetIsOpenCloseLocked @DatabaseName = 'CEGMasterDB', @Pubcode = 'CEX'


declare @SQL varchar(max)
declare @RowCount int

--declare @DatabaseName varchar(50)
--declare @Pubcode varchar(10)
--set @DatabaseName = 'CEGMasterDB'
--set @Pubcode = 'CEX'




select @SQL = 'Select ''BEFORE'', p.PubCode, p.PubName, p.IsOpenCloseLocked from ' + @DatabaseName + '.dbo.Pubs p  where p.PubCode = ''' + @Pubcode + ''''
execute (@SQL)


select @SQL = 'Update p set p.IsOpenCloseLocked = 0 from ' + @DatabaseName + '.dbo.Pubs p where PubCode = ''' + @Pubcode + ''''
execute (@SQL)


select @SQL = 'Select ''AFTER'', p.PubCode, p.PubName, p.IsOpenCloseLocked from ' + @DatabaseName + '.dbo.Pubs p  where p.PubCode = ''' + @Pubcode + ''''
execute (@SQL)