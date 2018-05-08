CREATE PROCEDURE [dbo].[e_BlastSingle_GetRefBlastID] 
	@BlastID int,
	@CustomerID int,
	@EmailID int = 0,
	@BlastType varchar(10)

	--declare @BlastID int
	--declare @CustomerID int
	--set @BlastID = 818088
	--set @CustomerID = 3634
AS     
BEGIN     		
DECLARE @RefBlastID int = 0
DECLARE @GroupID int = null
DECLARE @LoopBlastID int = 0
DECLARE @WhileIndex int = 0
IF @BlastType = 'layout'
	begin
		IF EXISTS 
		(
			SELECT 
				TOP 1 bs.RefBlastID
			FROM 
				BlastSingles bs WITH (NOLOCK)
				join LayoutPlans lp WITH (NOLOCK) on bs.BlastID = lp.BlastID and lp.IsDeleted = 0 and lp.CustomerID = @CustomerID
			WHERE 
				bs.BlastID = @BlastID AND 
				bs.EmailID = @EmailID AND
				bs.IsDeleted = 0
		) 
		BEGIN
			SELECT @RefBlastID = bs.RefBlastID
			FROM 
				BlastSingles bs WITH (NOLOCK)
				join LayoutPlans lp WITH (NOLOCK) on bs.BlastID = lp.BlastID and lp.IsDeleted = 0 and lp.CustomerID = @CustomerID
			WHERE 
				bs.BlastID = @BlastID AND 
				bs.EmailID = @EmailID AND
				bs.IsDeleted = 0 
			
			SET @GroupID = null
			SET @LoopBlastID = @RefBlastID
			SET @WhileIndex = 0
			WHILE @GroupID is null and @WhileIndex < 10
			BEGIN
				SELECT @LoopBlastID = refBlastID from BlastSingles bs with(nolock) where bs.BlastID = @LoopBlastID and bs.EmailID = @EmailID
				SELECT @GroupID = GroupID from Blast b with(nolock) where b.blastid = @LoopBlastID
				Set @WhileIndex = @WhileIndex + 1
			END
			if(@GroupID is not null)
			BEGIN
				SELECT @LoopBlastID
			END
			ELSE
			BEGIN
				SELECT 0
			END
		END
		ELSE 
			SELECT 0
	END
ELSE IF @BlastType = 'noopen'
	Begin
		IF EXISTS 
			(
				SELECT 
					TOP 1 bs.RefBlastID
				FROM 
					BlastSingles bs WITH (NOLOCK)
					join TriggerPlans tp WITH (NOLOCK) on bs.BlastID = tp.BlastID and tp.IsDeleted = 0 and tp.CustomerID = @CustomerID
				WHERE 
					bs.BlastID = @BlastID AND 
					bs.EmailID = @EmailID AND
					bs.IsDeleted = 0
			) 
			BEGIN
				SELECT @RefBlastID = bs.RefBlastID
				FROM 
					BlastSingles bs WITH (NOLOCK)
					join TriggerPlans tp WITH (NOLOCK) on bs.BlastID = tp.BlastID and tp.IsDeleted = 0 and tp.CustomerID = @CustomerID
				WHERE 
					bs.BlastID = @BlastID AND 
					bs.EmailID = @EmailID AND
					bs.IsDeleted = 0 

				SET @GroupID = null
				SET @LoopBlastID = @RefBlastID
				SET @WhileIndex = 0
				WHILE @GroupID is null and @WhileIndex < 10
				BEGIN
					SELECT @LoopBlastID = refBlastID from BlastSingles bs with(nolock) where bs.BlastID = @LoopBlastID and bs.EmailID = @EmailID
					SELECT @GroupID = GroupID from Blast b with(nolock) where b.blastid = @LoopBlastID
					Set @WhileIndex = @WhileIndex + 1
				END

			if(@GroupID is not null)
				BEGIN
					SELECT @LoopBlastID
				END
			ELSE
				BEGIN
					SELECT 0
				END
			END
		ELSE 
			SELECT 0
		
	END
END