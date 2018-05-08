CREATE PROCEDURE [dbo].[e_PubSubscriptions_Update_ReqFlags]
@ProductID int,
@IssueID int
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @id int = (SELECT CodeTypeId FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Requester Flag')
	DECLARE @N int = (SELECT CodeId FROM UAD_Lookup..Code WHERE CodeValue = 'N' AND CodeTypeId = @id)
	DECLARE @Y int = (SELECT CodeId FROM UAD_Lookup..Code WHERE CodeValue = 'Y' AND CodeTypeId = @id)
	DECLARE @A int = (SELECT CodeId FROM UAD_Lookup..Code WHERE CodeValue = 'A' AND CodeTypeId = @id)

	
	UPDATE ps
	SET ReqFlag = (CASE WHEN CategoryCodeValue IN (61,62,63) THEN @A 			
					WHEN CategoryCodeValue IN (20,21,22,23,24,27,28,29) THEN @Y
					WHEN CategoryCodeValue IN (30,31,32,33,34,35) THEN @Y
					WHEN CategoryCodeValue = 10 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 11 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 17 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 18 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue IN (50,51,52,55,56,57,58) THEN @N
					WHEN CategoryCodeValue = 10 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 10 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 11 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 11 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 17 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 17 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 18 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 18 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					END)
	FROM PubSubscriptions ps
		LEFT JOIN UAD_LookUp..Code c ON ps.PubQSourceID = c.CodeId
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
	WHERE ps.PubID = @ProductID


	UPDATE ic
	SET ReqFlag = (CASE WHEN CategoryCodeValue IN (61,62,63) THEN @A 			
					WHEN CategoryCodeValue IN (20,21,22,23,24,27,28,29) THEN @Y
					WHEN CategoryCodeValue IN (30,31,32,33,34,35) THEN @Y
					WHEN CategoryCodeValue = 10 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 11 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 17 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue = 18 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) >= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @Y
					WHEN CategoryCodeValue IN (50,51,52,55,56,57,58) THEN @N
					WHEN CategoryCodeValue = 10 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 10 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 11 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 11 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 17 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 17 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					WHEN CategoryCodeValue = 18 AND ISNULL(CodeValue, '') in ('G','H','S','I','J','K','L','M','N') THEN @N
					WHEN CategoryCodeValue = 18 AND ISNULL(NULLIF(Qualificationdate, ''), GETDATE()) <= DATEADD(day, -1095, GETDATE()) AND ISNULL(CodeValue, '') in ('A','B','C','D','E','F','Q','R') THEN @N
					END)
	FROM IssueCompDetail ic
		LEFT JOIN UAD_LookUp..Code c ON ic.PubQSourceID = c.CodeId
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ic.PubCategoryID
	WHERE ic.IssueCompID = (SELECT IssueCompID FROM IssueComp i WHERE IssueId = @IssueID AND i.IsActive = 1)

END

--create table #Tmp (id int, demo7 varchar(10), PubCategoryID int, PubQSourceID int, Qualificationdate datetime, ReqFlag int)
--insert into #Tmp VALUES(1,'A', 101, 1874, '10-22-2000', 0) -- N
--insert into #Tmp VALUES(2,'C', 102, 1890, '', 0) -- N
--insert into #Tmp VALUES(3,'C', 105, 1890, '', 0) -- N
--insert into #Tmp VALUES(4,'C', 103, 0, '', 0) --N
--insert into #Tmp VALUES(5,'A', 119, 1874, '', 0) --Y
--insert into #Tmp VALUES(6,'C', 117, 0, '', 0) -- Y
--insert into #Tmp VALUES(7,'C', 111, 0, '', 0) -- A

--select t.id,t.demo7, c2.CodeValue as QSource, cc.CategoryCodeValue, c.CodeValue as ReqFlag, ISNULL(NULLIF(t.Qualificationdate, ''),GETDATE()) as Qualificationdate FROM #Tmp t
--left join UAD_Lookup..Code c on c.CodeId = t.ReqFlag 
--left join UAD_Lookup..Code c2 on c2.CodeId = t.PubQSourceID
--left join UAD_Lookup..CategoryCode cc on cc.CategoryCodeID = t.PubCategoryID
--order by id

--drop table #Tmp