--------------------------------------------------------------------
--
--	Tools.lua
--
--		Bits that don't go anywhere else
--
--------------------------------------------------------------------

--------------------------------------------------------------------
-- table.copy - returns a shallow copy of the table
--------------------------------------------------------------------
function table.copy( source )
	local copied = {}
	
	for k, v in pairs( source ) do
		copied[ k ] = v
	end
	
	return copied
end

--------------------------------------------------------------------
-- Output to console
--------------------------------------------------------------------
PRINTTIMESTAMPS = true
function Output( str )
	if( PRINTTIMESTAMPS ) then
		print( os.date( "%X") .. " - " .. str )
	else
		print( str )
	end
end