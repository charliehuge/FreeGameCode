--------------------------------------------------------------------
--
-- Message.lua
--
--------------------------------------------------------------------

Message = {}
function Message.new( sender, receiver, message, sendTime, params )
	local self = {};
	
	self.sender		= sender
	self.receiver	= receiver
	self.message	= message
	self.sendTime	= sendTime
	self.params		= params

	return self
end