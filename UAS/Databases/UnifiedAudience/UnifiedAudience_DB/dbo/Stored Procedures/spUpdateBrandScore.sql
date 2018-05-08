CREATE PROC dbo.spUpdateBrandScore
@BrandId INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON

	IF @BrandId IS NULL

	BEGIN

		TRUNCATE TABLE BrandScore;

		INSERT INTO BrandScore (
			SubscriptionId,
			BrandId,
			Score,
			CreatedDate,
			CreateUser)

		SELECT Main.SubscriptionID, Main.BrandID, Score = ISNULL(c1,0) + ISNULL(c2,0) + ISNULL(c3,0) , GETDATE(), SYSTEM_USER
		FROM 
			(
			SELECT s.subscriptionID, bd.BrandID, ISNULL(SUM(p.Score), 0) AS c1
			FROM Subscriptions s WITH (NOLOCK) 
				INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID 
				INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
				INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
			GROUP BY s.subscriptionID, bd.brandid
			) Main
				LEFT OUTER JOIN 
				(
				SELECT ps.subscriptionID, bd.BrandID, ISNULL(COUNT(DISTINCT ClickActivityID), 0) AS c2
				FROM PubSubscriptions ps WITH (NOLOCK) 
					INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
					INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
					JOIN SubscriberClickActivity sc WITH (NOLOCK) ON ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> '' 
				GROUP BY ps.subscriptionID, bd.brandid
				) Op ON Main.subscriptionID = Op.SubscriptionID AND Main.BrandID = Op.BrandID
				LEFT OUTER JOIN 
				(
				SELECT s.subscriptionID, bd.BrandID, ISNULL(COUNT(DISTINCT OpenActivityID), 0) AS c3
				FROM Subscriptions s WITH (NOLOCK) 
					INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID 
					INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
					INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
					JOIN SubscriberOpenActivity so WITH (NOLOCK) ON ps.PubSubscriptionID = so.PubSubscriptionID 
				GROUP BY s.subscriptionID, bd.brandid
				) Cl ON Main.subscriptionID = Cl.SubscriptionID AND Main.BrandID = Cl.BrandID
			ORDER BY 1,2
	END

	IF @BrandId IS NOT NULL
		BEGIN
			DELETE FROM BrandScore 
			WHERE BrandId = @BrandId

				INSERT INTO BrandScore (
					SubscriptionId,
					BrandId,
					Score,
					CreatedDate,
					CreateUser)

				SELECT Main.SubscriptionID, Main.BrandID, Score = ISNULL(c1,0) + ISNULL(c2,0) + ISNULL(c3,0) , GETDATE(), SYSTEM_USER
				FROM 
				(
				SELECT s.subscriptionID, bd.BrandID, ISNULL(SUM(p.Score), 0) AS c1
				FROM Subscriptions s WITH (NOLOCK) 
					INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID 
					INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
					INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
				WHERE bd.BrandID = @BrandID
				GROUP BY s.subscriptionID, bd.brandid
				) Main
					LEFT OUTER JOIN 
					(
					SELECT  ps.subscriptionID, bd.BrandID, ISNULL(COUNT(DISTINCT ClickActivityID), 0) AS c2
					FROM PubSubscriptions ps WITH (NOLOCK) 
						INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
						INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
						JOIN SubscriberClickActivity sc WITH (NOLOCK) ON ps.PubSubscriptionID = sc.PubSubscriptionID and ISNULL(link,'') <> '' 
					WHERE bd.BrandID = @BrandID
					GROUP BY ps.subscriptionID, bd.brandid
					) Op ON Main.subscriptionID = Op.SubscriptionID AND Main.BrandID = Op.BrandID
					LEFT OUTER JOIN 
					(
					SELECT s.subscriptionID, bd.BrandID, ISNULL(COUNT(DISTINCT OpenActivityID), 0) AS c3
					FROM Subscriptions s WITH (NOLOCK) 
						INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON s.SubscriptionID = ps.SubscriptionID 
						INNER JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
						INNER JOIN BrandDetails bd  WITH (NOLOCK)  ON P.PubID = bd.pubid
						JOIN SubscriberOpenActivity so WITH (NOLOCK) ON ps.PubSubscriptionID = so.PubSubscriptionID 
					WHERE bd.BrandID = @BrandID
					GROUP BY s.subscriptionID, bd.brandid
					) Cl ON Main.subscriptionID = Cl.SubscriptionID AND Main.BrandID = Cl.BrandID
			ORDER BY 1,2
		END

END