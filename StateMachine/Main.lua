--------------------------------------------------------------------
--
-- Main.lua
-- main, and shit.
--
--------------------------------------------------------------------

require "AgentManager"
require "MessageManager"
require "Time"

print("Starting Main.lua");

--------------------------------------------------------------------
-- Initialize the pseudo random number generator
--------------------------------------------------------------------
math.randomseed( GetTime() )
math.random(); math.random(); math.random()

PRINTTIMESTAMPS = true
function Output( str )
	if( PRINTTIMESTAMPS ) then
		print( os.date( "%X") .. " - " .. str )
	else
		print( str )
	end
end

--------------------------------------------------------------------
-- Program Contents
--------------------------------------------------------------------

-- init
AgentManager_AddAgent( "Sean" )
AgentManager_AddAgent( "Charlie" )
AgentManager_AddAgent( "Crystal" )
AgentManager_AddAgent( "Erika" )
AgentManager_AddAgent( "Link" )
AgentManager_AddAgent( "Zelda" )

-- main loop
while( 1 ) do
	-- send messages that aren't immediately fired
	SendDelayedMessages()
	
	-- update the agents
	AgentManager_Update()
	
	-- wait (horrible hack, but it's what we've got)
	-- if you're trying to run this on something other than Mac OS X,
	-- you're gonna have to change it to whatever your system has
	if os.execute("/bin/sleep 0.5") ~= 0 then break end
end

--------------------------------------------------------------------
-- End Program Contents
--------------------------------------------------------------------
print("Ending Main.lua");