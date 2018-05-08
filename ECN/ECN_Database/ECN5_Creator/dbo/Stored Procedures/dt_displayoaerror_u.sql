﻿CREATE PROCEDURE dbo.dt_displayoaerror_u
    @iObject int,
    @iresult int
as
	-- This procedure should no longer be called;  dt_displayoaerror should be called instead.
	-- Calls are forwarded to dt_displayoaerror to maintain backward compatibility.
	set nocount on
	exec dbo.dt_displayoaerror
		@iObject,
		@iresult

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dt_displayoaerror_u] TO PUBLIC
    AS [dbo];

