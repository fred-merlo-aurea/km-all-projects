﻿CREATE PROCEDURE [dbo].[e_SourceFile_Select_SourceFileID] 
@SourceFileID int
AS
SELECT * 
FROM SourceFile With(NoLock)
Where SourceFileID = @SourceFileID
GO
