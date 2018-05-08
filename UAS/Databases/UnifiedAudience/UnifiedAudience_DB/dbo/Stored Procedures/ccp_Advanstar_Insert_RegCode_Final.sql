CREATE PROCEDURE [dbo].[ccp_Advanstar_Insert_RegCode_Final]
AS
BEGIN

	set nocount on

	DECLARE @temp TABLE (id varchar(250), tickettype varchar(250), ticketsubt varchar(250), regcode varchar(250))
	
	INSERT INTO @temp
	SELECT rc.Person_ID, rc.Ticket_Type, rc.Ticket_Stub, rcc.REGCODE FROM tempAdvanstarRegCode rc
	JOIN tempAdvanstarRegCodeCompare rcc ON rc.Ticket_Type = rcc.TICKETTYPE AND rc.Ticket_Stub = rcc.TICKETSUBT;
	
	INSERT INTO tempAdvanstarRegCodeFinal
	SELECT DISTINCT
       'CBIATTEN' as PubCode,
       p.id as Person_ID,
       RegCode = ISNULL(Stuff((SELECT DISTINCT  ', ' + regcode AS [text()]
                                                FROM @temp C1
                                                WHERE c1.id = p.id
                                                FOR XML PATH ('')),1,1,''),'')
	FROM @temp p

END
GO