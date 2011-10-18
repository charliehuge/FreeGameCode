--------------------------------------------------------------------
--
--	Time.lua
--
--		Because I like milliseconds, and can't get them without
--		a dependency on a network library.
--
--		You need LuaSocket installed for this to work.
--
--------------------------------------------------------------------
require "socket"

function GetTime()
	return socket.gettime() * 1000
end