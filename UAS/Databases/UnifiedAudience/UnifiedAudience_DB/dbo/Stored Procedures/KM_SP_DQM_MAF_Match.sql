-- =============================================
-- Author:                            <Author,,Name>
-- Create date: <Create Date,,>
-- Description:    <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[KM_SP_DQM_MAF_Match] 
                -- Add the parameters for the stored procedure here
                                @gv_fname varchar(1024),
                                @gv_lname varchar(1024),
                                @gv_country varchar(1024),
                                @GV_ZZ_PAR_ZIP_STD varchar(1024),
                                @GV_ZZ_PAR_PRIMARY_STREET varchar(1024),
                                @GV_ZZ_PAR_EMAIL_STD varchar(1024),
                                @GV_ZZ_PAR_COMPANY_STD varchar(1024),
                                @GV_PHONE varchar(1024)

                   
AS
BEGIN
declare @GV_sqlStmt varchar(4000);

IF(@gv_fname  != 'NULL') SET @gv_fname  = '''' + @gv_fname  + ''''; 
IF(@gv_lname  != 'NULL') SET @gv_lname  = '''' + @gv_lname  + ''''; 
IF(@gv_country  != 'NULL') SET @gv_country  = '''' + @gv_country  + ''''; 
IF(@GV_ZZ_PAR_ZIP_STD  != 'NULL') SET @GV_ZZ_PAR_ZIP_STD  = '''' + @GV_ZZ_PAR_ZIP_STD  + ''''; 
IF(@GV_ZZ_PAR_PRIMARY_STREET  != 'NULL') SET @GV_ZZ_PAR_PRIMARY_STREET  = '''' + @GV_ZZ_PAR_PRIMARY_STREET  + ''''; 
IF(@GV_ZZ_PAR_EMAIL_STD  != 'NULL') SET @GV_ZZ_PAR_EMAIL_STD  = '''' + @GV_ZZ_PAR_EMAIL_STD  + ''''; 
IF(@GV_ZZ_PAR_COMPANY_STD  != 'NULL') SET @GV_ZZ_PAR_COMPANY_STD  = '''' + @GV_ZZ_PAR_COMPANY_STD  + ''''; 
IF(@GV_PHONE  != 'NULL') SET @GV_PHONE  = '''' + @GV_PHONE  + ''''; 

--SET @GV_sqlStmt  = 'SELECT a.*,b.ZZ_MAF_ACTION,b.ZZ_PAR_FNAME_STD,b.ZZ_PAR_FNAME_MATCH1,b.ZZ_PAR_FNAME_MATCH2,b.ZZ_PAR_FNAME_MATCH3,b.ZZ_PAR_FNAME_MATCH4,b.ZZ_PAR_FNAME_MATCH5,b.ZZ_PAR_FNAME_MATCH6,b.ZZ_PAR_LNAME_STD,b.ZZ_PAR_TITLE_STD,b.ZZ_PAR_COMPANY_STD,b.ZZ_PAR_COMPANY2,b.ZZ_PAR_COMPANY_MATCH1,b.ZZ_PAR_COMPANY_MATCH2,b.ZZ_PAR_ADDRESS_STD,b.ZZ_PAR_MAILSTOP_STD,b.ZZ_PAR_CITY_STD,b.ZZ_PAR_STATE_STD,b.ZZ_PAR_ZIP_STD,b.ZZ_PAR_PLUS4_STD,b.ZZ_PAR_FORZIP_STD,b.ZZ_PAR_POSTCODE,b.ZZ_PAR_EMAIL_STD,b.ZZ_PAR_USCAN_PHONE,b.ZZ_PAR_INTL_PHONE,b.ZZ_PAR_PRIMARY_NUMBER,b.ZZ_PAR_PRIMARY_PREFIX,b.ZZ_PAR_PRIMARY_STREET,b.ZZ_PAR_PRIMARY_TYPE,b.ZZ_PAR_PRIMARY_POSTFIX,b.ZZ_PAR_RR_BOX,b.ZZ_PAR_RR_NUMBER,b.ZZ_PAR_UNIT_DESCRIPTION,b.ZZ_PAR_UNIT_NUMBER,b.ZZ_PAR_POBOX FROM Subscriptions as a inner join Subscriptions_DQM as b on a.SubscriptionID = b.SubscriptionID where ';
SET @GV_sqlStmt  = 'SELECT a.*,B.* FROM Subscriptions as a inner join Subscriptions_DQM as b on a.SubscriptionID = b.SubscriptionID where ';
if(@gv_fname = 'NULL' and @gv_lname = 'NULL')
begin
                SET  @GV_sqlStmt  = @GV_sqlStmt  +  'b.ZZ_PAR_EMAIL_STD LIKE ' + @GV_ZZ_PAR_EMAIL_STD;
