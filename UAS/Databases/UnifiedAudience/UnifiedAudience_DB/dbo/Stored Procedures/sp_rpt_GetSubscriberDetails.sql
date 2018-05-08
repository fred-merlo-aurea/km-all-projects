CREATE PROCEDURE [dbo].[sp_rpt_GetSubscriberDetails]
	@ProductID int,
	@AddKillID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT s.SubscriberID, s.Email, s.FirstName, s.LastName, s.COMPANY, s.TITLE, s.Address1, s.Address2, s.CITY, s.RegionCode, s.ZipCode, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, a.CategoryCodeID as CAT,
		c.CategoryCodeValue as CategoryName, a.TransactionCodeID as XACT, s.Plus4, s.County, sub.QSourceID, q.QSourceName + '' + q.QSourceCode + '' as Qsource, 
		sub.QSourceDate, sub.SubsrcTypeID, p.PublicationCode as pubcode, sub.Copies
	from Subscriber s 
		JOIN Subscription sub ON s.SubscriberID = sub.SubscriberID 
		JOIN Action a ON a.ActionID = sub.ActionID_Current 
		JOIN CategoryCode c ON c.CategoryCodeID = a.CategoryCodeID 
		LEFT outer join QualificationSource q on q.QSourceID = sub.QSourceID 
		join Publication p ON p.PublicationID = sub.PublicationID
	WHERE p.PublicationID = @ProductID AND
	sub.AddRemoveID > 0

END