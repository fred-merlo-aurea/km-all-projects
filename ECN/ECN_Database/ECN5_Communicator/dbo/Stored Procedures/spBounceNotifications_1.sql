CREATE PROCEDURE [dbo].[spBounceNotifications]
AS
BEGIN

declare @s varchar(1000),
                        @b varchar(MAX),
                        @lastbounceprocesseddate datetime
                        
                        
set @b = ''                    
set @s = 'Bounce Issue Notification : '
declare @BounceCheck table(lastbounceprocesseddate datetime, bounceengine varchar(50), bounceDateDiff int, sendEmail bit)

insert into @BounceCheck(bounceengine, lastbounceprocesseddate, bounceDateDiff, sendEmail)
select bc.BounceDomain, MAX(ActionDate), DATEDIFF(Minute, getdate(), MAX(ActionDate)), 0
from 
                        EmailActivityLog eal with (NOLOCK) 
                        join Blast b  with (NOLOCK)  on eal.blastID = b.blastID 
                        join ECN5_ACCOUNTS..Customer c  with (NOLOCK)  on c.CustomerID = b.CustomerID 
                        join ECN5_ACCOUNTS..Basechannel bc  with (NOLOCK)  on bc.BaseChannelID = c.BaseChannelID
where ActionTypeCode = 'bounce' and  ActionDate > DATEADD(HOUR,-4, GETDATE()) and BounceDomain in ('kmpsgroupbounce.com','kmemails.com','advanstaremails.com','tradepressemails.com','ubmcanonemails.com','pennwellemails.com','scrantongilletteemails.com')
group by bc.BounceDomain
order by 2 desc

update @BounceCheck
set sendEmail = 1
where (
                                    DATEPART(HH,getdate()) >= 6 and 
                                    DATEPART(HH,getdate()) <= 18 and 
                                    bounceDateDiff < -20
                        ) 
                        or 
                        (
                                    (
                                                DATEPART(HH,getdate()) < 6 or 
                                                DATEPART(HH,getdate()) > 18
                                    ) 
                                    and 
                                    (
                                                bounceDateDiff < -30
                                    )
                        )

--updated 4/5/2016 by Bill as notifications being sent out at night when the bounce count was legitimately 0 for over 15 minutes
            if exists (select top 1 * from @BounceCheck where sendEmail = 1)                       
            Begin
                        delete from @BounceCheck where sendEmail = 0
            
                        select @b = 'Bounce engine is behind in processing the bounces. Either a connection issue or it must be processing same batch over and over. Please look into this ASAP.
                        <br> BOUNCE ENGINES:<br>' + STUFF((Select ' ' + bc.bounceengine +'<br>'                        
                        from @BounceCheck bc
                        FOR XML PATH(''),type).value('.','nvarchar(MAX)'),1,1,'') + '<br>Query :
                        <br><br>'
                        
                        select @b = @b + STUFF((Select ' ' + 'select top 10 bc.BounceDomain, ActionDate, COUNT(*)
<br>from EmailActivityLog eal with (NOLOCK) 
<br>join Blast b  with (NOLOCK)  on eal.blastID = b.blastID 
<br>join ECN5_ACCOUNTS..Customer c  with (NOLOCK)  on c.CustomerID = b.CustomerID 
<br>join ECN5_ACCOUNTS..Basechannel bc  with (NOLOCK)  on bc.BaseChannelID = c.BaseChannelID
<br>where ActionTypeCode = ''bounce'' and bc.BounceDomain = ''' + bounceengine + '''  and ActionDate > DATEADD(HOUR,-4, GETDATE())
<br>group by bc.BounceDomain, actiondate
<br>order by ActionDate desc<br><br>'         
                                    FROM @BounceCheck
                                    FOR XML PATH(''),type).value('.','nvarchar(MAX)'),1,1,'') 
            End

            if @b <> ''
            Begin
                        EXEC msdb..sp_send_dbmail 
                                    @profile_name='SQLAdmin', 
                                    @recipients='bill.hipps@teamkm.com;sunil@teamkm.com;justin.welter@teamkm.com;pete.anderson@teamkm.com;brandon.peterson@teamkm.com', 
                                    @importance='High',
                                    @body_format = 'HTML',
                                    @subject=@s, 
                                    @body=@b
            End      
END