end
else
begin
                if(@GV_fname != 'NULL')
                SET  @GV_sqlStmt  = @GV_sqlStmt  + 'b.ZZ_PAR_FNAME_STD LIKE ' + @GV_FNAME + ' AND ';
                if(@GV_Lname != 'NULL')
                SET  @GV_sqlStmt  = @GV_sqlStmt  + 'b.ZZ_PAR_LNAME_STD LIKE ' + @GV_LNAME + ' AND ';
                SET  @GV_sqlStmt  = @GV_sqlStmt  + '(';             
--forigen country
                if(@GV_COUNTRY != 'NULL')
                SET  @GV_sqlStmt  = @GV_sqlStmt  + '(a.country LIKE ' + @GV_COUNTRY + '  AND b.ZZ_PAR_PRIMARY_STREET LIKE ' + @GV_ZZ_PAR_PRIMARY_STREET + '  and ( b.ZZ_PAR_ZIP_STD LIKE ' + @GV_ZZ_PAR_ZIP_STD + '  or b.ZZ_PAR_ZIP_STD is null)) ';
                else
--USA    
                SET @GV_sqlStmt  = @GV_sqlStmt  + '(a.COUNTRY is null AND b.ZZ_PAR_ZIP_STD LIKE ' + @GV_ZZ_PAR_ZIP_STD + '  AND b.ZZ_PAR_PRIMARY_STREET LIKE ' + @GV_ZZ_PAR_PRIMARY_STREET + ' )'; 
                
                SET @GV_sqlStmt  = @GV_sqlStmt  + ' OR (b.ZZ_PAR_EMAIL_STD LIKE ' + @GV_ZZ_PAR_EMAIL_STD + ' )';
                SET @GV_sqlStmt  = @GV_sqlStmt  + ' OR (b.ZZ_PAR_COMPANY_STD LIKE ' + @GV_ZZ_PAR_COMPANY_STD + '  and ( a.country LIKE ' + @GV_COUNTRY + '  or a.COUNTRY is null) AND ( b.ZZ_PAR_ZIP_STD LIKE ' + @GV_ZZ_PAR_ZIP_STD + '  or  b.ZZ_PAR_ZIP_STD is null))'; 

                if(@GV_PHONE != 'NULL')
                SET @GV_sqlStmt  = @GV_sqlStmt  + ' OR (b.ZZ_PAR_USCAN_PHONE LIKE ' + @GV_PHONE + '  or b.ZZ_PAR_INTL_PHONE LIKE ' + @GV_PHONE + ' )';
                SET  @GV_sqlStmt  = @GV_sqlStmt  + ')';             
end

                -- SET NOCOUNT ON added to prevent extra result sets from
                -- interfering with SELECT statements.
                SET NOCOUNT ON;
    PRINT( @GV_sqlStmt);
                exec(@GV_sqlStmt);

END
GO
GRANT VIEW DEFINITION
    ON OBJECT::[dbo].[KM_SP_DQM_MAF_Match] TO [webuser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[KM_SP_DQM_MAF_Match] TO [webuser]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[KM_SP_DQM_MAF_Match] TO [webuser]
    AS [dbo];

