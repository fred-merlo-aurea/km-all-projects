CREATE PROCEDURE [dbo].[spScheduleReportExportErrorNotifications]
AS
BEGIN
	declare @s varchar(1000), @b varchar(MAX)
	set @b = ''                    
	set @s = 'ECN Schedule Report Export Error Notification'

	if exists (select top 1 * from ReportQueue where SendTime = DATEADD(HH, -1, DATEADD(HH,DATEDIFF(HH, 0, GETDATE()),0)) and [Status] = 'Pending')                       
	Begin
		select @b = 'ECN Schedule Report Export has a pending report for the past hour. Please look into this ASAP.<br><br>'            
		select @b = @b + 'Select * from ReportQueue
		<br>where SendTime = DATEADD(HH, -1, DATEADD(HH,DATEDIFF(HH, 0, GETDATE()),0)) and [Status] = ''Pending'''
	End

	if @b <> ''
	Begin
		EXEC msdb..sp_send_dbmail 
			@profile_name='SQLAdmin', 
			@recipients='agustin.mendoza@teamkm.com;brandon.peterson@teamkm.com', 
			@importance='High',
			@body_format='HTML',
			@subject=@s, 
			@body=@b
	End
END
