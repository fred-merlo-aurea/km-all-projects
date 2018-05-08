CREATE PROC [dbo].[spRunBlast] 
(
	@BlastID int,
	@OnlyCounts bit = 0 
	--@LogSuppressed bit
)
AS

BEGIN        
	SET NOCOUNT ON 
	
	DECLARE @CustomerID int,        	
			@GroupID int,        
			@FilterID int,        
			@BlastID_and_BounceDomain varchar(250),        
			@ActionType varchar(10),        
			@RefBlastID varchar(2000),
			@SupressionList varchar(2000)
			
	SELECT	@CustomerID = CustomerID, @GroupID = GroupID, @FilterID = ISNULL(FilterID, 0), @BlastID_and_BounceDomain = '', @RefBlastID = ISNULL(RefBlastID, ''),
			@SupressionList = ISNULL(BlastSuppression, '')			
	FROM	Blast
	WHERE	BlastID = @BlastID
	
	
	SET @ActionType = ''
	IF @FilterID in (2147483647, 2147483646, 2147483645, 2147483644)
	BEGIN
		IF @FilterID = 2147483647
		BEGIN
			SET @ActionType = 'unclick'
		END
		IF @FilterID = 2147483646
		BEGIN
			SET @ActionType = 'unopen_unclick'
		END
		IF @FilterID = 2147483645
		BEGIN
			SET @ActionType = 'unopen'
		END
		IF @FilterID = 2147483644
		BEGIN
			SET @ActionType = 'suppressed'
		END
		IF @FilterID = 2147483643
		BEGIN
			SET @ActionType = 'open'
		END
		IF @FilterID = 2147483642
		BEGIN
			SET @ActionType = 'click'
		END
	END
	EXEC	v_Blast_GetBlastEmailsListForDynamicContent @CustomerID, @BlastID, 
				@GroupID, @FilterID, @BlastID_and_BounceDomain, @ActionType, @RefBlastID, @SupressionList, @OnlyCounts, 0
END