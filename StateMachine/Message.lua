--------------------------------------------------------------------
--
-- Message.lua
--
--------------------------------------------------------------------

Message = {}
function Message.new( sender, receiver, message, sendTime, params )
	local self = {};
	
	local m_Sender 		= sender;
	local m_Receiver	= receiver;
	local m_Message		= message;
	local m_SendTime	= sendTime;
	local m_Params		= params;
	
	function self.GetSender()
		return m_Sender
	end
	function self.GetReceiver()
		return m_Receiver
	end
	function self.GetMessage()
		return m_Message
	end
	function self.GetSendTime()
		return m_SendTime
	end
	function self.GetParams()
		return m_Params
	end
	
	return self
end