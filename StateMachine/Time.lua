require "socket"

function GetTime()
	return socket.gettime() * 1000
end