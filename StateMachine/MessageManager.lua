--------------------------------------------------------------------
--
-- MessageManager.lua
--
--------------------------------------------------------------------
require "Message"

local MessageQueue = {} -- not actually a queue...

--------------------------------------------------------------------
-- Send a message, if there's a delay, add it to the queue
--------------------------------------------------------------------
function SendMessage( message )
	if( message.sendTime <= GetTime() ) then
		message.receiver.HandleMessage( message )
	else
		table.insert( MessageQueue, message )
	end
end

--------------------------------------------------------------------
-- Send any messages whose expiration dates have come up
--------------------------------------------------------------------
function SendDelayedMessages()
	local idxsToRemove = {}
	for idx, msg in ipairs( MessageQueue ) do
		if( msg.sendTime <= GetTime() ) then
			msg.receiver.HandleMessage( msg )
			table.insert( idxsToRemove, idx )
		end
	end
	for _, idx in ipairs( idxsToRemove ) do
		table.remove( MessageQueue, idx )
	end
end