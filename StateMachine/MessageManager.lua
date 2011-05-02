--------------------------------------------------------------------
--
-- MessageManager.lua
--
--------------------------------------------------------------------
require "Message"

local MessageQueue = {}

function SendMessage( message )
	if( message.GetSendTime() <= GetTime() ) then
		message.GetReceiver().HandleMessage( message )
	else
		table.insert( MessageQueue, message )
	end
end

function SendDelayedMessages()
	local idxsToRemove = {}
	for idx, msg in ipairs( MessageQueue ) do
		if( msg.GetSendTime() <= GetTime() ) then
			msg.GetReceiver().HandleMessage( msg )
			table.insert( idxsToRemove, idx )
		end
	end
	for _, idx in ipairs( idxsToRemove ) do
		table.remove( MessageQueue, idx )
	end
end